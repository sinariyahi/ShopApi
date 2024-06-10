using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface ISocialMediaService
    {
        Task<ShopActionResult<List<SocialMediaDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<List<UserSocialMediaDto>>> GetUserList(int count = 8);
        Task<ShopActionResult<int>> Add(SocialMediaInputModel model);
        Task<ShopActionResult<int>> Update(SocialMediaInputModel model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<SocialMediaDto>> GetById(int id);
    }
}
