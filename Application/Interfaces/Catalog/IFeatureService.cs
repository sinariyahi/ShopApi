using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface IFeatureService
    {
        Task<ShopActionResult<List<FeatureDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(FeatureDto model);
        Task<ShopActionResult<int>> Update(FeatureDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<FeatureDto>> GetById(int id);
        Task<ShopActionResult<List<FeatureDto>>> GetByFeaturesCategoryId(int? id);
    }
}
