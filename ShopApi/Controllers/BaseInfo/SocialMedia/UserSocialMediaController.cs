using Application.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.SocialMedia
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSocialMediaController : ControllerBase
    {
        private readonly ISocialMediaService _service;
        public UserSocialMediaController(ISocialMediaService service)
        {
            _service = service;
        }
        /// <summary>
        ///   برگرداندن  SocialMediaController 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList/{count}")]
        public async Task<IActionResult> GetUserList(int count)
        {
            var result = await _service.GetUserList(count);
            return Ok(result);
        }


    }
}
