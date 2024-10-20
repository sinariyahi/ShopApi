using Application.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.CustomReport
{
    public class OrganizationChartController : BaseController
    {
        private readonly IRoleService roleService;
        public OrganizationChartController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet("ShowTree")]
        public async Task<IActionResult> ShowTree()
        {
            var result = await roleService.GetOragnizationChartData();
            return Ok(result);
        }
    }
}
