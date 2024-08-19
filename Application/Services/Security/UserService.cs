using Application.Interfaces.Security;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.EIED;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Security
{
    public class UserService : IUserService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<User> userQueryService;
        public UserService(BIContext context, IGenericQueryService<User> userQueryService)
        {
            this.context = context;
            this.userQueryService = userQueryService;
        }


        /// <summary>
        ///  برگرداندن لیست کاربران
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<UserDto>>> GetUsers(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserDto>>();

            var query = context.Users
                .Include(q => q.UserRoles).ThenInclude(q => q.Role)
                .Include(q => q.UserProjects).ThenInclude(q => q.Project).AsQueryable();

            var filterModel = new UserFilterModel();

            foreach (var item in model.Filtered)
            {
                item.column = item.column.ToLower();
                if (item.column.ToLower() == "username")
                {
                    filterModel.UserName = item.value;
                }
                if (item.column == "firstname")
                {
                    filterModel.FirstName = item.value;
                }
                if (item.column == "lastname")
                {
                    filterModel.LastName = item.value;
                }
                //if (item.column == "usertype")
                //{
                //    filterModel.UserType = (UserType)Enum.Parse(typeof(RegisterBy), item.value);
                //}

            }

            query = query.Where(q =>
            (string.IsNullOrEmpty(filterModel.UserName) || q.UserName.Contains(filterModel.UserName)) &&
            (string.IsNullOrEmpty(filterModel.FirstName) || q.FirstName.Contains(filterModel.FirstName)) &&
            (string.IsNullOrEmpty(filterModel.LastName) || q.LastName.Contains(filterModel.LastName)) 
            //(filterModel.UserType == null || q.UserType == filterModel.UserType.Value)
            // (q.UserType == UserType.TaminPlus)
            );

            result.Total = await query.CountAsync();
            var skipCount = (model.Page - 1) * model.Size;

            var tempResult = await query.OrderByDescending(q => q.CreateDate).Skip(skipCount).Take(model.Size).ToListAsync();

            result.Data = tempResult.Select(item => new UserDto
            {
                Id = item.Id,
                UserName = item.UserName,
                IsActive = item.IsActive,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Roles = item.UserRoles.Count > 0 ? item.UserRoles.Select(q => q.RoleId).ToArray() : null,
                RolesName = item.UserRoles.Count > 0 ? item.UserRoles.Select(u => u.Role.RoleName).ToArray() : null,
                Projects = item.UserProjects.Count > 0 ? item.UserProjects.Select(q => q.ProjectId).ToArray() : null,
                ProjectNames = item.UserProjects.Count > 0 ? item.UserProjects.Select(u => u.Project.Title).ToArray() : null,
                OrganizationUnits = item.UserOrganizationUnits.Count > 0 ? item.UserOrganizationUnits.Select(q => q.OrganizationUnitId).ToArray() : null,
                OrganizationUnitNames = item.UserOrganizationUnits.Count > 0 ? item.UserOrganizationUnits.Select(u => u.OrganizationUnit.RoleName).ToArray() : null,
                CreateDate = item.CreateDate,
                Email = item.UserName,
                PhoneNumber = item.PhoneNumber,
                Sex = item.Sex,
                Code = item.Code,
                UserType = item.UserType,
                UserTypeTitle = item.UserType != null ? EnumHelpers.GetNameAttribute(item.UserType.Value) : String.Empty,
                ShamsiCreateDate = DateUtility.ToShamsi(item.CreateDate),
            }).ToList();

            result.IsSuccess = true;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }


        /// <summary>
        /// ثبت کاربر
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<UserInputModel>> AddUser(UserInputModel model, Guid userId)
        {
            var result = new ShopActionResult<UserInputModel>();
            var hashPassword = EncryptionUtility.HashSHA256(model.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                CreateDate = DateTime.Now,
                IsActive = model.IsActive,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = hashPassword,
                Sex = model.Sex,
                //PhoneNumber = model.PhoneNumber,
                Code = model.Code,
                //UserType = UserType.TaminPlus,
                Email = model.UserName,
            };

            if (model.Roles != null)
                foreach (var item in model.Roles)
                {
                    var role = new UserRole
                    {
                        RoleId = item,
                        UserId = user.Id
                    };
                    await context.UserRoles.AddAsync(role);
                }

            if (model.Projects != null)
                foreach (var item in model.Projects)
                {
                    var userProject = new UserProject
                    {
                        ProjectId = item,
                        UserId = user.Id
                    };
                    await context.UserProjects.AddAsync(userProject);
                }

            if (model.Cartables != null)
                foreach (var item in model.Cartables)
                {
                    var userCartable = new UserCartable
                    {
                        CartableId = item,
                        UserId = user.Id,
                        CreateDate = DateTime.Now,
                        RegisterUserId = userId
                    };
                    await context.UserCartables.AddAsync(userCartable);
                }

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
           // result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        ///    ویرایش کاربر
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<UserInputModel>> UpdateUser(UserInputModel model, Guid userId)
        {
            var result = new ShopActionResult<UserInputModel>();
            var userData = context.Users
                .Include(q => q.UserRoles)
                .Include(q => q.UserProjects)
                .Include(q => q.UserDisciplines)
                .Include(q => q.UserOrganizationUnits)
                .SingleOrDefault(x => x.Id == model.Id);

            userData.UserName = model.UserName;
            userData.IsActive = model.IsActive;
            userData.FirstName = model.FirstName;
            userData.LastName = model.LastName;
            userData.Sex = model.Sex;
            //userData.PhoneNumber = model.PhoneNumber;
            userData.Email = model.UserName;
            userData.Code = model.Code;
            //userData.UserType = UserType.TaminPlus;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var hashPassword = EncryptionUtility.HashSHA256(model.Password);
                userData.Password = hashPassword;
            }
            foreach (var item2 in userData.UserRoles.ToList())
            {
                context.UserRoles.Remove(item2);
            }
            if (model.Roles != null)
                foreach (var item in model.Roles)
                {
                    var role = new UserRole
                    {
                        RoleId = item,
                        UserId = userData.Id
                    };
                    await context.UserRoles.AddAsync(role);
                }

            foreach (var item2 in userData.UserProjects.ToList())
            {
                context.UserProjects.Remove(item2);
            }
            if (model.Projects != null)
                foreach (var item in model.Projects)
                {
                    var userProject = new UserProject
                    {
                        ProjectId = item,
                        UserId = userData.Id
                    };
                    await context.UserProjects.AddAsync(userProject);
                }

            await context.SaveChangesAsync();
            //result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        ///    حذف کاربر
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<Guid>> DeleteUser(Guid userid)
        {
            var result = new ShopActionResult<Guid>();

            var data = await context.Users.FindAsync(userid);
            context.Users.Remove(data);
            await context.SaveChangesAsync();
            //result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        ///   ویرایش رمز عبور
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<Guid>> UpdatePassword(ChangePasswordInputModel model, bool isCurrentUser)
        {
            var result = new ShopActionResult<Guid>();

            if (isCurrentUser)
            {
                var hashOldPassword = EncryptionUtility.HashSHA256(model.OldPassword);

                if (!await context.Users.AnyAsync(q => q.Id == model.UserId && q.Password == hashOldPassword))
                {
                    result.IsSuccess = false;
                   // result.Message = MessagesFA.OldPasswordInvalid;
                    return result;
                }
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.PasswordPolicyInvalid;
                return result;
            }

            var hashPassword = EncryptionUtility.HashSHA256(model.Password);
            var user = await context.Users.FindAsync(model.UserId);
            user.Password = hashPassword;
            await context.SaveChangesAsync();
           // result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        ///   ویرایش امضای ایمیل کاربر   
        /// /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<bool>> ChangeEmailSignature(ChangeEmailSignatureInputModel model)
        {
            var result = new ShopActionResult<bool>();
            var user = await context.Users.FindAsync(model.UserId);
            if (user == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidUserNameOrPassword;
                return result;
            }

            user.EmailSignature = model.EmailSignuature;
            await context.SaveChangesAsync();

           // result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        ///  برگرداندن یک کاربر
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ShopActionResult<UserDto>> GetUserById(Guid userid)
        {
            var result = new ShopActionResult<UserDto>();

            var data = await context.Users
                .Include(q => q.UserRoles).ThenInclude(q => q.Role)
                .Include(q => q.UserProjects).ThenInclude(q => q.Project)
                .Include(q => q.UserOrganizationUnits).ThenInclude(q => q.OrganizationUnit)
                .SingleOrDefaultAsync(q => q.Id == userid);

            var model = new UserDto
            {
                UserName = data.UserName,
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Projects = data.UserProjects.Count > 0 ? data.UserProjects.Select(q => q.ProjectId).Distinct().ToArray() : null,
                ProjectNames = data.UserProjects.Count > 0 ? data.UserProjects.Select(u => u.Project.Title).Distinct().ToArray() : null,
                OrganizationUnits = data.UserOrganizationUnits.Count > 0 ? data.UserOrganizationUnits.Select(q => q.OrganizationUnitId).Distinct().ToArray() : null,
                Roles = data.UserRoles.Count > 0 ? data.UserRoles.Select(q => q.RoleId).ToArray() : null,
                RolesName = data.UserRoles.Count > 0 ? data.UserRoles.Select(u => u.Role.RoleName).ToArray() : null,
                Sex = data.Sex,
                PhoneNumber = data.PhoneNumber,
                Email = data.UserName,
                IsActive = data.IsActive,
                Code = data.Code,
                UserType = data.UserType,
                EmailSignature = data.EmailSignature,
            };
            result.Data = model;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        ///  برگرداندن امضای ایمیل کاربر
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ShopActionResult<string>> GetEmailSignuature(Guid userid)
        {
            var result = new ShopActionResult<string>();

            var data = await context.Users.FindAsync(userid);

            result.Data = data.EmailSignature;

            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        ///  برگرداندن لیست برای کومبو
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboItemDto>> GetUsersForCombo()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();

            var data = await context.Users.Where(i => i.IsActive == true).Select(x => new ComboItemDto
            {
                Value = x.Id,
                Text = x.FirstName + " " + x.LastName,

            }).ToListAsync();
            return data;
        }

        /// <summary>
        ///  برگرداندن لیست برای کومبو
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDto>> GetUsersInfoForCombo()
        {
            var result = new ShopActionResult<List<UserDto>>();

            var data = await context.Users.Include(q => q.UserRoles).ThenInclude(q => q.Role).Where(i => i.IsActive == true).Select(x => new UserDto
            {
                Value = x.Id,
                Text = x.FirstName + " " + x.LastName,
                UserName = x.UserName,
                RolesName = x.UserRoles.Count > 0 ? x.UserRoles.Select(q => q.Role.RoleName).ToArray() : null,
                PhoneNumber = x.PhoneNumber,
                SexTitle = x.Sex != null ? EnumHelpers.GetNameAttribute<Sex>(x.Sex.Value).ToString() : null,
                LastName = x.LastName,
                FirstName = x.FirstName,
                IsActiveTitle = x.IsActive == true ? "Active" : "DeActive",
                UserTypeTitle = x.UserType != null ? EnumHelpers.GetNameAttribute<UserType>(x.UserType.Value).ToString() : null,
                Email = x.UserName
            }).ToListAsync();
            return data;
        }
    }
}
