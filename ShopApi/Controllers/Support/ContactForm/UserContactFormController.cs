using Application.Interfaces.Support;
using Infrastructure.Common;
using Infrastructure.Models.Support;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using XAct.Messages;

namespace ShopApi.Controllers.Support.ContactForm
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserContactFormController : ControllerBase
    {
        private readonly IContactFormService _service;
        readonly IMemoryCache cache;
        public UserContactFormController(IContactFormService service, IMemoryCache cache)
        {
            _service = service;
            this.cache = cache;

        }

        /// <summary>
        ///     ثبت فرم همکاری با ما
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] ContactFormInputModel model)
        {

            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
                captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }

            var result = await _service.Add(model);
            return Ok(result);
        }



    }
}
