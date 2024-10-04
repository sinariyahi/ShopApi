using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Brand
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _service;
        public BrandController(IBrandService service)
        {
            _service = service;
        }

        /// <summary>
        ///   لیست برند
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await _service.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن برند 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن برند بر اساس محصولات 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBrandProductsForDashboard")]
        public async Task<IActionResult> GetBrandProductsForDashboard()
        {
            var result = await _service.GetBrandProductsForDashboard();
            return Ok(result);
        }


        /// <summary>
        ///     ثبت برند
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] BrandInputModel model)
        {
           // if (UserType == UserType)
           // {

                var result = await _service.Add(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش برند 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] BrandInputModel model)
        {
           // if (UserType == UserType)
           // {

                var result = await _service.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///   حذف برند
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }
    }
}
