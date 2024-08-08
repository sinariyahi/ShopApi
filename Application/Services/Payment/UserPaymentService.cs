using Application.Interfaces.Base;
using Application.Interfaces.Payment;
using Application.Interfaces.Security;
using Application.Interfaces.Sms;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Payment
{
    public class UserPaymentService : IUserPaymentService
    {
        private readonly BIContext context;

        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly ISmsService smsService;
        private readonly IComboInfoService comboInfoService;


        public UserPaymentService(BIContext context, ILogger<UserPaymentService> logger, IUserService userService, ISmsService smsService, IComboInfoService comboInfoService)
        {
            this.context = context;
            this.logger = logger;
            this.userService = userService;
            this.smsService = smsService;
            this.comboInfoService = comboInfoService;
        }





        public async Task<ShopActionResult<List<UserPaymenstDto>>> GetUserAllPayments(UserPaymentFilterDto model, Guid userId)
        {
            var result = new ShopActionResult<List<UserPaymenstDto>>();
            var skipCount = (model.Page - 1) * model.Size;
            var userData = await context.Users.FindAsync(userId);
            if (userData.UserType == UserType.Customer)
            {
                result.IsSuccess = false;

               // result.Message = MessagesFA.YouDoNotHaveAccess;
                return result;
            }
            var filterModel = new UserPaymentFilterDto();

            foreach (var item in model.Filtered)
            {
                if (item.column == "title")
                {
                    filterModel.Title = item.value;
                }
                if (item.column == "firstName")
                {
                    filterModel.FirstName = item.value;
                }
                if (item.column == "lastName")
                {
                    filterModel.LastName = item.value;
                }
                if (item.column == "mobile")
                {
                    filterModel.Mobile = item.value;
                }

                if (item.column == "isSuccess")
                {
                    filterModel.IsSuccess = Convert.ToBoolean(item.value);
                }
                if (item.column == "fromDate")
                {
                    filterModel.FromDate = Convert.ToDateTime(item.value);
                }
                if (item.column == "toDate")
                {
                    filterModel.ToDate = Convert.ToDateTime(item.value);
                }
                if (item.column == "token")
                {
                    filterModel.Token = item.value;
                }
            }




            var query = context.UserPayments
                .Include(i => i.User)
                .Where(q =>
                    (string.IsNullOrEmpty(filterModel.Mobile) || q.User.UserName.Contains(filterModel.Mobile)) &&
                    (string.IsNullOrEmpty(filterModel.FirstName) || q.User.FirstName.Contains(filterModel.FirstName)) &&
                    (string.IsNullOrEmpty(filterModel.LastName) || q.User.LastName.Contains(filterModel.LastName)) &&
                    (filterModel.IsSuccess == null || q.IsSuccess == filterModel.IsSuccess) &&
                    (filterModel.FromDate == null || q.OrderDate.Date >= filterModel.FromDate.Value) &&
                    (filterModel.ToDate == null || q.OrderDate.Date <= filterModel.ToDate.Value) &&
                    (filterModel.Token == null || q.TrackingCode == filterModel.Token)

                )
            .AsQueryable();


            var tempResult = query.Select(q => new UserPaymenstDto
            {
                FirstName = q.User.FirstName,
                LastName = q.User.LastName,
                Amount = q.Amount,
                Token = q.TrackingCode,
                PaymentResult = q.PaymentResult,
                BankConfirmResult = q.BankConfirmResult,
                OrderDate = DateUtility.CovertToShamsi(q.OrderDate),
                Status = q.IsSuccess == null ? "پرداخت ناموفق" : q.IsSuccess == true ? " پرداخت موفق" : "پرداخت ناموفق"
            }).ToList();
            tempResult = tempResult.Where(w => (string.IsNullOrEmpty(filterModel.Title) || w.Title.Contains(filterModel.Title))).ToList();

            tempResult = tempResult.OrderByDescending(q => q.OrderDate).Skip(skipCount).Take(model.Size).ToList();

            result.Data = tempResult;
            result.IsSuccess = true;
            result.Total = query.Count();
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }
        public async Task<ShopActionResult<List<UserPaymenstModel>>> GetUserAllPaymentsForUser(Guid userId)
        {
            var result = new ShopActionResult<List<UserPaymenstModel>>();

            var filterModel = new UserPaymentFilterDto();





            var query = context.UserPayments
                .Include(i => i.User)
                .Where(q =>
                q.UserId == userId

                )
            .AsQueryable();


            var tempResult = query.Select(q => new UserPaymenstModel
            {
                OrderNumber = q.OrderValue,
                Amount = q.Amount,
                Token = q.TrackingCode,
                OrderDate = DateUtility.CovertToShamsi(q.OrderDate),
                Status = q.IsSuccess == null ? "پرداخت ناموفق" : q.IsSuccess == true ? " پرداخت موفق" : "پرداخت ناموفق"
            }).ToList();

            tempResult = tempResult.OrderByDescending(q => q.OrderDate).ToList();

            result.Data = tempResult;
            result.IsSuccess = true;

            return result;
        }

        public async Task<ShopActionResult<BankResultDto>> GetBankResult(string refId, Guid userId)
        {
            var result = new ShopActionResult<BankResultDto>();
            result.Data = new BankResultDto();


            var userPayment = await context.UserPayments.FirstOrDefaultAsync(q => q.TrackingCode == refId && q.UserId == userId);
            result.Data.Status = userPayment.IsSuccess == true ? 1 : 0;
            result.Data.Amount = userPayment.Amount;
            result.Data.OrderNumber = userPayment.OrderValue;

            result.Data.Token = userPayment.TrackingCode;

            if (userPayment == null)
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.InvalidToken;
              //  result.Data.Message = MessagesFA.InvalidToken;
                return result;
            }




            await context.SaveChangesAsync();

            result.IsSuccess = true;
            result.Data.Amount = userPayment.Amount;
            result.Data.Token = userPayment.TrackingCode;
            //result.Message = MessagesFA.PaymentWasSuccessful;
            result.Data.Message = userPayment.PaymentResult;
            return result;
        }

    }
}
