using Application.Interfaces.Blog;
using Infrastructure.Common;
using Infrastructure.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Blog.ArticleCategory
{
    public class ArticleCategoryController : BaseController
    {
        private readonly IArticleCategoryService articleCategoryService;
        public ArticleCategoryController(IArticleCategoryService articleCategoryService)
        {
            this.articleCategoryService = articleCategoryService;
        }

        /// <summary>
        ///  لیست گروه بندی مقاله ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await articleCategoryService.GetList(model);
            return Ok(result);
        }





        /// <summary>
        ///   برگرداندن یک گروه بندی مقاله 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await articleCategoryService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت گروه بندی مقاله جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(ArticleCategoryDto model)
        {
           // if (UserType == UserType)
           // {
                var result = await articleCategoryService.Add(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش گروه بندی مقاله 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ArticleCategoryDto model)
        {
            //if (UserType == UserType)
            //{
                var result = await articleCategoryService.Update(model);
                return Ok(result);
            //}
            //else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف گروه بندی مقاله 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //if (UserType == UserType)
            //{
                var result = await articleCategoryService.Delete(id);
                return Ok(result); ;
            //}
            //else { return BadRequest(); }

        }


    }
}
