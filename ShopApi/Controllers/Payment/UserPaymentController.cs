using Application.Interfaces.Payment;
using Infrastructure.Models.Payment;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Payment
{
    public class UserPaymentController : BaseController
    {
        private readonly IUserPaymentService service;
        private readonly ILogger logger;

        public UserPaymentController(IUserPaymentService service, ILogger<UserPaymentController> logger)
        {
            this.service = service;
            this.logger = logger;
        }





        /// <summary>
        ///   برگرداندن تاریخچه پرداخت ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUserAllPayments")]
        public async Task<IActionResult> GetUserAllPayments(UserPaymentFilterDto model)
        {
            var result = await service.GetUserAllPayments(model, UserId);
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن تاریخچه پرداخت های یک کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserAllPaymentsForUser")]
        public async Task<IActionResult> GetUserAllPaymentsForUser()
        {
            var result = await service.GetUserAllPaymentsForUser(UserId);
            return Ok(result);
        }




        /// <summary>
        ///    درخواست ثبت پرداخت
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddUserPayment")]
        public async Task<IActionResult> AddUserPayment(UserPaymentDto model)
        {


            //var result = await service.AddUserPayment(model, UserId , true);
            return Ok();



        }


    }
}

