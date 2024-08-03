using Application.Interfaces.Dashboard;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Origin> _queryService;

        public DashboardService(BIContext context)
        {
            this.context = context;
        }




        public async Task<ShopActionResult<List<DashboardDto>>> GetCountData()
        {
            var result = new ShopActionResult<List<DashboardDto>>();

            var list = new List<DashboardDto>();
            var model = new DashboardDto();


            model = new DashboardDto();

            model.Total = await context.Products.Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling).CountAsync();
            model.Icon = "AiOutlinePieChart";
            model.Title = "تعداد محصولات";
            model.Color = "purple";
            model.Path = "/admin/catalog/products";

            list.Add(model);

            model = new DashboardDto();

            model.Icon = "BsBagPlus";
            model.Title = "تعداد سفارشات";
            model.Color = "black";
            model.Path = "/admin/catalog/orders";
            model.Total = await context.Orders.Where(w => w.OrderStatus == OrderStatus.InProgress || w.OrderStatus == OrderStatus.Register).CountAsync();
            list.Add(model);




            model = new DashboardDto();
            model.Total = await context.Customers.CountAsync();
            model.Icon = "FiUsers";
            model.Title = "تعداد مشتریان";
            model.Color = "orange";
            model.Path = "/admin/customers";
            list.Add(model);

            model = new DashboardDto();
            model.Total = await context.UserLogSearchForProducts.CountAsync();
            model.Icon = "MdOutlineManageSearch";
            model.Title = "تعداد جستجوی کاربران";
            model.Color = "#7feff3";
            model.Path = "/user-log-search";

            list.Add(model);

            result.Data = list;
            result.IsSuccess = true;

            return result;
        }

    }
}
