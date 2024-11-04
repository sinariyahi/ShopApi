using Application.Interfaces.Media;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.Video
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVideoController : ControllerBase
    {
        private readonly IVideoService videoService;

        public UserVideoController(IVideoService videoService)
        {
            this.videoService = videoService;

        }

        /// <summary>
        ///  لیست ویدیو  ها 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllId")]
        public async Task<IActionResult> GetAllId()
        {
            var result = await videoService.GetAllId();
            return Ok(result);
        }



        /// <summary>
        ///  لیست ویدیو ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {

            var result = await videoService.GetUserList(model);
            return Ok(result);
        }


        /// <summary>
        ///  چند مقاله اخر  ویدیو ها 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLastVideo")]
        public async Task<IActionResult> GetLastVideo()
        {
            var result = await videoService.GetLastVideo();
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن یک مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await videoService.GetById(id);
            return Ok(result);
        }






    }
}
