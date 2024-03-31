using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Authorization
{
    public class AuthenticateModel
    {
        public string LastLoginDate { get; set; }

        public bool? IsCompany { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public int RefreshTokenTimeout { get; set; }
        public int TokenTimeout { get; set; }
        public UserInfoDto User { get; set; } = new UserInfoDto();
    }

    public class UserAuthenticateModel
    {
        public string LastLoginDate { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public int RefreshTokenTimeout { get; set; }
        public int TokenTimeout { get; set; }
        public InfoDto User { get; set; } = new InfoDto();
    }





    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public string CaptchaKey { get; set; }
    }

    public class LoginWithRefereshTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserInfoDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public List<UserPermissionDto> Permissions { get; set; }
    }


    public class InfoDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
    }


    public class UserPermissionDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string EnTitle { get; set; }
        public string Icon { get; set; }
        public int? SortOrder { get; set; }
        public List<UserPermissionDto> Children { get; set; }
    }
}
