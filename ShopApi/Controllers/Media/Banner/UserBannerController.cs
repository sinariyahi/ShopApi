using Application.Interfaces.Media;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.Banner
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBannerController : ControllerBase
    {
        private readonly IBannerService bannerService;
        public UserBannerController(IBannerService bannerService)
        {
            this.bannerService = bannerService;
        }

        /// <summary>
        ///  لیست  banner  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList/{positionPlace}")]
        public async Task<IActionResult> GetUserList(PositionPlace positionPlace)
        {
            var result = await bannerService.GetUserList(positionPlace);
            return Ok(result);
        }





    }
}
