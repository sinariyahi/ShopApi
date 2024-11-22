using Application.Interfaces.Security;
using Infrastructure.Common;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Security
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        ///  لیست کاربران برای کومبو
        /// </summary>
        /// <returns></returns> 
        [HttpPost("GetListForCombo")]
        public async Task<IActionResult> GetListForCombo()
        {
            var result = await userService.GetUsersForCombo();
            return Ok(result);
        }

        /// <summary>
        ///  لیست کاربران برای کومبو
        /// </summary>
        /// <returns></returns> 
        [HttpPost("GetUsersInfoForCombo")]
        public async Task<IActionResult> GetUsersInfoForCombo()
        {
            var result = await userService.GetUsersInfoForCombo();
            return Ok(result);
        }

        /// <summary>
        ///  لیست کاربران 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser(GridQueryModel model = null)
        {
        //    if (UserType == UserType)
        //    {
                var result = await userService.GetUsers(model);
                return Ok(result);
        //    }
        //    else { return BadRequest(); }

        }

        /// <summary>
        ///   برگرداندن یک کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await userService.GetUserById(userId);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن اطلاعات کاربر جاری 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await userService.GetUserById(UserId);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن امضای ایمیل کاربر جاری 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmailSignuature")]
        public async Task<IActionResult> GetEmailSignuature()
        {
            var result = await userService.GetEmailSignuature(UserId);
            return Ok(result);
        }

        /// <summary>
        ///     ثبت کاربر جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserInputModel model)
        {
        //    if (UserType == UserType)
        //    {
                var result = await userService.AddUser(model, UserId);
                return Ok(result);
        //    }
        //    else { return BadRequest(); }

        }

        /// <summary>
        ///   ویرایش کاربر 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserInputModel model)
        {
        //    if (UserType == UserType)
        //    {
                var result = await userService.UpdateUser(model, UserId);
                return Ok(result);
        //    }
        //    else { return BadRequest(); }

        }

        /// <summary>
        ///   حذف کاربر 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
     //       if (UserType == UserType)
     //       {
                var result = await userService.DeleteUser(userId);
                return Ok(result);
     //       }
     //       else { return BadRequest(); }

        }

        /// <summary>
        ///  ویرایش رمز عبور کاربر مورد نظر
        /// </summary>
        /// <returns></returns> 
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(ChangePasswordInputModel model)
        {
            var result = await userService.UpdatePassword(model);
            return Ok(result);
        }

        /// <summary>
        ///  ویرایش امضای ایمیل کاربر
        /// </summary>
        /// <returns></returns> 
        [HttpPost("ChangeEmailSignature")]
        public async Task<IActionResult> ChangeEmailSignature(ChangeEmailSignatureInputModel model)
        {
            model.UserId = UserId;
            var result = await userService.ChangeEmailSignature(model);
            return Ok(result);
        }

        /// <summary>
        ///  ویرایش رمز عبور کاربر جاری
        /// </summary>
        /// <returns></returns> 
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            model.UserId = UserId;
            var result = await userService.UpdatePassword(model, true);
            return Ok(result);
        }
    }
}
