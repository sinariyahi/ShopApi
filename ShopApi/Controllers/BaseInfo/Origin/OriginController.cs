using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.Origin
{
    public class OriginController : BaseController
    {
        private readonly IOriginService _service;
        public OriginController(IOriginService service)
        {
            _service = service;
        }

        /// <summary>
        ///   لیست منطقه یا کشور
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await _service.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن منطقه یا کشور 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت منطقه یا کشور جدید
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(OriginDto model)
        {
            var result = await _service.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش منطقه یا کشور 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(OriginDto model)
        {
            var result = await _service.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف منطقه یا کشور
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
