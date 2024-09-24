using Application.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.Page
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPageController : ControllerBase
    {
        private readonly IPageService pageService;

        public UserPageController(IPageService pageService)
        {
            this.pageService = pageService;

        }




        /// <summary>
        ///   برگرداندن یک صفحه 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var result = await pageService.GetByTitle(title);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن لیست id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLink")]
        public async Task<IActionResult> GetAllLink()
        {
            var result = await pageService.GetAllLink();
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن لیست 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListUserPages")]
        public async Task<IActionResult> GetListUserPages()
        {
            var result = await pageService.GetListUserPages();
            return Ok(result);
        }



    }
}
