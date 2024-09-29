using Application.Interfaces.Blog;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Blog.Article
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserArticleController : ControllerBase
    {
        private readonly IArticleService articleService;
        private readonly IArticleCategoryService articleCategoryService;

        public UserArticleController(IArticleService articleService)
        {
            this.articleService = articleService;

        }

        /// <summary>
        ///  لیست مقاله ها 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllId")]
        public async Task<IActionResult> GetAllId()
        {
            var result = await articleService.GetAllId();
            return Ok(result);
        }



        /// <summary>
        ///  لیست مقاله ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {

            var result = await articleService.GetUserList(model);
            return Ok(result);
        }


        /// <summary>
        ///  چند مقاله اخر  مقاله ها 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLastArticle")]
        public async Task<IActionResult> GetLastArticle()
        {
            var result = await articleService.GetLastArticle();
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن یک مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await articleService.GetById(id);
            return Ok(result);
        }





    }
}
