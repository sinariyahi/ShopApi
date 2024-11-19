using Application.Interfaces.Base;
using Application.Interfaces.Security;
using Infrastructure.Common;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Security
{
    public class RoleController : BaseController
    {
        private readonly IRoleService roleService;
        private readonly IComboInfoService comboService;

        public RoleController(IRoleService roleService, IComboInfoService comboService)
        {
            this.roleService = roleService;
            this.comboService = comboService;
        }

        /// <summary>
        ///   Role لیست 
        /// </summary>
        /// <returns></returns>

        [HttpPost("GetRole")]
        public async Task<IActionResult> GetRole(GridQueryModel model = null)
        {
            var result = await roleService.GetRoles(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن یک نقش 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await roleService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت نقش جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(RoleDto model)
        {
            model.UserId = UserId;
            var result = await roleService.Add(model);
            return Ok(result);
        }

        /// <summary>
        ///   ویرایش نقش 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update(RoleDto model)
        {
            model.UserId = UserId;
            var result = await roleService.Update(model);
            return Ok(result);
        }

        /// <summary>
        ///   حذف نقش 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await roleService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        ///  نمایش نقش ها به صورت درختواره
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetTree")]
        public async Task<IActionResult> GetTree(int? roleId = null)
        {
            var result = await roleService.GetRoleTree(roleId);
            return Ok(result);
        }



        /// <summary>
        ///  لیست همه دسترسی ها به همراه دسترسی های نقش جاری
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetRolePermissions/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            var result = await roleService.GetRolePermissions(roleId);
            return Ok(result);
        }

        /// <summary>
        ///  بروزرسانی دسترسی های یک نقش
        /// </summary>
        /// <returns></returns> 
        [HttpPost("UpdateRolePermissions/{roleId}")]
        public async Task<IActionResult> UpdateRolePermissions(int roleId, List<RolePermissionDto> permissions)
        {
            var result = await roleService.UpdateRolePermissions(roleId, permissions);
            return Ok(result);
        }
    }
}
