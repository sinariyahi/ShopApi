using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Authorization
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class UserIsExsitsDto
    {
        public string PhoneNumber { get; set; }
        public string Captcha { get; set; }
        public string CaptchaKey { get; set; }
    }

    public class ChangePasswordWithResetLinkDto
    {
        public Guid Code { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Guid UserId { get; set; }

        public Guid? ResetCode { get; set; }
    }

    public class SendResetPasswordLinkDto
    {
        [EmailAddress]
        public string UserName { get; set; }
    }
}
