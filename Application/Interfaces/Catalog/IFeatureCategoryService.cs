using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface IFeatureCategoryService
    {
        Task<ShopActionResult<List<FeatureCategoryDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(FeatureCategoryDto model);
        Task<ShopActionResult<int>> Update(FeatureCategoryDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<FeatureCategoryDto>> GetById(int id);
    }
}
