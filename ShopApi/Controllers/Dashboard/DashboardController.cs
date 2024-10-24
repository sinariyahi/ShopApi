using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        /// <summary>
        /// تعداد اپشن ها
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountData")]
        public async Task<IActionResult> GetCountData()
        {
          //  if (UserType == UserType)
            {
                var result = await dashboardService.GetCountData();
                return Ok(result);
            }
          //  else { return BadRequest(); }

        }


    }
}
