using Application.Interfaces.Media;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.VideoCategory
{
    public class VideoCategoryController : BaseController
    {
        private readonly IVideoCategoryService videoCategoryService;
        public VideoCategoryController(IVideoCategoryService videoCategoryService)
        {
            this.videoCategoryService = videoCategoryService;
        }

        /// <summary>
        ///  لیست گروه بندی ویدیو ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await videoCategoryService.GetList(model);
            return Ok(result);
        }





        /// <summary>
        ///   برگرداندن یک گروه بندی ویدیو 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await videoCategoryService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت گروه بندی ویدیو جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(VideoCategoryDto model)
        {
         //   if (UserType == UserType)
         //   {
                var result = await videoCategoryService.Add(model);
                return Ok(result);
         //   }
         //   else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش گروه بندی ویدیو
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update(VideoCategoryDto model)
        {
           // if (UserType == UserType)
           // {
                var result = await videoCategoryService.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف گروه بندی ویدیو 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await videoCategoryService.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


    }
}
