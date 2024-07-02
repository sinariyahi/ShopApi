using Infrastructure.Common;
using Infrastructure.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IAuthenticationService
    {
        Task<ShopActionResult<bool>> ChangePasswordForUserAsync(ResetPasswordDto model, string ip, string browser);
        Task<ShopActionResult<Guid>> ResetPasswordConfirmation(Guid code, string ip, string browser);
        Task<ShopActionResult<bool>> ConfirmationAsync(Guid code, string ip, string browser);
        Task<(ShopActionResult<bool>, Guid?)> RegisterAsync(RegisterDto model, string ip, string browser);
        Task<ShopActionResult<bool>> ChangePasswordAsync(ResetPasswordDto model, string ip, string browser);
        Task<ShopActionResult<AuthenticateModel>> LoginAsync(LoginDto model, string ip, string browser);
        Task<ShopActionResult<AuthenticateModel>> GenerateTokenWithRefreshTokenAsync(string token, string currentRefreshToken, string ip, string browser);
        Task<ShopActionResult<bool>> SendResetPasswordLink(SendResetPasswordLinkDto model, string ip, string browser);
        Task<ShopActionResult<bool>> ResetPassword(ResetPasswordDto model, string ip, string browser);
        Task<ShopActionResult<UserAuthenticateModel>> CustomerLoginAsync(LoginDto model, string ip, string browser);
        Task<ShopActionResult<bool>> CheckUserIsExsits(string phoneNumber);
        Task<ShopActionResult<bool>> CheckUserIsExsitsForActive(string phoneNumber);
    }
}
