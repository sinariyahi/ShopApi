using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface ICityService
    {

        Task<ShopActionResult<List<CityDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(CityInputModel model);
        Task<ShopActionResult<int>> Update(CityInputModel model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<CityDto>> GetById(int id);
    }
}
