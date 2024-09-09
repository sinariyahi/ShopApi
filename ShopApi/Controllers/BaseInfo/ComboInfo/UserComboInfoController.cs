using Application.Interfaces.Base;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.BaseInfo.ComboInfo
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserComboInfoController : ControllerBase
    {
        private readonly IComboInfoService _comboService;
        public UserComboInfoController(IComboInfoService comboService, IMemoryCache cache)
        {
            _comboService = comboService;
        }



        /// <summary>
        /// محصولات   Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductsByCategoryIdForUser/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryIdForUser(int categoryId)
        {
            var result = await _comboService.GetProductsByCategoryIdForUser(categoryId);
            return Ok(result);
        }

        /// <summary>
        ///فرصت های شغلی  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJobOpportunities")]
        public async Task<IActionResult> GetJobOpportunities()
        {
            var result = await _comboService.GetJobOpportunities();
            return Ok(result);
        }

        /// <summary>
        ///ها Brand  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _comboService.GetBrands();
            return Ok(result);
        }

        /// <summary>
        /// GetCities   Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCities/{provinceId}")]
        public async Task<IActionResult> GetCities(Province provinceId)
        {
            var result = await _comboService.GetCities(provinceId);
            return Ok(result);
        }


        /// <summary>
        /// استانها  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProvince")]
        public async Task<IActionResult> GetProvince()
        {
            var result = await _comboService.GetProvince();
            return Ok(result);
        }

        /// <summary>
        ///ها ProductCategory  Dropdown 
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetProductCategory")]
        public async Task<IActionResult> GetProductCategory()
        {
            var result = await _comboService.GetProductCategory();
            return Ok(result);
        }


        /// <summary>
        /// Get All Products  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProductsForUser")]
        public async Task<IActionResult> GetAllProductsForUser()
        {
            var result = await _comboService.GetAllProductsForUser();
            return Ok(result);
        }






    }
}
