using Application.Interfaces.Support;
using Infrastructure.Common;
using Infrastructure.Models.Support;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.Support.LetMeKnow
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLetMeKnowController : ControllerBase
    {
        private readonly ILetMeKnowService _service;
        readonly IMemoryCache cache;
        public UserLetMeKnowController(ILetMeKnowService service, IMemoryCache cache)
        {
            _service = service;
            this.cache = cache;

        }

        /// <summary>
        ///   خبر بده 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Add(UserLetMeKnowDto model)
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
