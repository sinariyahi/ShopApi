using Application.Interfaces.Security;
using Infrastructure.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public UserCustomerController(ICustomerService customerService)
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
            var result = await customerService.AddCustomer(model, false);
            return Ok(result);
        }


        /// <summary>
        ///      
        /// </summary>
        /// <returns></returns>
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerInputModel model)
        {
            var result = await customerService.RegisterCustomer(model);
            return Ok(result);
        }


    }
}
