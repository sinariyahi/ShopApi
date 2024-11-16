using Application.Interfaces.Security;
using Infrastructure.Common;
using Infrastructure.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Security
{

    public class CustomerController : BaseController
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }


        /// <summary>
        ///     ثبت مشتری جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerInputModel model)
        {

            var result = await customerService.AddCustomer(model);
            return Ok(result);
        }


        /// <summary>
        ///     ویرایش مشتری جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(CustomerInputModel model)
        {
            var result = await customerService.UpdateCustomer(model, true);
            return Ok(result);
        }



        /// <summary>
        ///     ویرایش مشتری توسط خود کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateCustomerByUser")]
        public async Task<IActionResult> UpdateCustomerByUser(CustomerInputModel model)
        {
            model.Id = UserId;
            var result = await customerService.UpdateCustomerByUser(model);
            return Ok(result);
        }


        /// <summary>
        ///     ویرایش مشتری توسط خود ارسال محصول برای کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateCustomerInfoByUser")]
        public async Task<IActionResult> UpdateCustomerInfoByUser(CustomerInputModel model)
        {
            model.Id = UserId;
            var result = await customerService.UpdateCustomerInfoByUser(model);
            return Ok(result);
        }

        /// <summary>
        ///  لیست مشتریان 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCustomers")]
        public async Task<IActionResult> GetCustomers(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await customerService.GetCustomers(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///   برگرداندن یک مشتری 
        /// <returns></returns>
        [HttpGet("GetCustomerById/{userId}")]
        public async Task<IActionResult> GetCustomerById(Guid userId)
        {
            //if (UserType == UserType)
           // {
                var result = await customerService.GetCustomerById(userId);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///   برگرداندن یک مشتری توسط خود کاربر 
        /// <returns></returns>
        [HttpGet("GetCustomerByUser")]
        public async Task<IActionResult> GetCustomerByUser()
        {
            var result = await customerService.GetCustomerById(UserId);
            return Ok(result);
        }
    }
}
