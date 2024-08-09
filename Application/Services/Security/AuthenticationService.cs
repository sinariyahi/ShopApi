using Application.Interfaces.Base;
using Application.Interfaces.Security;
using Application.Interfaces.Sms;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Authorization;
using Infrastructure.Models.Base;
using Infrastructure.Models.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomEncryption customEncryption;
        private readonly ILogger logger;
        public IConfiguration configuration { get; }
        private readonly Configs configs;
        private readonly BIContext context;
        private readonly IEmailTemplateService emailTemplateService;
        private readonly ISmsService _smsService;

        public AuthenticationService(IOptions<Configs> options, BIContext context, ICustomEncryption customEncryption,
            IConfiguration configuration, ILogger<AuthenticationService> logger,
            IEmailTemplateService emailTemplateService, ISmsService smsService)
        {
            this.configs = options.Value;
            this.context = context;
            this.customEncryption = customEncryption;
            this.configuration = configuration;
            this.logger = logger;
            this.emailTemplateService = emailTemplateService;
            this._smsService = smsService;

        }

        private async Task InsertUpdateRefreshTokenAsync(Guid userId, string refreshToken, string userIP, string userBrowser)
        {
            //create UserLogin & insert to db
            var lastUserLogin = context.UserRefreshTokens.SingleOrDefault(q => q.UserId == userId);
            if (lastUserLogin != null)
            {
                //update
                lastUserLogin.RefreshToken = refreshToken;
                lastUserLogin.CreateDate = DateTime.Now;
                await context.SaveChangesAsync();
            }
            else
            {
                //insert
                var userLoginNew = new UserRefreshToken
                {
                    CreateDate = DateTime.Now,
                    IsValid = true,
                    RefreshToken = refreshToken,
                    UserId = userId,
                    UserIP = userIP,
                    UserBrowser = userBrowser,
                };

                await context.AddAsync(userLoginNew);
                await context.SaveChangesAsync();
            }

            #region Insert in RefreshTokenHistory Table
            var history = new UserRefreshTokenHistory
            {
                CreateDate = DateTime.Now,
                RefreshToken = refreshToken,
                UserId = userId,
                UserIP = userIP,
                UserBrowser = userBrowser,
            };
            await context.AddAsync(history);
            await context.SaveChangesAsync();
            #endregion
        }
        public async Task<ShopActionResult<bool>> CheckUserIsExsits(string phoneNumber)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(f => f.UserName == phoneNumber && f.UserType == UserType.Customer);
            if (user != null)
            {
                var smsResult = await _smsService.SendSmsForgetPassword(phoneNumber);

                if (smsResult != null)
                {
                    result.IsSuccess = smsResult.IsSuccess;
                    result.Message = smsResult.Message;
                }


            }
            else
            {
                result.IsSuccess = false;
             //   result.Message = MessagesFA.NotFound;
            }

            return result;
        }

        public async Task<ShopActionResult<bool>> CheckUserIsExsitsForActive(string phoneNumber)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(f => f.UserName == phoneNumber && f.UserType == UserType.Customer);

            if (user != null)
            {

                var smsResult = await _smsService.SendSmsForActiveUser(phoneNumber);

                if (smsResult != null)
                {
                    result.IsSuccess = smsResult.IsSuccess;
                    result.Message = smsResult.Message;
                }


            }
            else
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.NotFound;
            }

            return result;
        }


        public async Task<ShopActionResult<AuthenticateModel>> GenerateTokenWithRefreshTokenAsync(string token, string currentRefreshToken, string ip, string browser)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userId = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "userGuid").Value);

            var result = new ShopActionResult<AuthenticateModel>();
            var userLogin = await context.UserRefreshTokens.SingleOrDefaultAsync(q => q.UserId == userId
            && q.RefreshToken == currentRefreshToken && q.IsValid);

            if (userLogin == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidRefreshToken;
                return result;
            }

            if (userLogin.CreateDate.AddMinutes(configs.RefreshTokenTimeout) <= DateTime.Now)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidRefreshToken;
                return result;
            }

            var user = await context.Users.Include(q => q.UserRoles).ThenInclude(q => q.Role)
                .SingleOrDefaultAsync(q => q.Id == userLogin.UserId);

            var roleCodes = string.Join(",", user.UserRoles.Select(q => q.Role.Code).ToList());

            var model = new AuthenticateModel();
            model.Token = customEncryption.GenerateNewToken(user.Id, configs.TokenTimeout, user.UserType, roleCodes);
            model.RefreshToken = customEncryption.GenerateNewRefreshToken();
            await InsertUpdateRefreshTokenAsync(user.Id, model.RefreshToken, ip, browser);

            result.Data = model;
            return result;


        }

        public async Task<ShopActionResult<AuthenticateModel>> LoginAsync(LoginDto model, string ip, string browser)
        {
            var result = new ShopActionResult<AuthenticateModel>();
            #region Validation

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.EnterUserNameAndPassword;
                return result;
            }

            #endregion
            try
            {


                var hashPassword = EncryptionUtility.HashSHA256(model.Password);
                var user = context.Users.Include(q => q.UserRoles).ThenInclude(q => q.Role).SingleOrDefault(q => q.UserName == model.UserName && q.Password == hashPassword);

                if (user == null || !user.IsActive)
                {
                    result.IsSuccess = false;
                    //result.Message = MessagesFA.InvalidUserNameOrPassword;
                    return result;
                }

                var roleCodes = "";
                if (user.UserRoles != null)
                    roleCodes = string.Join(",", user.UserRoles.Select(q => q.Role.Code).ToList());

                var loginModel = new AuthenticateModel
                {
                    RefreshToken = customEncryption.GenerateNewRefreshToken(),
                    Token = customEncryption.GenerateNewToken(user.Id, configs.TokenTimeout, user.UserType, roleCodes),
                    RefreshTokenTimeout = configs.RefreshTokenTimeout,
                    TokenTimeout = configs.TokenTimeout,
                   // IsCompany = (user.UserType != null && user.UserType.Value == UserType.Shop) ? false : true,
                    LastLoginDate = DateUtility.FormatShamsiDateTime(DateTime.Now),

                    User = new UserInfoDto
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        RoleName = user.UserRoles != null && user.UserRoles.Count() > 0 ? user.UserRoles.FirstOrDefault().Role.RoleName : String.Empty,
                        Permissions = await GetUserPermissions(user),
                        UserName = user.UserName,
                    },
                };



                await InsertUpdateRefreshTokenAsync(user.Id, loginModel.RefreshToken, ip, browser);

                result.IsSuccess = true;
                result.Data = loginModel;
              //  result.Message = MessagesFA.UserAuthenticateSuccessful;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
                result.IsSuccess = false;
                //result.Message = MessagesFA.CommunicationError;
                return result;
            }
        }



        public async Task<ShopActionResult<UserAuthenticateModel>> CustomerLoginAsync(LoginDto model, string ip, string browser)
        {
            var result = new ShopActionResult<UserAuthenticateModel>();
            #region Validation

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.EnterUserNameAndPassword;
                return result;
            }

            #endregion
            try
            {


                var hashPassword = EncryptionUtility.HashSHA256(model.Password);
                var user = context.Users.SingleOrDefault(q => q.UserName == model.UserName && q.Password == hashPassword);



                var now = DateTime.Now;


                var userLoginData = await context.Users.FirstOrDefaultAsync(a => a.UserName == model.UserName);


                if (userLoginData != null && user == null)
                {
                    if (userLoginData.IsActive == false)
                    {
                        result.IsSuccess = false;
                 //       result.Message = MessagesFA.UserIsDeActive;
                        return result;
                    }
                    if (!String.IsNullOrEmpty(userLoginData.LastTryLoginDate.ToString()) && userLoginData.LoginfailedCount >= 3)
                    {
                        var date = now - userLoginData.LastTryLoginDate.Value;
                        if (date.Minutes < 15)
                        {
                            result.IsSuccess = false;
                   //         result.Message = MessagesFA.YouCannotLogin;
                            return result;
                        }
                    }
                }


                if (user == null)
                {

                    if (userLoginData != null)
                    {
                        userLoginData.LoginfailedCount += 1;
                        userLoginData.LastTryLoginDate = DateTime.Now;
                        await context.SaveChangesAsync();

                    }
                    result.IsSuccess = false;
                  //  result.Message = MessagesFA.InvalidUserNameOrPassword;
                    return result;
                }

                if (user.IsActive == false)
                {
                    result.IsSuccess = false;
                  //  result.Message = MessagesFA.UserIsDeActive;
                    return result;
                }

                user.LastLoginDate = DateTime.Now;
                user.LoginfailedCount = 0;


                var loginModel = new UserAuthenticateModel
                {
                    RefreshToken = customEncryption.GenerateNewRefreshToken(),
                    Token = customEncryption.GenerateNewToken(user.Id, configs.TokenTimeout, user.UserType, ""),
                    RefreshTokenTimeout = configs.RefreshTokenTimeout,
                    TokenTimeout = configs.TokenTimeout,
                    LastLoginDate = DateUtility.FormatShamsiDateTime(DateTime.Now),
                    User = new InfoDto
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        UserName = user.UserName,
                    },
                };



                await InsertUpdateRefreshTokenAsync(user.Id, loginModel.RefreshToken, ip, browser);

                await context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Data = loginModel;
               // result.Message = MessagesFA.UserAuthenticateSuccessful;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
                result.IsSuccess = false;
               // result.Message = MessagesFA.CommunicationError;
                return result;
            }
        }





        private async Task<List<UserPermissionDto>> GetUserPermissions(User user)
        {
            var roles = user.UserRoles.Select(q => q.RoleId).ToList();
            var permissions = await context.RolePermissions
                .Include(q => q.Permission)
                .ThenInclude(q => q.PermissionGroup)
                .Where(q => roles.Contains(q.RoleId) && q.Permission.IsActive == true && q.Permission.ShowInSidebar == true)
                .OrderBy(q => q.Permission.SortOrder)
                .Select(q => q.Permission)
                .Distinct()
                .ToListAsync();

            var userPermissions = new List<UserPermissionDto>();

            var topLevelPermissions = permissions.Where(q => q.IsTopLevel.Value).ToList();
            var permissionsWithGroupBy = new List<UserPermissionDto>();

            foreach (var item in topLevelPermissions)
            {
                permissionsWithGroupBy.Add(new UserPermissionDto
                {
                    Icon = item.Icon,
                    Id = item.Id,
                    Path = item.Url,
                    Title = item.Title,
                    EnTitle = item.EnTitle,
                    SortOrder = item.SortOrder,
                });
            }

            permissionsWithGroupBy.AddRange(permissions.Where(q => !q.IsTopLevel.Value)
                .GroupBy(q => new { q.PermissionGroupId, q.PermissionGroup.Title, q.PermissionGroup.Icon, q.PermissionGroup.EnTitle, q.PermissionGroup.SortOrder })
                .Select(q => new UserPermissionDto
                {
                    Id = q.Key.PermissionGroupId.Value,
                    Title = q.Key.Title,
                    Icon = q.Key.Icon,
                    EnTitle = q.Key.EnTitle,
                    SortOrder = q.Key.SortOrder,
                    Children = q.Select(c => new UserPermissionDto
                    {
                        Icon = c.Icon,
                        Id = c.Id,
                        Path = c.Url,
                        Title = c.Title,
                        EnTitle = c.EnTitle,
                        SortOrder = c.SortOrder,

                    }).OrderBy(q => q.SortOrder).ToList()
                }));

            permissionsWithGroupBy = permissionsWithGroupBy.OrderBy(q => q.SortOrder).ToList();
            return permissionsWithGroupBy;
        }

        public async Task<(ShopActionResult<bool>, Guid?)> RegisterAsync(RegisterDto model, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            #region Validation
            if (string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.ConfirmPassword))
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.PleaseCompleteForm;
                return (result, null);
            }

            if (model.Password != model.ConfirmPassword)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.InvalidConfirmPassword;
                return (result, null);
            }

            if (await context.Users.AnyAsync(q => q.Email == model.Email))
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.EmailUlreadyExists;
                return (result, null);
            }

            if (await context.Users.AnyAsync(q => q.UserName == model.Email))
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.EmailUlreadyExists;
                return (result, null);
            }

            #endregion

            var confirmationCode = Guid.NewGuid();
            var user = new User
            {
                CreateDate = DateTime.Now,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //IsActive = true,
                IsActive = false,
                Password = EncryptionUtility.HashSHA256(model.Password),
                UserName = model.Email,
                ConfirmationCode = confirmationCode,
                IsCompany = true,
                Id = Guid.NewGuid(),
                UserType = UserType.Customer,
            };

            //using (var dbContextTransaction = context.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        await context.Users.AddAsync(user);
            //        await context.SaveChangesAsync();

            //        //send email
            //        var actionLink = string.Format(MessagesFA.ConfrimationEmailLink, configs.WebSiteAddress, confirmationCode);
            //        var emailSernderInfo = new EmailSenderInfoDto
            //        {
            //            SenderEmail = configs.ConfirmationEmail,
            //            SenderName = String.Empty,
            //            SenderRole = configs.SystemEmailSenderRole,
            //            Website = configs.WebSiteAddress,
            //        };
            //        var userMailMessage = new UserMessageDto
            //        {
            //            ActionLink = actionLink,
            //            SendEmail = true,
            //            Subject = EnumHelpers.GetNameAttribute(UserMessageType.RegisterConfirmation),
            //            UserMessageType = UserMessageType.RegisterConfirmation,
            //            SendWithSystem = true,
            //            ReceiverUserId = user.Id,
            //            To = user.Email,
            //            Body = await emailTemplateService.GenerateEmailTemplate(EmailTemplateType.RegisterConfirmationEmail, emailSernderInfo,
            //        EnumHelpers.GetNameAttribute(UserMessageType.RegisterConfirmation),
            //        user.FirstName + " " + user.LastName,
            //        actionLink),
            //        };

            //        dbContextTransaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        result.IsSuccess = false;
            //        result.Message = MessagesFA.ErrorForAddData;
            //        return (result, null);
            //    }

            //}

            result.IsSuccess = true;
            //result.Message = MessagesFA.SendConfirmationEmail;
            return (result, confirmationCode);
        }

        public async Task<ShopActionResult<bool>> ConfirmationAsync(Guid code, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            #region Validation
            var user = await context.Users.FirstOrDefaultAsync(q => q.ConfirmationCode == code);
            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidConfirmationCode;
                return result;
            }

            if (user.CreateDate.AddDays(1) < DateTime.Now)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.ExpireConfirmationCode;
                return result;
            }
            #endregion

            user.IsActive = true;
            user.ConfirmationDate = DateTime.Now;
            user.ConfirmationCode = Guid.Empty;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UserAccountConfimationSuccess;
            return result;
        }

        public async Task<ShopActionResult<Guid>> ResetPasswordConfirmation(Guid code, string ip, string browser)
        {
            var result = new ShopActionResult<Guid>();

            #region Validation
            var resetPassword = await context.UserResetPasswords.FirstOrDefaultAsync(q => q.ResetToken == code);
            if (resetPassword == null || resetPassword.IsUsed)
            {
                result.IsSuccess = false;
              //  result.Message = MessagesFA.InvalidConfirmationCode;
                return result;
            }

            if (resetPassword.CreateDate.AddDays(1) < DateTime.Now)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.ExpireConfirmationCode;
                return result;
            }

            #endregion
            result.Data = resetPassword.UserId;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<bool>> SendResetPasswordLink(SendResetPasswordLinkDto model, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(q => q.UserName.ToLower() == model.UserName.ToLower());
            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidUserName;
                return result;
            }

            if (user.IsActive == false)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.UserAccountDisabled;
                return result;
            }

            var resetToken = Guid.NewGuid();
            var resetPassword = new UserResetPassword
            {
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsUsed = false,
                ResetToken = resetToken,
                UserId = user.Id,
            };

            #region Send Reset Email for User

          //  var actionLink = string.Format(MessagesFA.ResetPasswordFormat, configs.WebSiteAddress, resetToken);
            var emailSernderInfo = new EmailSenderInfoDto
            {
                SenderEmail = configs.ConfirmationEmail,
                SenderName = String.Empty,
                SenderRole = configs.SystemEmailSenderRole,
                Website = configs.WebSiteAddress,
            };
            var userMailMessage = new UserMessageDto
            {
            //    ActionLink = actionLink,
                SendEmail = true,
                Subject = EnumHelpers.GetNameAttribute(UserMessageType.ForgotPasswordConfirmation),
                UserMessageType = UserMessageType.ForgotPasswordConfirmation,
                SendWithSystem = true,
                To = user.Email,
                ReceiverUserId = user.Id,
                Body = await emailTemplateService.GenerateEmailTemplate(EmailTemplateType.ResetPasswordEmail, emailSernderInfo,
            EnumHelpers.GetNameAttribute(UserMessageType.ForgotPasswordConfirmation),
            user.FirstName + " " + user.LastName
            //actionLink
            ),
            };

            #endregion
            await context.UserResetPasswords.AddAsync(resetPassword);
            await context.SaveChangesAsync();


            result.IsSuccess = true;

            return result;
        }

        public async Task<ShopActionResult<bool>> ResetPassword(ResetPasswordDto model, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(q => q.UserName == model.UserName);

            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidUserName;
                return result;
            }

            if (model.Password != model.ConfirmPassword)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidConfirmPassword;
                return result;
            }

            if (user.IsActive == false)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.UserAccountDisabled;
                return result;
            }

            var resetToken = await context.UserResetPasswords.FirstOrDefaultAsync(q => q.ResetToken == model.ResetCode.Value);
            if (resetToken == null || resetToken.IsUsed || resetToken.CreateDate <= DateTime.Now.AddDays(-5))
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidResetToken;
                return result;
            }


            //update reset token is used
            resetToken.IsUsed = true;
            resetToken.UsedDate = DateTime.Now;
            user.Password = EncryptionUtility.HashSHA256(model.Password);

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<bool>> ChangePasswordAsync(ResetPasswordDto model, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(q => q.UserName == model.UserName);

            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidUserName;
                return result;
            }

            if (model.Password != model.ConfirmPassword)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidConfirmPassword;
                return result;
            }

            if (user.IsActive == false)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.UserAccountDisabled;
                return result;
            }

            var resetToken = await context.UserResetPasswords.FirstOrDefaultAsync(q => q.ResetToken == model.ResetCode.Value);
            if (resetToken == null || resetToken.IsUsed || resetToken.CreateDate <= DateTime.Now.AddDays(-5))
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidResetToken;
                return result;
            }


            //update reset token is used
            resetToken.IsUsed = true;
            user.Password = EncryptionUtility.HashSHA256(model.Password);

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            return result;
        }


        public async Task<ShopActionResult<bool>> ChangePasswordForUserAsync(ResetPasswordDto model, string ip, string browser)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.FirstOrDefaultAsync(q => q.Id == model.UserId && q.UserType == UserType.Customer);

            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidUserName;
                return result;
            }

            if (model.Password != model.ConfirmPassword)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidConfirmPassword;
                return result;
            }

            if (user.IsActive == false)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.UserAccountDisabled;
                return result;
            }

            var resetToken = await context.UserResetPasswords.FirstOrDefaultAsync(q => q.UserId == model.UserId && q.ResetToken == model.ResetCode.Value);
            if (resetToken == null || resetToken.IsUsed || resetToken.CreateDate <= DateTime.Now.AddDays(-5))
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidResetToken;
                return result;
            }


            //update reset token is used
            resetToken.IsUsed = true;
            user.Password = EncryptionUtility.HashSHA256(model.Password);

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            return result;
        }

    }
}
