using Application.Interfaces.Base;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.ApplicationConfig
{
    public class ApplicationConfigController : BaseController
    {
        private readonly IApplicationConfigService applicationConfigService;
        public ApplicationConfigController(IApplicationConfigService applicationConfigService)
        {
            this.applicationConfigService = applicationConfigService;
        }

        /// <summary>
        ///   برگرداندن یک آیتم 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var result = await applicationConfigService.Get();
            return Ok(result);
        }

        /// <summary>
        ///   ویرایش آیتم 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] ApplicationConfigDto model)
        {
            //if (UserType == UserType.)
            //{
                var result = await applicationConfigService.Update(model);
                return Ok(result);
            //}
            //else
            //{
            //    return BadRequest();
            //}

        }
    }
}
