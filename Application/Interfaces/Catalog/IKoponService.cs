using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface IKoponService
    {
        Task<ShopActionResult<List<KoponDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(KoponDto model);
        Task<ShopActionResult<int>> Update(KoponDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<KoponDto>> GetById(int id);
        Task<ShopActionResult<UserKoponDto>> GetByCode(string code);
    }
}
