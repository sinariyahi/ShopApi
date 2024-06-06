using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IOriginService
    {
        Task<ShopActionResult<List<OriginDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(OriginDto model);
        Task<ShopActionResult<int>> Update(OriginDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<OriginDto>> GetById(int id);
    }
}
