using Infrastructure.Common;
using Infrastructure.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IUserGroupService
    {
        Task<ShopActionResult<UserAccessForUserGroupDto>> GetUserActionsAccessForUserGroup(Guid userId);

        Task<ShopActionResult<List<UserGroupDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<Guid>> Add(UserGroupDto model);
        Task<ShopActionResult<Guid>> Update(UserGroupDto model);
        Task<ShopActionResult<Guid>> Delete(Guid id);
        Task<ShopActionResult<UserGroupDto>> GetById(Guid id);
    }
}
