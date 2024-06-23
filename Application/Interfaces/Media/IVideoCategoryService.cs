using Infrastructure.Common;
using Infrastructure.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Media
{
    public interface IVideoCategoryService
    {
        Task<ShopActionResult<List<VideoCategoryDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(VideoCategoryDto model);
        Task<ShopActionResult<int>> Update(VideoCategoryDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<VideoCategoryDto>> GetById(int id);
        Task<ShopActionResult<List<VideoCategoryDto>>> GetUserList();
    }
}
