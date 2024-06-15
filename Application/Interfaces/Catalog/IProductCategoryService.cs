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
    public interface IProductCategoryService
    {
        Task<ShopActionResult<List<ProductCategoryDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(ProductCategoryDto model);
        Task<ShopActionResult<int>> Update(ProductCategoryDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ProductCategoryDto>> GetById(int id);
        Task<ShopActionResult<List<TreeDto>>> GetTree(int? parentId = null);
        Task<ShopActionResult<List<FeatureDto>>> GetFeatures(int categoryId);
        Task<ShopActionResult<List<TreeDto>>> Search(string text, string code);
        Task<ShopActionResult<List<FeatureDto>>> GetFeaturesForSearch(int categoryId);
        Task<ShopActionResult<ProductCategoryForDashboardDto>> GetProductCategoryForDashboard();
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserList(int count = 8);
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserByParentId(int count = 8, string category = "");
        Task<ShopActionResult<List<TreeDto>>> GetCategoriesForMegaMenu();
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserAllList();
        Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserById(int id = 1);
        Task<ShopActionResult<List<UserFeatureDto>>> GetFeaturesByCategoryId(string category, string brand);
        Task<ShopActionResult<List<UserFeatureDto>>> GetFeatureOptionByCategoryId(int categoryId);

        Task<ShopActionResult<List<TreeDto>>> GetCategoryByParentId(string category);
        Task<ShopActionResult<List<FeatureProductDto>>> GetFeatureOptionByCategory(string category);
        Task<List<TreeDto>> GetCategoryWithBrands(string category);
        Task<ShopActionResult<FeatureDto>> GetFeaturesByTitle(string category);

        Task<ShopActionResult<List<TreeDto>>> GetSubCategoryByCategoryAndBrand(string category, string brand);
        Task<ShopActionResult<List<UserFeatureDto>>> GetFeaturesBySubCategoryId(string category, string brand, string subCategory);
    }
}
