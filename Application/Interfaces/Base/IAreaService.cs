using Infrastructure.Common;
using Infrastructure.Models.OrganizationUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IAreaService
    {
        Task<ShopActionResult<List<AreaDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(AreaDto model);
        Task<ShopActionResult<int>> Update(AreaDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<AreaDto>> GetById(int id);
    }
}
