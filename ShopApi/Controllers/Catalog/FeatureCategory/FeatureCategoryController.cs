using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.FeatureCategory
{
    public class FeatureCategoryController : BaseController
    {
        private readonly IFeatureCategoryService _service;
        public FeatureCategoryController(IFeatureCategoryService service)
        {
            _service = service;
        }

        /// <summary>
        ///   لیست FeatureCategory
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await _service.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن FeatureCategory 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت FeatureCategory جدید
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(FeatureCategoryDto model)
        {
            var result = await _service.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش FeatureCategory 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(FeatureCategoryDto model)
        {
            var result = await _service.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف FeatureCategory
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
