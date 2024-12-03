using Application.Interfaces.Sms;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Sms
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSmsController : ControllerBase
    {
        private readonly ISmsService service;

        public UserSmsController(ISmsService service)
        {
            this.service = service;

        }


        /// <summary>
        ///     ارسال پیامک برای کاربر برای ثبت نام شماره موبایل کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("SendSmsForRegisterUser/{phoneNumber}")]
        public async Task<IActionResult> SendSmsForRegisterUser(string phoneNumber)
        {
            var result = await service.SendSmsForRegisterUser(phoneNumber);
            return Ok(result);
        }


        /// <summary>
        ///      چک کردن کد ارسالی کاربر برای ثبت نام کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckCodeSmSForRegisterUser/{code}/{phoneNumber}")]
        public async Task<IActionResult> CheckCodeSmSForRegisterUser(string code, string phoneNumber)
        {
            var result = await service.CheckCodeSmSForRegisterUser(code, phoneNumber);
            return Ok(result);
        }

        /// <summary>
        ///      چک کردن کد ارسالی کاربر برای  فعال سازی کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckCodeSmSForActiveUser/{code}/{phoneNumber}")]
        public async Task<IActionResult> CheckCodeSmSForActiveUser(string code, string phoneNumber)
        {
            var result = await service.CheckCodeSmSForActiveUser(code, phoneNumber);
            return Ok(result);
        }
        /// <summary>
        ///      چک کردن کد ارسالی کاربر برای ثبت نام کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckCodeSmSForForgetPasswordUser/{code}/{userName}")]
        public async Task<IActionResult> CheckCodeSmSForForgetPasswordUser(string code, string userName)
        {
            var result = await service.CheckCodeSmSForForgetPasswordUser(code, userName);
            return Ok(result);
        }



        /// <summary>
        ///         ارسال پیامک  برای فراموشی رمز عبور کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("SendSmsForgetPassword/{userName}")]
        public async Task<IActionResult> SendSmsForgetPassword(string userName)
        {
            var result = await service.SendSmsForgetPassword(userName);
            return Ok(result);
        }


    }
}
