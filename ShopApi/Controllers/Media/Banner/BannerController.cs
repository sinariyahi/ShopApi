using Application.Interfaces.Media;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.Banner
{
    public class BannerController : BaseController
    {
        private readonly IBannerService bannerService;
        public BannerController(IBannerService bannerService)
        {
            this.bannerService = bannerService;
        }

        /// <summary>
        ///  لیست  banner ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
          //  if (UserType == UserType.)
          //  {
                var result = await bannerService.GetList(model);
                return Ok(result);
         //   }
         //   else { return BadRequest(); }

        }





        /// <summary>
        ///   برگرداندن یک  banner 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await bannerService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت  banner جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] BannerDto model)
        {
           // if (UserType == UserType.TaminPlus)
           // {
                var result = await bannerService.Add(model);
                return Ok(result);
           // }
          // else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش  banner
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] BannerDto model)
        {
           // if (UserType == UserType.TaminPlus)
           // {
                var result = await bannerService.Update(model);
                return Ok(result);
          //  }
          //  else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف  banner 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType.TaminPlus)
           // {
                var result = await bannerService.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


    }
}
