using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Order
{
    public interface IOrderService
    {
        Task<ShopActionResult<List<OrderDto>>> GetList(OrderFilterDto model = null);
        Task<ShopActionResult<UserPaymenstDto>> Add(UserOrderDto model, Guid userId);
        Task<ShopActionResult<Guid>> Update(OrderInputDto model, Guid userId);
        Task<ShopActionResult<Guid>> Delete(Guid id);
        Task<ShopActionResult<OrderDto>> GetById(Guid id);
        Task<ShopActionResult<List<OrderModel>>> GetDetailById(Guid userId);
        Task<byte[]> GetListForExcel(GridQueryModel model = null, string fileName = null);
        Task<ShopActionResult<Guid>> ReturnOrderByCustomer(ReturnOrderInputDto model, Guid userId);
    }
}
