using Application.Interfaces.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Brand
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBrandController : ControllerBase
    {
        private readonly IBrandService _service;
        public UserBrandController(IBrandService service)
        {
            _service = service;
        }
        /// <summary>
        ///   برگرداندن برند ها 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList/{count}")]
        public async Task<IActionResult> GetUserList(int count)
        {
            var result = await _service.GetUserList(count);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن برند ها با محصولات برای منو 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBrandMenu")]
        public async Task<IActionResult> GetBrandMenu()
        {
            var result = await _service.GetBrandMegaMenu();
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن برند ها با محصولات برای فیلتر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBrandMegaMenu")]
        public async Task<IActionResult> GetAllBrandMegaMenu()
        {
            var result = await _service.GetAllBrandMegaMenu();
            return Ok(result);
        }


        /// <summary>
        ///    برگرداندن برند یک productCategory 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserListByProductCategoryId/{category}")]
        public async Task<IActionResult> GetUserListByProductCategoryId(string category)
        {
            var result = await _service.GetUserListByProductCategoryId(category);
            return Ok(result);
        }

        /// <summary>
        ///    برگرداندن برند یک قطعه 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserProductCategoryByBrandId/{brandId}")]
        public async Task<IActionResult> GetUserProductCategoryByBrandId(int brandId)
        {
            var result = await _service.GetUserProductCategoryByBrandId(brandId);
            return Ok(result);
        }



        /// <summary>
        ///    برگرداندن برند یک قطعه 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserProductCategoryByBrandTitle/{brand}")]
        public async Task<IActionResult> GetUserProductCategoryByBrandTitle(string brand)
        {
            var result = await _service.GetUserProductCategoryByBrandTitle(brand);
            return Ok(result);
        }

    }
}
