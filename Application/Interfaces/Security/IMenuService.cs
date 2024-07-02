using Infrastructure.Common;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IMenuService
    {
        Task<ShopActionResult<List<MenuDto>>> GetMenu();
        Task<ShopActionResult<List<MenuDto>>> GetMenuAccessForUser(Guid UserId);
    }
}
