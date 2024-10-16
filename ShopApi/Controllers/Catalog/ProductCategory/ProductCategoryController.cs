using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.ProductCategory
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            this.productCategoryService = productCategoryService;
        }

        /// <summary>
        ///  لیست گروه بندی کالا ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await productCategoryService.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///  جستجو لیست گروه بندی کالا ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search(SearchInputDto model)
        {
            if (string.IsNullOrEmpty(model.Code) && string.IsNullOrEmpty(model.Text))
            {
                var treeResult = await productCategoryService.GetTree();
                return Ok(treeResult);
            }

            var result = await productCategoryService.Search(model.Text, model.Code);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن یک گروه بندی کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await productCategoryService.GetById(id);
            return Ok(result);
        }







        /// <summary>
        ///     ثبت گروه بندی کالا جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] ProductCategoryDto model)
        {
          //  if (UserType == UserType)
          //  {
                var result = await productCategoryService.Add(model);
                return Ok(result);
          //  }
          //  else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش گروه بندی کالا 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] ProductCategoryDto model)
        {
          //  if (UserType == UserType)
          //  {
                var result = await productCategoryService.Update(model);
                return Ok(result);
          //  }
          //  else { return BadRequest(); }
        }


        /// <summary>
        ///   حذف گروه بندی کالا 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await productCategoryService.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///  نمایش گروه ها به صورت درختواره
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetTree")]
        public async Task<IActionResult> GetTree(int? categoryId = null)
        {
            var result = await productCategoryService.GetTree(categoryId);
            return Ok(result);
        }

        /// <summary>
        ///  نمایش ویژگی های یک گروه
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetFeatures/{categoryId}")]
        public async Task<IActionResult> GetFeatures(int categoryId)
        {
            var result = await productCategoryService.GetFeatures(categoryId);
            return Ok(result);
        }


        /// <summary>
        ///  نمایش ویژگی های یک گروه برای جستجو
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetFeaturesForFilter/{categoryId}")]
        public async Task<IActionResult> GetFeaturesForFilter(int categoryId)
        {
            var result = await productCategoryService.GetFeaturesForSearch(categoryId);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن آمار گروه بندی ها برای داشبورد 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductCategoryForDashboard")]
        public async Task<IActionResult> GetProductCategoryForDashboard()
        {
            var result = await productCategoryService.GetProductCategoryForDashboard();
            return Ok(result);
        }
    }
}
