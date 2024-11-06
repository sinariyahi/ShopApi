using Application.Interfaces.Media;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.Video
{
    public class VideoController : BaseController
    {
        private readonly IVideoService VideoService;
        public VideoController(IVideoService VideoService)
        {
            this.VideoService = VideoService;
        }

        /// <summary>
        ///  لیست  ویدیو ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await VideoService.GetList(model);
            return Ok(result);
        }





        /// <summary>
        ///   برگرداندن یک  ویدیو 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await VideoService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت  ویدیو جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] VideoDto model)
        {
         //   if (UserType == UserType)
        //    {
                var result = await VideoService.Add(model);
                return Ok(result);
        //    }
         //   else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش  ویدیو
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] VideoDto model)
        {
          //  if (UserType == UserType)
          //  {
                var result = await VideoService.Update(model);
                return Ok(result);
         //   }
         //   else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف  ویدیو 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await VideoService.Delete(id);
                return Ok(result);
          //  }
         //   else { return BadRequest(); }

        }


    }
}
