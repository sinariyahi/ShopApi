using Application.Interfaces.Security;
using Infrastructure.Common;
using Infrastructure.Models.Security;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Security
{
    public class UserGroupController : BaseController
    {
        private readonly IUserGroupService service;
        public UserGroupController(IUserGroupService service)
        {
            this.service = service;
        }

        /// <summary>
        ///  لیست گروه کاربران  
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await service.GetList(model);
            return Ok(result);
        }


        /// <summary>
        ///User  یک  Actions, DisciplineIds , UserGroup برگرداندن
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserActionsAccessForUserGroup")]
        public async Task<IActionResult> GetUserActionsAccessForUserGroup()
        {
            var result = await service.GetUserActionsAccessForUserGroup(UserId);
            return Ok(result);
        }






        /// <summary>
        ///   برگرداندن یک گروه کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await service.GetById(id);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت گروه کاربر جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(UserGroupDto model)
        {
            var result = await service.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش گروه کاربر 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserGroupDto model)
        {
            var result = await service.Update(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف گروه کاربر 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await service.Delete(id);
            return Ok(result);
        }
    }
}
