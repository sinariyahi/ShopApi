using Infrastructure.Common;
using Infrastructure.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Media
{
    public interface ISliderService
    {
        Task<ShopActionResult<List<SliderDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(SliderDto model);
        Task<ShopActionResult<int>> Update(SliderDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<SliderDto>> GetById(int id);
    }
}
