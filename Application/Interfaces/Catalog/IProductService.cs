using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.EIED;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface IProductService
    {
        #region Admin - Product
        Task<ShopActionResult<List<UserLogSearchForProductDto>>> GetListUserLogSearchForProduct(GridQueryModel model = null);
        Task<ShopActionResult<List<ProductDto>>> GetList(GridQueryModel model = null);
        Task<byte[]> GetListForExcel(GridQueryModel model = null, string fileName = null);
        Task<ShopActionResult<int>> ImportPriceListWithExcel(IFormFile file, Guid userId);
        Task<ShopActionResult<List<ComboItemDto>>> GetListWithCategory(int categoryId);
        Task<ShopActionResult<int>> Add(ProductDto model);
        Task<ShopActionResult<int>> Update(ProductDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ProductDto>> GetById(int id);
        Task<byte[]> GetListUserLogSearchForExcel(GridQueryModel model = null, string fileName = null);
        #endregion

        #region  Product - Attachment
        Task<ShopActionResult<bool>> DeleteAttachment(Guid attachmentId);
        Task<ShopActionResult<ProductAttachmentDto>> GetProductAttachment(int id);
        Task<ShopActionResult<int>> UpdateProductAttachment(ProductAttachmentInputDto model);
        #endregion

        #region Article - Product
        Task<ShopActionResult<ArticleProductInputDto>> GetArticleProductById(int id);
        Task<ShopActionResult<int>> AddArticleProduct(ArticleProductInputDto model);
        #endregion


        #region Video - Product
        Task<ShopActionResult<VideoProductInputDto>> GetVideoProductById(int id);
        Task<ShopActionResult<int>> AddVideoProduct(VideoProductInputDto model);
        #endregion

        #region Similar - Product
        Task<ShopActionResult<SimilarProductInputDto>> GetSimilarProductById(int id);
        Task<ShopActionResult<int>> AddSimilarProduct(SimilarProductInputDto model);
        #endregion

        #region Delivery - Product
        Task<ShopActionResult<int>> UpdateDeliveryProduct(DeliveryProductDto model);
        Task<ShopActionResult<List<DeliveryProductDto>>> GetDeliveryProduct(DeliveryProductFilterDto model = null);
        Task<ShopActionResult<int>> AddDeliveryProduct(DeliveryProductDto model);
        Task<ShopActionResult<DeliveryProductDto>> GetDeliveryProductById(int id);
        Task<ShopActionResult<int>> DeleteDeliveryProduct(int id);
        #endregion


        #region Financial - Product
        Task<ShopActionResult<int>> UpdateFinancialProduct(FinancialProductDto model);
        Task<ShopActionResult<FinancialProductDto>> GetFinancialProductById(int id);
        Task<ShopActionResult<List<FinancialProductDto>>> GetFinancialProductList(int productId);
        #endregion


        #region User - Product
        Task<ShopActionResult<UserProductDetailsDto>> GetByTitle(string title);
        Task<ShopActionResult<List<UserProductDto>>> GetTopVisitedProductList(int count = 6);
        Task<ShopActionResult<List<UserProductDto>>> GetTopSaleProductList(int count = 6);
        Task<ShopActionResult<List<UserProductDto>>> GetTopNewProductList(int count = 6);
        Task<ShopActionResult<List<UserProductDto>>> GetSpecialOfferProductList(int count = 6);

        Task<ShopActionResult<UserSimilarProductDto>> GetUserSimilarProductByTitle(string title);

        Task<ShopActionResult<List<UserProductDto>>> GetUserList(GridQueryModel model = null);
        Task<ShopActionResult<List<string>>> GetAllTitle();

        Task<ShopActionResult<UserProductDetailsDto>> GetProductDetail(int id);
        Task<ShopActionResult<SimilarDto>> GetSimilarData(int id);
        Task<ShopActionResult<List<UserProductDto>>> GetProductsForParentItemsMegaMenu(GridQueryModel model = null);
        Task<ShopActionResult<int>> GetSpecialOfferProductCount();
        Task<ShopActionResult<int>> AddFavoriteProduct(FavoriteProductModel model, Guid userId);

        Task<ShopActionResult<List<FavoriteProductDto>>> GetAllFavoriteProduct(Guid userId);
        Task<ShopActionResult<bool>> GetByIdFavoriteProduct(Guid userId, int productId);
        #endregion

    }
}
