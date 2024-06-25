using Infrastructure.Common;
using Infrastructure.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Media
{
    public interface IVideoService
    {
        Task<ShopActionResult<List<VideoDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(VideoDto model);
        Task<ShopActionResult<int>> Update(VideoDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<VideoDto>> GetById(int id);

        Task<ShopActionResult<List<UserVideoDto>>> GetLastVideo();
        Task<ShopActionResult<List<UserVideoDto>>> GetUserList(GridQueryModel model = null);
        Task<ShopActionResult<List<int>>> GetAllId();
    }
}
