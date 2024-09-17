using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.JobOpportunity
{
  public class JobOpportunityController : BaseController
    {
        private readonly IJobOpportunityService _service;
        public JobOpportunityController(IJobOpportunityService service)
        {
            _service = service;
        }

        /// <summary>
        ///   لیست فرصت های شغلی
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.GetList(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///   برگرداندن  فرصت های شغلی
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           // if (UserType == UserType)
           // {
                var result = await _service.GetById(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///     ثبت فرصت های شغلی جدید
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(JobOpportunityDto model)
        {
         //   if (UserType == UserType)
         //   {
                var result = await _service.Add(model);
                return Ok(result);
         //   }
         //   else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایشفرصت های شغلی 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(JobOpportunityDto model)
        {
           // if (UserType == UserType.)
           // {
                var result = await _service.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }


        }


        /// <summary>
        ///   حذف فرصت های شغلی
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (UserType == UserType.)
           // {
                var result = await _service.Delete(id);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }
    }
}

