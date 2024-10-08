using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Feature
{
    public class FeatureController : BaseController
    {
        private readonly IFeatureService featureService;
        public FeatureController(IFeatureService featureService)
        {
            this.featureService = featureService;
        }

        /// <summary>
        ///  لیست ویژگی ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await featureService.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن یک ویژگی 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await featureService.GetById(id);
            return Ok(result);
        }


        /// <summary>
        ///FeaturesCategoryId  برگرداندن یک ویژگی توسط  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByFeaturesCategoryId")]
        public async Task<IActionResult> GetByFeaturesCategoryId(int? id)
        {
            var result = await featureService.GetByFeaturesCategoryId(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت ویژگی جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(FeatureDto model)
        {
            var result = await featureService.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش ویژگی 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update(FeatureDto model)
        {
            var result = await featureService.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف ویژگی 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await featureService.Delete(id);
            return Ok(result);
        }
    }
}
