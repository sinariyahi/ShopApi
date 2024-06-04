using Infrastructure.Common;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IComboInfoService
    {
        Task<ShopActionResult<List<ComboItemDto>>> GetFeatureCategory();
        Task<List<ComboItemDto>> GetSmsType();
        Task<List<ComboItemDto>> GetRolesCombo();
        Task<ShopActionResult<List<ComboItemDto>>> GetJobOpportunities();
        Task<ShopActionResult<List<ComboItemDto>>> GetPagesLinkTypes();
        Task<ShopActionResult<List<ComboItemDto>>> GetVideoSource();
        Task<ShopActionResult<List<ComboItemDto>>> GetVideoCategory();
        Task<ShopActionResult<List<ComboItemDto>>> GetSymbolWithOutParent();
        Task<ShopActionResult<List<ComboItemDto>>> GetBrands();
        Task<ShopActionResult<List<ComboItemDto>>> GetUserTypes();
        Task<ShopActionResult<List<ComboItemDto>>> GetCities(Province provinceId);
        Task<ShopActionResult<List<ComboItemDto>>> GetArticleCategory();
        Task<ShopActionResult<List<ComboItemDto>>> GetOrganizationLevel();
        Task<ShopActionResult<ComboInfoDto>> GetComboInfo(Guid userId);
        Task<ShopActionResult<List<ComboItemDto>>> GetDisciplinesById(int id);
        Task<ShopActionResult<List<ComboItemDto>>> GetDisciplines(int? organizationId = null);
        Task<ShopActionResult<List<ComboItemDto>>> GetPositionPlace();
        Task<ShopActionResult<List<ComboItemDto>>> GetArticles(int categoryId);
        Task<ShopActionResult<List<ComboItemDto>>> GetVideos(int categoryId);
        Task<ShopActionResult<List<ComboItemDto>>> GetProducts(int categoryId);

        Task<ShopActionResult<List<ComboItemDto>>> GetProvince();
        Task<ShopActionResult<List<ComboItemDto>>> GetProductCategory();
        Task<ShopActionResult<List<ComboItemDto>>> GetOrderStatus();

        Task<ShopActionResult<List<ComboItemDto>>> GetDeliveryType();
        Task<ShopActionResult<List<ComboItemDto>>> GetCountType();
        Task<ShopActionResult<List<ComboItemDto>>> GetSaleStatus();
        Task<ShopActionResult<List<ComboItemDto>>> GetUserOpinionType();

        Task<ShopActionResult<List<ComboItemDto>>> GetShowStatus();
        Task<ShopActionResult<List<ComboItemDto>>> GetAllProductsForUser();
        Task<ShopActionResult<List<ComboItemDto>>> GetProductsByCategoryIdForUser(int categoryId);

    }
}
