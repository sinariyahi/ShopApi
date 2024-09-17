using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using XAct.Messages;

namespace ShopApi.Controllers.BaseInfo.NewsLetter
{
    public class NewsLetterController : BaseController
    {
        private readonly INewsLetterService _service;
        readonly IMemoryCache cache;

        public NewsLetterController(INewsLetterService service, IMemoryCache cache)
        {
            _service = service;
            this.cache = cache;

        }

        /// <summary>
        ///   لیست خبر نامه ها
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
           // if (UserType == UserType.)
           // {
                var result = await _service.GetList(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///   لیست اعضا ثبت نام شده در خبر نامه ها
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUserRegisterNewsLetter")]
        public async Task<IActionResult> GetUserRegisterNewsLetter(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.GetUserRegisterNewsLetter(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///   برگرداندن خبر نامه 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت خبر نامه جدید
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(NewsLetterDto model)
        {
           // if (UserType == UserType.)
           // {
                var result = await _service.Add(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }


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
               // captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }

            var result = await _service.Register(model, UserId);
            return Ok(result);
        }




        /// <summary>
        ///   ویرایش خبر نامه 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(NewsLetterDto model)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف خبر نامه
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }
    }
}
