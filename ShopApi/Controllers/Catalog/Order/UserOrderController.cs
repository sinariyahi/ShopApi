using Application.Interfaces.Catalog;
using Application.Interfaces.Order;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Order
{
    public class UserOrderController : BaseController
    {
        private readonly IKoponService _koponservice;
        private readonly IOrderService _orderservice;

        public UserOrderController(IKoponService _koponservice, IOrderService _orderservice)
        {
            this._koponservice = _koponservice;
            this._orderservice = _orderservice;

        }
        /// <summary>
        ///     چک کردن کوپن 
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckKopon/{code}")]
        public async Task<IActionResult> CheckKopon(string code)
        {
            var result = await _koponservice.GetByCode(code);
            return Ok(result);
        }
        /// <summary>
        ///       ثبت درخواست 
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(UserOrderDto model)
        {
            var result = await _orderservice.Add(model, UserId);
            return Ok(result);
        }


        /// <summary>
        ///       مرجوع کردن درخواست  
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReturnOrderByCustomer")]
        public async Task<IActionResult> ReturnOrderByCustomer(ReturnOrderInputDto model)
        {
            var result = await _orderservice.ReturnOrderByCustomer(model, UserId);
            return Ok(result);
        }

        /// <summary>
        ///    جزئیات سفارش 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDetailById")]
        public async Task<IActionResult> GetDetailById()
        {
            var result = await _orderservice.GetDetailById(UserId);
            return Ok(result);
        }





    }
}
