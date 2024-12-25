using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.Support.UserNewsLetter
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNewsLetterController : ControllerBase
    {
        private readonly INewsLetterService _service;
        readonly IMemoryCache cache;
        public UserNewsLetterController(INewsLetterService service, IMemoryCache cache)
        {
            _service = service;
            this.cache = cache;

        }

        /// <summary>
        ///    ثبت عضویت در خبرنامه
        /// </summary>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Add(UserNewsLetterDto model)
        {

            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
                captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }

            var result = await _service.Register(model, null);
            return Ok(result);
        }



    }
}
