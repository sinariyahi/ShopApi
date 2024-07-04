using Infrastructure.Common;
using Infrastructure.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IUserGroupMemberService
    {
        Task<ShopActionResult<List<UserGroupMemberDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<Guid>> Add(UserGroupMemberDto model);
        Task<ShopActionResult<Guid>> Update(UserGroupMemberDto model);
        Task<ShopActionResult<Guid>> Delete(Guid id);
        Task<ShopActionResult<UserGroupMemberDto>> GetById(Guid id);
    }
}
