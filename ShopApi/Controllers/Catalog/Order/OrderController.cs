using Application.Interfaces.Order;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Order
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        ///  لیست Order 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(OrderFilterDto model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await orderService.GetList(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///  لیست درخواست ها به صورت خروجی اکسل 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListForExcel")]
        public async Task<IActionResult> GetListForExcel()
        {
           // if (UserType == UserType.)
           // {
                var fileName = ExcelUtility.GenerateExcelFileName("orders");
                var exportbytes = await orderService.GetListForExcel(null, fileName);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
           // }
           // else { return BadRequest(); }

        }




        /// <summary>
        ///   برگرداندن Order 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
         //   if (UserType == UserType)
         //   {
                var result = await orderService.GetById(Guid.Parse(id));
                return Ok(result);
         //   }
         //   else { return BadRequest(); }

        }







        ///// <summary>
        /////     ثبت Order 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("Add")]
        //public async Task<IActionResult> Add(OrderDto model)
        //{
        //    var result = await orderService.Add(model , UserId);
        //    return Ok(result);
        //}



        /// <summary>
        ///   ویرایش Order  
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(OrderInputDto model)
        {
           // if (UserType == UserType)
           // {
                var result = await orderService.Update(model, UserId);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        ///// <summary>
        /////   حذف Order 
        ///// </summary>
        ///// <returns></returns> 
        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var result = await orderService.Delete(id);
        //    return Ok(result);
        //}


    }
}
