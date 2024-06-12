using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface IBrandService
    {
        Task<ShopActionResult<List<BrandDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<List<UserBrandDto>>> GetUserList(int count = 8);
        Task<ShopActionResult<List<TreeDto>>> GetBrandMegaMenu();
        Task<ShopActionResult<int>> Add(BrandInputModel model);
        Task<ShopActionResult<int>> Update(BrandInputModel model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<BrandDto>> GetById(int id);
        Task<ShopActionResult<List<UserBrandDto>>> GetUserListByProductCategoryId(string category);
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserProductCategoryByBrandId(int brandId);
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserProductCategoryByBrandTitle(string brand);
        Task<ShopActionResult<List<TreeDto>>> GetAllBrandMegaMenu();
        Task<ShopActionResult<List<BrandForDashboardChart>>> GetBrandProductsForDashboard();
    }
}
