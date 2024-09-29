using Application.Interfaces.Blog;
using Infrastructure.Common;
using Infrastructure.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Blog.Article
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService articleService;
        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        /// <summary>
        ///  لیست مقاله ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {

            var result = await articleService.GetList(model);
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

        /// <summary>
        ///     ثبت کالا مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] ArticleDto model)
        {
            //if (UserType == UserType)
            //{
                model.UserId = UserId;
                var result = await articleService.Add(model);
                return Ok(result);
            //}
            //else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش مقاله 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] ArticleDto model)
        {



            //if (UserType == UserType)
            //{
                model.UserId = UserId;
                var result = await articleService.Update(model);
                return Ok(result);
            //}
            //else { return BadRequest(); }
        }


        /// <summary>
        ///   حذف مقاله 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //if (UserType == UserType)
            //{
                var result = await articleService.Delete(id);
                return Ok(result);
            //}
            //else { return BadRequest(); }

        }






    }
}
