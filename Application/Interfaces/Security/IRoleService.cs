using Infrastructure.Common;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IRoleService
    {
        Task<ShopActionResult<List<RoleDto>>> GetRoles(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(RoleDto model);
        Task<ShopActionResult<int>> Update(RoleDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<RoleDto>> GetById(int id);
        Task<ShopActionResult<List<TreeDto>>> GetRoleTree(int? parentId);
        Task<ShopActionResult<List<RoleGroupPermissionDto>>> GetRolePermissions(int roleId);
        Task<ShopActionResult<int>> UpdateRolePermissions(int roleId, List<RolePermissionDto> permissions);
        Task<ShopActionResult<List<TreeDto>>> GetOragnizationChartData(int? parentId = null);
    }
}
