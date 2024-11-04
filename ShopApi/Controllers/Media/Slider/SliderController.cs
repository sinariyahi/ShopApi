using Application.Interfaces.Media;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media.Slider
{
    public class SliderController : BaseController
    {
        private readonly ISliderService sliderService;
        public SliderController(ISliderService sliderService)
        {
            this.sliderService = sliderService;
        }

        /// <summary>
        ///  لیست  Slider ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await sliderService.GetList(model);
            return Ok(result);
        }





        /// <summary>
        ///   برگرداندن یک  Slider 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await sliderService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت  Slider جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] SliderDto model)
        {
            var result = await sliderService.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش  Slider
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] SliderDto model)
        {
            var result = await sliderService.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف  Slider 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await sliderService.Delete(id);
            return Ok(result);
        }


    }
}
