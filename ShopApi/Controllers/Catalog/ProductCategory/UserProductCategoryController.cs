using Application.Interfaces.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.ProductCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService productCategoryService;
        public UserProductCategoryController(IProductCategoryService productCategoryService)
        {
            this.productCategoryService = productCategoryService;
        }
        /// <summary>
        ///   برگرداندن   گروه بندی کالا برای اسلایدر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList/{count}")]
        public async Task<IActionResult> GetUserList(int count)
        {
            var result = await productCategoryService.GetUserList(count);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن گروه بندی کالا  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserAllList")]
        public async Task<IActionResult> GetUserAllList()
        {
            var result = await productCategoryService.GetUserAllList();
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن  گروه بندی کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategoriesForMegaMenu")]
        public async Task<IActionResult> GetCategoriesForMegaMenu()
        {
            var result = await productCategoryService.GetCategoriesForMegaMenu();
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن ویژگیها برای  گروه بندی کالا   
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeaturesForSearch/{id}")]
        public async Task<IActionResult> GetFeaturesForSearch(int id)
        {
            var result = await productCategoryService.GetFeaturesForSearch(id);
            return Ok(result);
        }

        /// <summary>
        ///    برگرداندن گروه محصول  category     
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSubCategoryByCategoryAndBrand/{category}/{brand}")]
        public async Task<IActionResult> GetSubCategoryByCategoryAndBrand(string category, string brand)
        {
            var result = await productCategoryService.GetSubCategoryByCategoryAndBrand(category, brand);
            return Ok(result);
        }

        /// <summary>
        ///    برگرداندن گروه محصول sub category     
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeaturesBySubCategoryIdAndBrand/{category}/{brand}/{subCategory}")]
        public async Task<IActionResult> GetFeaturesBySubCategoryIdAndBrand(string category, string brand, string subCategory)
        {
            var result = await productCategoryService.GetFeaturesBySubCategoryId(category, brand, subCategory);
            return Ok(result);
        }






        /// <summary>
        ///   برگرداندن ویژگیهای یک گروه محصول    
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeaturesByCategoryAndBrand/{category}/{brand}")]
        public async Task<IActionResult> GetFeaturesByCategoryAndBrand(string category, string brand)
        {
            var result = await productCategoryService.GetFeaturesByCategoryId(category, brand);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن ویژگیهای  option گروه محصول     
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeatureOptionByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetFeatureOptionByCategoryId(int categoryId)
        {
            var result = await productCategoryService.GetFeatureOptionByCategoryId(categoryId);
            return Ok(result);
        }


        /// <summary>
        ///     گروه محصولات و برند ها      
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategoryByParentId/{category}")]
        public async Task<IActionResult> GetCategoryByParentId(string category)
        {
            var result = await productCategoryService.GetCategoryByParentId(category);
            return Ok(result);
        }




        /// <summary>
        ///  level 2 برگرداندن   گروه بندی کالا برای اسلایدر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserByParentId/{count}/{category}")]
        public async Task<IActionResult> GetUserByParentId(int count, string category)
        {
            var result = await productCategoryService.GetUserByParentId(count, category);
            return Ok(result);
        }



        /// <summary>
        ///  برگرداندن   گروه بندی کالا برای اسلایدر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await productCategoryService.GetUserById(id);
            return Ok(result);
        }




        /// <summary>
        ///  برگرداندن   گروه بندی کالا بر اساس برند ها  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategoryWithBrands/{category}")]
        public async Task<IActionResult> GetCategoryWithBrands(string category)
        {
            var result = await productCategoryService.GetCategoryWithBrands(category);
            return Ok(result);
        }




        /// <summary>
        ///  برگرداندن ویژگی های یک  محصولات   
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeatureOptionByCategory/{category}")]
        public async Task<IActionResult> GetFeatureOptionByCategory(string category)
        {
            var result = await productCategoryService.GetFeatureOptionByCategory(category);
            return Ok(result);
        }


        /// <summary>
        ///  برگرداندن ویژگی های یک  گروه محصول   
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeaturesByTitle/{category}")]
        public async Task<IActionResult> GetFeaturesByTitle(string category)
        {
            var result = await productCategoryService.GetFeaturesByTitle(category);
            return Ok(result);
        }

    }
}
