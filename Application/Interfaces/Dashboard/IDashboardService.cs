using Infrastructure.Common;
using Infrastructure.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<ShopActionResult<List<DashboardDto>>> GetCountData();
    }
}
