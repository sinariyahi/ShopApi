using Infrastructure.Common;
using Infrastructure.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Media
{
    public interface IBannerService
    {
        Task<ShopActionResult<List<BannerDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(BannerDto model);
        Task<ShopActionResult<int>> Update(BannerDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<BannerDto>> GetById(int id);
        Task<ShopActionResult<UserBannerDto>> GetUserList(PositionPlace positionPlace);
    }
}
