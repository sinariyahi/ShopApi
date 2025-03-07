using Application.Interfaces.Security;
using Infrastructure.Common;
using Infrastructure.Models.Authorization;
using Infrastructure.Models.Customer;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using XAct.Messages;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService customerService;
        private readonly SMSUtility smsUtility;
        private readonly Configs configs;
        readonly IMemoryCache cache;
        public AuthenticateController(IAuthenticationService authenticationService, IOptions<Configs> options, IMemoryCache cache, ICustomerService customerService, SMSUtility smsUtility)
        {
            _authenticationService = authenticationService;
            this.configs = options.Value;
            this.cache = cache;
            this.customerService = customerService;
            this.smsUtility = smsUtility;
        }

        [HttpPost("CheckUserIsExsits")]
        public async Task<IActionResult> CheckUserIsExsits(UserIsExsitsDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();
            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
             //   captchaResult.Message = MessagesFA.CaptchaInvalid;
                return Ok(captchaResult);
            }
            var result = await _authenticationService.CheckUserIsExsits(model.PhoneNumber);
            return Ok(result);
        }

        [HttpPost("CheckUserIsExsitsForActive")]
        public async Task<IActionResult> CheckUserIsExsitsForActive(UserIsExsitsDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();
            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
              //  captchaResult.Message = MessagesFA.CaptchaInvalid;
                return Ok(captchaResult);
            }
            var result = await _authenticationService.CheckUserIsExsitsForActive(model.PhoneNumber);
            return Ok(result);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCustomerInputModel model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
                captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }

            var result = await customerService.RegisterCustomer(model);
            if (result.IsSuccess == true)
            {
                var loginModel = new LoginDto()
                {
                    Password = model.Password,
                    UserName = model.UserName

                };
                var data = await _authenticationService.CustomerLoginAsync(loginModel, ip, browser);
                return Ok(data);
            }
            return Ok(result);
        }




        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ResetPasswordDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.ChangePasswordAsync(model, ip, browser);
            return Ok(result);
        }

        [HttpPost("ChangePasswordForUser")]
        public async Task<IActionResult> ChangePasswordForUser(ResetPasswordDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();
            var result = await _authenticationService.ChangePasswordForUserAsync(model, ip, browser);
            return Ok(result);
        }


        [HttpPost("SendResetPasswordLink")]
        public async Task<IActionResult> SendResetPasswordLink(SendResetPasswordLinkDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.SendResetPasswordLink(model, ip, browser);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.ResetPassword(model, ip, browser);
            return Ok(result);
        }

        [HttpPost("Confirmation/{code}")]
        public async Task<IActionResult> Confirmation(Guid code)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.ConfirmationAsync(code, ip, browser);
            return Ok(result);
        }

        [HttpPost("ResetPasswordConfirmation/{code}")]
        public async Task<IActionResult> ResetPasswordConfirmation(Guid code)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.ResetPasswordConfirmation(code, ip, browser);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDto model)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
                captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }

            var result = await _authenticationService.LoginAsync(model, ip, browser);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();

            var browser = Request.Headers["User-Agent"].ToString();

            var captchaValueFromCache = cache.Get<string>(model.CaptchaKey);
            if (captchaValueFromCache == null || captchaValueFromCache != model.Captcha)
            {
                var captchaResult = new ShopActionResult<string>();
                captchaResult.IsSuccess = false;
                captchaResult.Message = Messages.CaptchaInvalid;
                return Ok(captchaResult);
            }
            var result = await _authenticationService.CustomerLoginAsync(model, ip, browser);
            return Ok(result);
        }



        [HttpPost("NewToken")]
        public async Task<IActionResult> RefreshToken(LoginWithRefereshTokenDto model)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var browser = Request.Headers["User-Agent"].ToString();

            var result = await _authenticationService.GenerateTokenWithRefreshTokenAsync(model.Token, model.RefreshToken, ip, browser);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Unauthorized(result.Message);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Id = 1, Title = "test call api" });
        }

        [HttpGet("SendSMS")]
        public async Task<IActionResult> SendSMS(string? mobile = "09362322511")
        {
            var result = await smsUtility.Send(mobile, "سلام ... سینا ریاحی هستم");
            return Ok(new { ResultCode = result });
        }
    }
}
