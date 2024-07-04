using Infrastructure.Common;
using Infrastructure.Models.EIED;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IUserService
    {
        Task<ShopActionResult<List<UserDto>>> GetUsers(GridQueryModel model = null);
        Task<ShopActionResult<UserInputModel>> AddUser(UserInputModel model, Guid userId);
        Task<ShopActionResult<UserInputModel>> UpdateUser(UserInputModel model, Guid userId);
        Task<ShopActionResult<Guid>> DeleteUser(Guid userid);
        Task<ShopActionResult<UserDto>> GetUserById(Guid userid);
        Task<ShopActionResult<string>> GetEmailSignuature(Guid userid);
        Task<ShopActionResult<Guid>> UpdatePassword(ChangePasswordInputModel model, bool isCurrentUser = false);
        Task<ShopActionResult<bool>> ChangeEmailSignature(ChangeEmailSignatureInputModel model);
        Task<List<UserDto>> GetUsersInfoForCombo();
        Task<List<ComboItemDto>> GetUsersForCombo();
    }
}
