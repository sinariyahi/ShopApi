using Application.Interfaces.Blog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Blog.ArticleCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserArticleCategoryController : ControllerBase
    {
        private readonly IArticleCategoryService articleCategoryService;

        public UserArticleCategoryController(IArticleCategoryService articleCategoryService)
        {
            this.articleCategoryService = articleCategoryService;
        }


        /// <summary>
        ///   برگرداندن یک مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await articleCategoryService.GetById(id);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن یک مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            var result = await articleCategoryService.GetUserList();
            return Ok(result);
        }







    }
}
