using Application.Interfaces.Security;
using Application.Interfaces.Sms;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Authorization;
using Infrastructure.Models.Customer;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Shop;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Security
{
    public class CustomerService : ICustomerService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<User> userQueryService;
        private readonly IShopService shopService;
        private readonly ISmsService _smsService;

        public CustomerService(BIContext context, IGenericQueryService<User> userQueryService, IShopService shopService, ISmsService smsService)
        {
            this.context = context;
            this.userQueryService = userQueryService;
            this.shopService = shopService;
            this._smsService = smsService;

        }


        /// <summary>
        ///  برگرداندن لیست مشتریان
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<CustomerDto>>> GetCustomers(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<CustomerDto>>();

            var query = context.Customers
                .Include(q => q.User).AsQueryable();

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

            }

            query = query.Where(q =>
            (string.IsNullOrEmpty(filterModel.FirstName) || q.User.FirstName.Contains(filterModel.FirstName)) &&
            (string.IsNullOrEmpty(filterModel.LastName) || q.User.LastName.Contains(filterModel.LastName)) &&
            (string.IsNullOrEmpty(filterModel.UserName) || q.User.PhoneNumber.Contains(filterModel.UserName))
            );

            result.Total = await query.CountAsync();
            var skipCount = (model.Page - 1) * model.Size;

            var tempResult = await query.OrderByDescending(q => q.CreateDate).Skip(skipCount).Take(model.Size).ToListAsync();

            result.Data = tempResult.Select(item => new CustomerDto
            {
                Id = item.User.Id,
                UserName = item.User.UserName,
                IsActive = item.IsActive,
                FirstName = item.User.FirstName,
                LastName = item.User.LastName,
                CreateDate = item.CreateDate,
                Email = item.User.Email,
                PhoneNumber = item.User.PhoneNumber,
                Sex = item.User.Sex,
                SexTitle = item.User.Sex != null ? item.User.Sex.Value.GetNameAttribute() : "",
                RegisterDate = DateUtility.FormatShamsiDateTime(item.CreateDate),
            }).ToList();

            result.IsSuccess = true;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }


        /// <summary>
        /// ثبت مشتری
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<CustomerInputModel>> AddCustomer(CustomerInputModel model, bool isAdmin = true)
        {
            var result = new ShopActionResult<CustomerInputModel>();


            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password)
                || String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName))
            {

               // result.Message = MessagesFA.InvalidUserNameOrPassword;
                result.IsSuccess = false;
                return result;

            }

            if (!String.IsNullOrEmpty(model.Email) && RegexUtility.IsValidEmail(model.Email) == false)
            {
               // result.Message = MessagesFA.InvalidEmailFormat;
                result.IsSuccess = false;
                return result;
            }
            if (!String.IsNullOrEmpty(model.PhoneNumber) && RegexUtility.IsValidPhoneNumber(model.PhoneNumber) == false)
            {
               // result.Message = MessagesFA.InvalidPhoneNumberFormat;
                result.IsSuccess = false;
                return result;
            }



            if (model.Password.Length < 8)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.PasswordPolicyInvalid;
                return result;
            }


            //if (!String.IsNullOrEmpty(model.Email) && context.Users.Any(a => a.Email == model.Email))
            //{
            //    result.Message = MessagesFA.EmailIsAlreadyRegistered;
            //    result.IsSuccess = false;
            //    return result;
            //}

            if (!String.IsNullOrEmpty(model.PhoneNumber) && context.Users.Any(a => a.PhoneNumber == model.PhoneNumber))
            {
               // result.Message = MessagesFA.PhoneNumberIsAlreadyRegistered;
                result.IsSuccess = false;
                return result;
            }



            if (context.Users.Any(a => a.UserName == model.UserName))
            {
               // result.Message = MessagesFA.CurrentUserAlreadyRegistered;
                result.IsSuccess = false;
                return result;
            }


            var password = EncryptionUtility.HashSHA256(model.Password);

            var user = new User();


            //if (RegexUtility.IsValidEmail(model.UserName) == true)
            //{
            //    user = new User
            //    {
            //        UserName = model.UserName,
            //        Password = password,
            //        Email = model.UserName,
            //        CreateDate = DateTime.Now,
            //        FirstName = model.FirstName,
            //        LastName = model.LastName,
            //        Id = Guid.NewGuid(),
            //        UserType = UserType.Customer,
            //        IsActive = model.IsActive,
            //        Sex = model.Sex,
            //        FullName= model.FirstName +" "+model.LastName,

            //    };
            //    await context.Users.AddAsync(user);
            //    result.Message = MessagesFA.SaveSuccessful;

            //}

            if (RegexUtility.IsValidPhoneNumber(model.UserName) == true)
            {
                user = new User
                {
                    UserName = model.UserName,
                    Password = password,
                    PhoneNumber = model.UserName,
                    CreateDate = DateTime.Now,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Id = Guid.NewGuid(),
                    UserType = UserType.Customer,
                    IsActive = true,
                    Sex = model.Sex,
                    FullName = model.FirstName + " " + model.LastName,

                };
                await context.Users.AddAsync(user);
                //result.Message = MessagesFA.SaveSuccessful;

            }

            if (user.UserName == null)
            {
                //result.Message = MessagesFA.InvalidUserNameFormat;
                result.IsSuccess = false;
                return result;
            }

            await context.SaveChangesAsync();

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                IsActive = model.IsActive,
                UserId = user.Id,
                Address = model.Address,
                //Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null,
                //CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) !=null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id :null : null,
                GoldIranCityId = model.GoldIranCityId,
                DeliveryAddress = model.DeliveryAddress,
                GoldIranProvinceId = model.GoldIranProvinceId,
                ParishId = model.ParishId,
                RegionId = model.RegionId,
                PostCode = model.PostCode
            };

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();


            result.IsSuccess = true;
            return result;




        }


        /// <summary>
        ///    ویرایش مشتری
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<CustomerInputModel>> UpdateCustomer(CustomerInputModel model, bool isAdmin = true)
        {
            var result = new ShopActionResult<CustomerInputModel>();

            if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName))
            {

                //result.Message = MessagesFA.EnterUserNameAndPassword;
                result.IsSuccess = false;
                return result;
            }

            if (!String.IsNullOrEmpty(model.Email) && RegexUtility.IsValidEmail(model.Email) == false)
            {
                //result.Message = MessagesFA.InvalidEmailFormat;
                result.IsSuccess = false;
                return result;
            }
            if (!String.IsNullOrEmpty(model.PhoneNumber) && RegexUtility.IsValidPhoneNumber(model.PhoneNumber) == false)
            {
                //result.Message = MessagesFA.InvalidPhoneNumberFormat;
                result.IsSuccess = false;
                return result;
            }



            var userData = context.Users
             .SingleOrDefault(x => x.Id == model.Id);

            //if (!String.IsNullOrEmpty(model.Email) && context.Users.Any(a => a.Id != model.Id && a.Email == model.Email))
            //{
            //    result.Message = MessagesFA.EmailIsAlreadyRegistered;
            //    result.IsSuccess = false;
            //    return result;
            //}

            if (!String.IsNullOrEmpty(model.PhoneNumber) && context.Users.Any(a => a.Id != model.Id && a.PhoneNumber == model.PhoneNumber))
            {
                //result.Message = MessagesFA.PhoneNumberIsAlreadyRegistered;
                result.IsSuccess = false;
                return result;
            }




            if (context.Users.Any(a => a.Id != model.Id && a.UserName == model.UserName))
            {
                //result.Message = MessagesFA.CurrentUserAlreadyRegistered;
                result.IsSuccess = false;
                return result;
            }

            //if (RegexUtility.IsValidEmail(model.UserName) == true)
            //{

            //    userData.UserName = model.UserName;
            //    userData.Email = model.Email;
            //    userData.PhoneNumber = model.PhoneNumber;
            //    userData.FirstName = model.FirstName;
            //    userData.LastName = model.LastName;
            //    userData.FullName = model.FirstName + " " + model.LastName;
            //    userData.Sex = model.Sex;
            //    userData.UserType = UserType.Customer;
            //    userData.IsActive = model.IsActive;
            //    result.Message = MessagesFA.UpdateSuccessful;

            //}

            if (RegexUtility.IsValidPhoneNumber(model.UserName) == true)
            {
                //if (isAdmin==true)
                //{
                //    userData.UserName = model.UserName;
                //}
                userData.Email = model.Email;
                //userData.PhoneNumber = model.PhoneNumber;
                userData.FirstName = model.FirstName;
                userData.LastName = model.LastName;
                userData.FullName = model.FirstName + " " + model.LastName;
                userData.Sex = model.Sex;
                userData.UserType = UserType.Customer;
                userData.IsActive = model.IsActive;

                //result.Message = MessagesFA.UpdateSuccessful;
            }

            else
            {
               // result.Message = MessagesFA.InvalidUserNameFormat;
                return result;
            }



            await context.SaveChangesAsync();

            var customer = await context.Customers.FirstOrDefaultAsync(f => f.UserId == model.Id);
            if (customer != null)
            {
                customer.IsActive = model.IsActive;
                customer.Address = model.Address;
                //customer.Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null;
                //customer.CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) != null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id : null : null;
                customer.GoldIranCityId = model.GoldIranCityId;
                customer.DeliveryAddress = model.DeliveryAddress;
                customer.GoldIranProvinceId = model.GoldIranProvinceId;
                customer.ParishId = model.ParishId;
                customer.RegionId = model.RegionId;
                customer.PostCode = model.PostCode;

            }

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            return result;




        }




        /// <summary>
        ///   ویرایش مشتری توسط خود کاربر 
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<CustomerInputModel>> UpdateCustomerByUser(CustomerInputModel model)
        {
            var result = new ShopActionResult<CustomerInputModel>();

            if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName) ||
                 String.IsNullOrEmpty(model.LastName) || model.GoldIranCityId == 0 || model.GoldIranProvinceId == 0)
            {

                //result.Message = MessagesFA.ErrorForEditData;
                result.IsSuccess = false;
                return result;
            }

            if (!String.IsNullOrEmpty(model.Email) && RegexUtility.IsValidEmail(model.Email) == false)
            {
                //result.Message = MessagesFA.InvalidEmailFormat;
                result.IsSuccess = false;
                return result;
            }




            var userData = context.Users
             .SingleOrDefault(x => x.Id == model.Id);

            userData.Email = model.Email;
            //userData.PhoneNumber = model.PhoneNumber;
            userData.FirstName = model.FirstName;
            userData.LastName = model.LastName;
            userData.FullName = model.FirstName + " " + model.LastName;




            await context.SaveChangesAsync();

            var customer = await context.Customers.FirstOrDefaultAsync(f => f.UserId == model.Id);
            if (customer != null)
            {
                if (!string.IsNullOrEmpty(model.Address))
                {
                    customer.Address = model.Address;

                }
                //customer.Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null;
                //customer.CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) !=null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id : null : null;
                customer.GoldIranCityId = model.GoldIranCityId;

                customer.DeliveryAddress = model.DeliveryAddress;
                customer.GoldIranProvinceId = model.GoldIranProvinceId;
                customer.ParishId = model.ParishId;
                customer.RegionId = model.RegionId;
                customer.PostCode = model.PostCode;
            }
            else
            {
                var customerItem = new Customer
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    UserId = userData.Id,
                    Address = model.Address,
                    //Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null,
                    //CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) != null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id : null : null,
                    GoldIranCityId = model.GoldIranCityId,
                    DeliveryAddress = model.DeliveryAddress,
                    GoldIranProvinceId = model.GoldIranProvinceId,
                    ParishId = model.ParishId,
                    RegionId = model.RegionId,
                    PostCode = model.PostCode

                };

                await context.Customers.AddAsync(customerItem);
            }
            await context.SaveChangesAsync();
            //result.Message = MessagesFA.SaveSuccessful;

            result.IsSuccess = true;
            return result;




        }




        /// <summary>
        ///   ویرایش مشتری توسط خود کاربر 
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<CustomerInputModel>> UpdateCustomerInfoByUser(CustomerInputModel model)
        {
            var result = new ShopActionResult<CustomerInputModel>();

            if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName) ||
                 String.IsNullOrEmpty(model.LastName) || model.CityId == 0)
            {

               // result.Message = MessagesFA.ErrorForEditData;
                result.IsSuccess = false;
                return result;
            }

            if (!String.IsNullOrEmpty(model.Email) && RegexUtility.IsValidEmail(model.Email) == false)
            {
               // result.Message = MessagesFA.InvalidEmailFormat;
                result.IsSuccess = false;
                return result;
            }




            var userData = context.Users
             .SingleOrDefault(x => x.Id == model.Id);


            var customer = await context.Customers.FirstOrDefaultAsync(f => f.UserId == model.Id);

            if (customer != null)
            {
                customer.Address = model.Address;
                //customer.Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null;
                //customer.CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) != null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id : null : null;
                customer.GoldIranCityId = model.GoldIranCityId;
                customer.DeliveryAddress = model.DeliveryAddress;
                customer.GoldIranCityId = model.GoldIranCityId;
                customer.DeliveryAddress = model.DeliveryAddress;
                customer.GoldIranProvinceId = model.GoldIranProvinceId;
                customer.ParishId = model.ParishId;
                customer.RegionId = model.RegionId;
                customer.PostCode = model.PostCode;
                customer.ProvinceTitle = model.ProvinceTitle;
                customer.CityTitle = model.CityTitle;
                customer.RegionTitle = model.RegionTitle;
                customer.ParishTitle = model.ParishTitle;


            }
            else
            {
                var customerItem = new Customer
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    UserId = userData.Id,
                    Address = model.Address,
                    //Province = !String.IsNullOrEmpty(model.ProvinceTitle) ? EnumHelpers.ParseEnum<Province>(model.ProvinceTitle) : null,
                    //CityId = !String.IsNullOrEmpty(model.CityTitle) ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)) !=null ? context.Cities.FirstOrDefault(f => f.Title.Contains(model.CityTitle)).Id : null : null,
                    GoldIranCityId = model.GoldIranCityId,
                    DeliveryAddress = model.DeliveryAddress,
                    GoldIranProvinceId = model.GoldIranProvinceId,
                    ParishId = model.ParishId,
                    RegionId = model.RegionId,
                    PostCode = model.PostCode,
                    ProvinceTitle = model.ProvinceTitle,
                    CityTitle = model.CityTitle,
                    ParishTitle = model.ParishTitle,
                    RegionTitle = model.RegionTitle,
                };

                await context.Customers.AddAsync(customerItem);
            }
            await context.SaveChangesAsync();
           // result.Message = MessagesFA.SaveSuccessful;

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
                    //result.Message = MessagesFA.OldPasswordInvalid;
                    return result;
                }
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.PasswordPolicyInvalid;
                return result;
            }

            var hashPassword = EncryptionUtility.HashSHA256(model.Password);
            var user = await context.Users.FindAsync(model.UserId);
            user.Password = hashPassword;
            await context.SaveChangesAsync();
            //result.Message = MessagesFA.SuccessTheOperation;
            result.IsSuccess = true;
            return result;
        }




        /// <summary>
        ///  برگرداندن یک مشتری
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ShopActionResult<CustomerDto>> GetCustomerById(Guid userId)
        {
            var result = new ShopActionResult<CustomerDto>();

            var data = await context.Users
                .SingleOrDefaultAsync(q => q.Id == userId);


            if (data == null)
            {
                result.IsSuccess = false;
                return result;
            }

            var customer = await context.Customers.FirstOrDefaultAsync(f => f.UserId == userId);
            if (customer != null)
            {
                var model = new CustomerDto
                {
                    UserName = data.UserName,
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    PhoneNumber = data.PhoneNumber,
                    Email = data.Email,
                    Address = customer?.Address,
                    GoldIranCityId = customer?.GoldIranCityId,
                    GoldIranProvinceId = customer?.GoldIranProvinceId,
                    RegionId = customer?.RegionId,
                    ParishId = customer?.ParishId,
                    DeliveryAddress = customer?.DeliveryAddress,
                    IsActive = customer.IsActive,
                    PostCode = customer.PostCode,
                };
                result.Data = model;
            }

            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<AuthenticateModel>> RegisterCustomer(RegisterCustomerInputModel model)
        {
            var result = new ShopActionResult<AuthenticateModel>();




            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password))
            {

               // result.Message = MessagesFA.InvalidUserNameOrPassword;
                result.IsSuccess = false;
                return result;

            }

            if (model.Password.Length < 8)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.PasswordPolicyInvalid;
                return result;
            }

            if (context.Users.Any(a => a.UserName == model.UserName))
            {
               // result.Message = MessagesFA.CurrentUserAlreadyRegistered;
                result.IsSuccess = false;
                return result;
            }


            var password = EncryptionUtility.HashSHA256(model.Password);

            var user = new User();


            //if (RegexUtility.IsValidEmail(model.UserName) == true)
            //{

            //    if (context.Users.Any(a => a.Email == model.UserName))
            //    {
            //        result.Message = MessagesFA.CurrentUserAlreadyRegistered;
            //        result.IsSuccess = false;
            //        return result;
            //    }


            //    user = new User
            //    {
            //        UserName = model.UserName,
            //        Password = password,
            //        Email = model.UserName,
            //        CreateDate = DateTime.Now,
            //        //FirstName = "",
            //        //LastName = "",
            //        Id = Guid.NewGuid(),
            //        UserType = UserType.Customer,
            //        IsActive = true,
            //        LastLoginDate = DateTime.Now,


            //    };
            //    await context.Users.AddAsync(user);
            //    result.Message = MessagesFA.SendConfirmationEmail;

            //}

            if (RegexUtility.IsValidPhoneNumber(model.UserName) == true)
            {


                if (context.Users.Any(a => a.PhoneNumber == model.UserName))
                {
                 //   result.Message = MessagesFA.CurrentUserAlreadyRegistered;
                    result.IsSuccess = false;
                    return result;
                }


                user = new User
                {
                    UserName = model.UserName,
                    Password = password,
                    PhoneNumber = model.UserName,
                    CreateDate = DateTime.Now,
                    //FirstName = "-",
                    //LastName = "-",
                    Id = Guid.NewGuid(),
                    UserType = UserType.Customer,
                    IsActive = false,
                    LastLoginDate = DateTime.Now,


                };
                await context.Users.AddAsync(user);
              //  result.Message = MessagesFA.SendConfirmationPhoneNumber;

            }


            if (user.UserName == null)
            {
              //  result.Message = MessagesFA.InvalidUserNameFormat;
                result.IsSuccess = false;
                return result;
            }

            await context.SaveChangesAsync();

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                IsActive = true,
                UserId = user.Id,

            };

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            var smsResult = await _smsService.SendSmsForRegisterUser(model.UserName);

            if (smsResult != null)
            {
                result.IsSuccess = smsResult.IsSuccess;
                result.Message = smsResult.Message;
            }

            result.IsSuccess = true;
            return result;



        }
    }
}
