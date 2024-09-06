using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.City
{
    public class CityController : BaseController
    {
        private readonly ICityService service;
        public CityController(ICityService service)
        {
            this.service = service;
        }
        /// <summary>
        ///  لیست  City ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
          //  if (UserType == UserType.)
          //  {
                var result = await service.GetList(model);
                return Ok(result);
          //  }
          // else { return BadRequest(); }
        }

        /// <summary>
        ///     ثبت City
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CityInputModel model)
        {
           // if (UserType == UserType.)
           // {
                var result = await service.Add(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش City 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CityInputModel model)
        {
           // if (UserType == UserType.)
           // {
                var result = await service.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }




        /// <summary>
        ///   برگرداندن یک  City 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           // if (UserType == UserType.)
           // {
                var result = await service.GetById(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///   حذف  City
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType.)
           // {
                var result = await service.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


    }
}
