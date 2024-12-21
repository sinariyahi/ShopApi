using Application.Interfaces.Support;
using Infrastructure.Common;
using Infrastructure.Models.Support;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.Support.LetMeKnow
{
    public class LetMeKnowController : BaseController
    {
        private readonly ILetMeKnowService _service;
        readonly IMemoryCache cache;
        public LetMeKnowController(ILetMeKnowService service, IMemoryCache cache)
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

            var result = await _service.Register(model, UserId);
            return Ok(result);
        }


        /// <summary>
        ///  لیست  افرادی که گفتن به من خبر بده ! 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.GetList(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }



        /// <summary>
        ///  لیست افراد ب من خبر بده به صورت خروجی اکسل 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListForExcel")]
        public async Task<IActionResult> GetListForExcel()
        {
          //  if (UserType == UserType)
          //  {
                var fileName = ExcelUtility.GenerateExcelFileName("LetMeKnow");
                var exportbytes = await _service.GetListForExcel(null, fileName);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
          //  }
          //  else { return BadRequest(); }

        }


    }
}
