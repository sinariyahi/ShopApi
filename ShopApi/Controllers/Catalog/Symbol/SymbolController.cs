using Application.Interfaces.Catalog;
using Application.Services.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Symbol
{
    public class SymbolController : BaseController
    {
        private readonly ISymbolService _service;
        public SymbolController(ISymbolService service)
        {
            _service = service;
        }

        /// <summary>
        ///   لیست Symbol
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await _service.GetList(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن Symbol 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن SymbolTree
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTree")]
        public async Task<IActionResult> GetTree(int? parentid)
        {
            var result = await _service.GetTree(parentid);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت Symbol جدید
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(SymbolDto model)
        {
            var result = await _service.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش Symbol 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(SymbolDto model)
        {
            var result = await _service.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف Symbol
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
