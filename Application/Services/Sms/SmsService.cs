using Application.Interfaces.Sms;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NanoidDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConnectionUtility connectionUtility;
        private readonly ICustomEncryption customEncryption;
        private readonly IGenericQueryService<UserSms> queryService;

        public IConfiguration configuration { get; }
        Configs configs;
        BIContext context;
        SMSUtility sms;
        public SmsService(IConnectionUtility connectionUtility,
            IOptions<Configs> options, BIContext context, ICustomEncryption customEncryption,
            IConfiguration configuration, SMSUtility sms
            )
        {
            this.connectionUtility = connectionUtility;
            this.configs = options.Value;
            this.context = context;
            this.customEncryption = customEncryption;
            this.configuration = configuration;
            this.sms = sms;
        }


        private async Task<string> GenrateCode()
        {

            var code = Nanoid.Generate("0123456789", 6).ToString();

            while (await context.UserSmss.AnyAsync(a => a.Code == code))
            {
                code = Nanoid.Generate("0123456789", 6).ToString();
            }

            return code;
        }



        public async Task<ShopActionResult<bool>> SendSmsForgetPasswordForAdmin(Guid userid)
        {
            var result = new ShopActionResult<bool>();

            //Guid id = Guid.Parse(userid);

            var user = await context.Users.SingleOrDefaultAsync(s => s.Id == userid);
            if (user == null)
            {
                //result.Message = Messages.NotFoundUser;
                result.IsSuccess = false;
                result.StatusCode = 401;
            }

            var code = await GenrateCode();


            var data = new UserSms()
            {
                Code = code,
                ActionTime = DateTime.Now,
                Type = SmsType.ForgotPassword,
                Status = true,
                UserId = user.Id,

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();

            var message = $"*تامین پلاس* \n " +
                       $"کد تایید بازنشانی رمز عبور :" + code;
            await sms.Send(user.PhoneNumber, message);

            //result.Message = MessagesFA.SendSmsForForgetPasswordSuccess;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;

        }

        public async Task<ShopActionResult<bool>> SendSmsForAlerts(string userName, string password)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.SingleOrDefaultAsync(s => s.UserName == userName);
            if (user == null)
            {
               // result.Message = MessagesFA.NotFound;
                result.IsSuccess = false;
                result.StatusCode = 401;
            }


            var message = $"*تامین پلاس* \n" +
                $"نام کاربری :" + user.UserName + "\n" +
                  $" رمز عبور :" + password;
            var data = new UserSms()
            {
                Code = "",
                ActionTime = DateTime.Now,
                Type = SmsType.Admin,
                Status = true,
                UserId = user.Id,

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();

          //  result.Message = MessagesFA.SendSmsForForgetPasswordSuccess;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;

        }


        public async Task<ShopActionResult<bool>> SendSmsForRequestAlerts(Guid userId, string alert)
        {
            var result = new ShopActionResult<bool>();

            var user = await context.Users.SingleOrDefaultAsync(s => s.Id == userId);
            if (user == null)
            {
               // result.Message = MessagesFA.NotFound;
                result.IsSuccess = false;
                result.StatusCode = 401;
            }

            var message = $"*تامین پلاس* \n" + alert;

            var data = new UserSms()
            {
                ActionTime = DateTime.Now,
                Type = SmsType.ChangeRequestStatus,
                Status = true,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                Message = message
            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();

            await sms.Send(user.PhoneNumber, message);

            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;

        }




        public async Task<ShopActionResult<bool>> SendSmsForgetPassword(string userName)
        {
            var result = new ShopActionResult<bool>();


            var user = await context.Users.SingleOrDefaultAsync(s => s.UserName == userName);
            if (user == null)
            {
               // result.Message = MessagesFA.NotFound;
                result.IsSuccess = false;
                result.StatusCode = 401;
                return result;

            }
            var date = DateTime.Now;
            var min = date.Minute;
            var hour = date.Hour;
            var day = date.Day;

            //ActionTime.Hour == hour &&
            var usersms = await context.SmsLogs.Where(s => s.UserName == userName && s.ActionTime.Year == date.Year && date.Month == date.Month &&
            s.ActionTime.Day == day && s.SmsType == SmsType.ForgotPassword).ToListAsync();

            if (usersms.Count() > 4)
            {
                result.StatusCode = 505;
                result.Message = "شما تا پایان امروز مجوز تغییر رمز عبور را ندارید";
                result.IsSuccess = false;
                return result;
            }

            var code = await GenrateCode();


            var dataSmsLog = new SmsLog()
            {
                ActionTime = DateTime.Now,
                Status = true,
                UserName = userName,
                SmsType = SmsType.ForgotPassword,
                Id = Guid.NewGuid(),
            };

            await context.SmsLogs.AddAsync(dataSmsLog);

            var message = $"*تامین پلاس* \n " +
                        $"کد تایید بازنشانی رمز عبور :" + code;

            var data = new UserSms()
            {
                Code = code,
                ActionTime = DateTime.Now,
                Type = SmsType.ForgotPassword,
                Status = true,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                Message = message

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();


            await sms.Send(user.PhoneNumber, message);

           // result.Message = MessagesFA.SendSmsForForgetPasswordSuccess;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;

        }

        public async Task<ShopActionResult<bool>> SendSmsForConfirmPhoneNumber(Guid userid, string phoneNumber)
        {
            var result = new ShopActionResult<bool>();
            var user = await context.Users.SingleOrDefaultAsync(s => s.Id == userid);

            var code = await GenrateCode();

            var message = $"*تامین پلاس* \n " +
                       $"کد تایید برای شماره موبایل  :" + code;
            var data = new UserSms()
            {
                Code = code,
                ActionTime = DateTime.Now,
                Type = SmsType.ConfirmPhoneNumber,
                Status = true,
                UserId = user.Id,
                PhoneNumber = phoneNumber,
                Message = message

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();



            await sms.Send(user.PhoneNumber, message);

           // result.Message = MessagesFA.SendCodeConfirmPhone;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;
        }



        public async Task<ShopActionResult<bool>> SendSmsForRegisterUser(string phoneNumber)
        {
            var result = new ShopActionResult<bool>();

            var code = await GenrateCode();
            var message = $"*تامین پلاس* \n " +
                         $"کد تایید برای ثبت نام :" + code;
            var data = new UserSms()
            {
                Code = code,
                ActionTime = DateTime.Now,
                Type = SmsType.RegisterUser,
                Status = true,
                PhoneNumber = phoneNumber,
                Message = message

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();




            await sms.Send(phoneNumber, message);


           // result.Message = MessagesFA.SendCodeConfirmPhone;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;
        }

        public async Task<ShopActionResult<bool>> SendSmsForActiveUser(string phoneNumber)
        {
            var result = new ShopActionResult<bool>();

            var code = await GenrateCode();
            var message = $"*تامین پلاس* \n " +
                         $"کد تایید برای فعال سازی :" + code;
            var data = new UserSms()
            {
                Code = code,
                ActionTime = DateTime.Now,
                Type = SmsType.SetActiveUser,
                Status = true,
                PhoneNumber = phoneNumber,
                Message = message

            };
            await context.UserSmss.AddAsync(data);
            await context.SaveChangesAsync();




            await sms.Send(phoneNumber, message);


            //result.Message = MessagesFA.SendCodeConfirmPhone;
            result.IsSuccess = true;
            result.StatusCode = 200;
            return result;
        }



        public async Task<ShopActionResult<List<SmsDto>>> GetList(GridQueryModel model)
        {
            var result = new ShopActionResult<List<SmsDto>>();

            var query = context.UserSmss.OrderByDescending(o => o.ActionTime).Include(i => i.User).AsQueryable();


            var filterModel = new SmsFilterModel();

            foreach (var item in model.Filtered)
            {
                item.column = item.column.ToLower();
                if (item.column.ToLower() == "phonenumber")
                {
                    filterModel.PhoneNumber = item.value;
                }

                if (item.column == "firstname")
                {
                    filterModel.FirstName = item.value;
                }
                if (item.column == "lastname")
                {
                    filterModel.LastName = item.value;
                }

                if (item.column == "smstype")
                {
                    filterModel.SmsType = (SmsType)Enum.Parse(typeof(SmsType), item.value);
                }
                if (item.column == "status")
                {
                    filterModel.Status = Convert.ToBoolean(item.value);
                }
            }

            query = query.Where(q =>
                    (string.IsNullOrEmpty(filterModel.FirstName) || q.User.FirstName.Contains(filterModel.FirstName)) &&
                    (string.IsNullOrEmpty(filterModel.LastName) || q.User.LastName.Contains(filterModel.LastName)) &&
                    (string.IsNullOrEmpty(filterModel.PhoneNumber) || q.User.PhoneNumber == (filterModel.PhoneNumber)) &&
                    (filterModel.SmsType == null || q.Type == filterModel.SmsType.Value) &&
                    (filterModel.Status == null || q.Status == filterModel.Status.Value)
                    );


            result.Total = await query.CountAsync();
            var skipCount = (model.Page - 1) * model.Size;

            var tempResult = await query.Skip(skipCount).Take(model.Size).ToListAsync();

            result.Data = tempResult.Select(q => new SmsDto
            {
                ActionTime = DateUtility.CovertToShamsi(q.ActionTime),
                Code = q.Code,
                FullName = q.User?.FirstName + " " + q.User?.LastName,
                PhoneNumber = q.PhoneNumber,
                Status = q.Status == true ? "فعال" : "غیر فعال",
                Type = q.Type.GetNameAttribute(),
                Message = q.Message,
                Id = q.Id

            }).ToList();
            result.IsSuccess = true;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }


        public async Task<ShopActionResult<bool>> CheckCodeSmSForPhoneNumber(string code, Guid userId)
        {
            var result = new ShopActionResult<bool>();
            var codeSmsValue = await context.UserSmss.FirstOrDefaultAsync(a => a.UserId == userId && a.Status == true && a.Type == SmsType.ConfirmPhoneNumber && a.Code == code);
            if (codeSmsValue != null)
            {
                var date = DateTime.Now;
                var min = date.Minute;
                var basemin = min - codeSmsValue.ActionTime.Minute;

                if (Math.Abs(basemin) < 7 && date.Month == codeSmsValue.ActionTime.Month && date.Hour == codeSmsValue.ActionTime.Hour && date.Day == codeSmsValue.ActionTime.Day && date.Year == codeSmsValue.ActionTime.Year)
                {
                    var userData = await context.Users.FirstOrDefaultAsync(f => f.Id == userId);
                    userData.IsConfirmPhoneNumber = true;
                    await context.SaveChangesAsync();
                    result.IsSuccess = true;
                    //result.Message = MessagesFA.YourPhoneNumberIsAccept;
                    return result;
                }

            }
            //if (codeSmsValue !=null)
            //{
            //    codeSmsValue.Status = false;
            //    await context.SaveChangesAsync();

            //}

            result.IsSuccess = false;
           // result.Message = MessagesFA.CodeNotExists;
            return result;



        }


        public async Task<ShopActionResult<bool>> CheckCodeSmSForRegisterUser(string code, string phoneNumber)
        {
            var result = new ShopActionResult<bool>();
            var codeSmsValue = await context.UserSmss.FirstOrDefaultAsync(a => a.Status == true && a.Type == SmsType.RegisterUser && a.Code == code && a.PhoneNumber == phoneNumber);
            if (codeSmsValue != null)
            {
                var date = DateTime.Now;
                var min = date.Minute;
                var basemin = min - codeSmsValue.ActionTime.Minute;

                if (Math.Abs(basemin) <= 3 && date.Hour == codeSmsValue.ActionTime.Hour && date.Month == codeSmsValue.ActionTime.Month && date.Day == codeSmsValue.ActionTime.Day && date.Year == codeSmsValue.ActionTime.Year)
                {
                    var user = await context.Users.FirstOrDefaultAsync(a => a.UserName == phoneNumber);
                    if (user != null)
                    {
                        user.IsConfirmPhoneNumber = true;
                        user.IsActive = true;
                        result.IsSuccess = true;
                        await context.SaveChangesAsync();
                       // result.Message = MessagesFA.RegisterSuccessful;
                        return result;

                    }
                    else
                    {

                        result.IsSuccess = false;
                       // result.Message = MessagesFA.CodeNotExists;
                        return result;
                    }
                }

            }

            result.IsSuccess = false;
           // result.Message = MessagesFA.CodeNotExists;
            return result;



        }
        public async Task<ShopActionResult<bool>> CheckCodeSmSForActiveUser(string code, string phoneNumber)
        {
            var result = new ShopActionResult<bool>();
            var codeSmsValue = await context.UserSmss.FirstOrDefaultAsync(a => a.Status == true && a.Type == SmsType.SetActiveUser && a.Code == code && a.PhoneNumber == phoneNumber);
            if (codeSmsValue != null)
            {
                var date = DateTime.Now;
                var min = date.Minute;
                var basemin = min - codeSmsValue.ActionTime.Minute;

                if (Math.Abs(basemin) <= 3 && date.Hour == codeSmsValue.ActionTime.Hour && date.Month == codeSmsValue.ActionTime.Month && date.Day == codeSmsValue.ActionTime.Day && date.Year == codeSmsValue.ActionTime.Year)
                {
                    var user = await context.Users.FirstOrDefaultAsync(a => a.UserName == phoneNumber);
                    if (user != null)
                    {
                        user.IsConfirmPhoneNumber = true;
                        user.IsActive = true;
                        result.IsSuccess = true;
                        await context.SaveChangesAsync();
                        //result.Message = MessagesFA.RegisterSuccessful;
                        return result;

                    }
                    else
                    {

                        result.IsSuccess = false;
                        //result.Message = MessagesFA.CodeNotExists;
                        return result;
                    }
                }

            }

            result.IsSuccess = false;
            //result.Message = MessagesFA.CodeNotExists;
            return result;



        }

        public async Task<ShopActionResult<Guid>> CheckCodeSmSForForgetPasswordUser(string code, string userName)
        {
            var result = new ShopActionResult<Guid>();
            var codeSmsValue = await context.UserSmss.Include(i => i.User)
                .FirstOrDefaultAsync(a => a.Status == true && a.Type == SmsType.ForgotPassword && a.Code == code && a.User.UserName == userName);
            if (codeSmsValue != null)
            {
                var date = DateTime.Now;
                var min = date.Minute;
                var basemin = min - codeSmsValue.ActionTime.Minute;

                if (Math.Abs(basemin) < 2 && date.Hour == codeSmsValue.ActionTime.Hour && date.Month == codeSmsValue.ActionTime.Month && date.Day == codeSmsValue.ActionTime.Day && date.Year == codeSmsValue.ActionTime.Year)
                {
                    var user = await context.Users.FirstOrDefaultAsync(a => a.PhoneNumber == codeSmsValue.User.PhoneNumber);
                    if (user != null)
                    {
                        await context.UserResetPasswords.AddAsync(
                            new UserResetPassword
                            {
                                Id = Guid.NewGuid(),
                                IsUsed = false,
                                CreateDate = date,
                                ResetToken = codeSmsValue.Id,
                                UserId = user.Id,

                            }
                            );

                        //codeSmsValue.Status = false;
                        await context.SaveChangesAsync();
                        result.Data = codeSmsValue.Id;
                        result.IsSuccess = true;
                      //  result.Message = MessagesFA.SpecifyTheNewPassword;
                        return result;

                    }
                    else
                    {

                        result.IsSuccess = false;
                      //  result.Message = MessagesFA.CodeNotExists;
                        return result;
                    }
                }

            }

            result.IsSuccess = false;
            //result.Message = MessagesFA.CodeNotExists;
            return result;
        }
    }
}
