using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class UserLogSearchForProductDto
    {

        public Guid Id { get; set; }
        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }
        public string BrandName { get; set; }

        public string ProductName { get; set; }

        public string CreateDate { get; set; }


    }
    public class ProductExportToExcelDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public int? MainFeatureId { get; set; }
        public string EnName { get; set; }
        public string IsActiveTitle { get; set; }
        public string BrandName { get; set; }
        public string SaleStatusTitle { get; set; }

    }
    public class ProductDto
    {
        public long? APIAmount { get; set; }
        public int? APIQuantity { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int? MainFeatureId { get; set; }
        public string EnName { get; set; }

        public double Weight { get; set; }
        public string BrandName { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public string IsActiveTitle { get; set; }
        public ProductAttachmentType ProductAttachmentType { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }
        public int? Inventory { get; set; }
        public bool GetInventoryFromApi { get; set; }

        public SaleStatus? SaleStatus { get; set; }
        public string SaleStatusTitle { get; set; }
        public bool IsTopVisited { get; set; } = false;
        public double VisitedCount { get; set; } = 0;

        public bool IsTopSale { get; set; } = false;
        public bool IsSpecialOffer { get; set; } = false;

        public double SaleCount { get; set; } = 0;
        public string CoverFile { get; set; }
        public bool IsTopNew { get; set; } = false;
        public string CategoryName { get; set; }
        public Guid UserId { get; set; }
        public List<CategoryFeatureDto> FeatureValues { get; set; } = new List<CategoryFeatureDto>();
        public List<ProductUsageDto> ProductUsages { get; set; } = new List<ProductUsageDto>();
        public List<IFormFile> File { get; set; }
        public List<CategoryFeatureDto> ProductFeatureValues { get; set; } = new List<CategoryFeatureDto>();
        public List<string> ProductAttachments { get; set; }

        //public List<ProductAttachmentDto> ProductAttachments { get; set; } = new List<ProductAttachmentDto>();
    }
    public class UserProductDetailsDto
    {
        public int Id { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int? MainFeatureId { get; set; }
        public string EnTitle { get; set; }

        public bool IsActive { get; set; } = true;
        public string IsActiveTitle { get; set; }
        public string ShortDescription { get; set; }
        public ProductAttachmentType ProductAttachmentType { get; set; }
        public List<CategoryFeatureDto> ProductFeatureValues { get; set; } = new List<CategoryFeatureDto>();
        public List<ProductUsageDto> ProductUsages { get; set; } = new List<ProductUsageDto>();
        public List<string> OtherFileAttachments { get; set; }
        public List<string> OrginalFileAttachments { get; set; }
        public List<string> CatalogueAndBrochureFileAttachments { get; set; }

        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }
        public int? Inventory { get; set; }
        public bool GetInventoryFromApi { get; set; }

        public SaleStatus? SaleStatus { get; set; }
        public string SaleStatusTitle { get; set; }

        public string CoverFile { get; set; }
        public string CategoryName { get; set; }
        public SimilarDto SimilarModel { get; set; }

    }
    public class UserProductDto
    {
        public bool IsTopVisited { get; set; }
        public double VisitedCount { get; set; }
        public string BrandName { get; set; }
        public string EnTitle { get; set; }
        public int? Inventory { get; set; }
        public bool IsTopSale { get; set; }
        public double SaleCount { get; set; }
        public bool IsSpecialOffer { get; set; }

        public bool IsTopNew { get; set; }
        public List<string> ProductAttachments { get; set; }
        public string CoverFile { get; set; }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public SaleStatus SaleStatus { get; set; }

        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }
    }


    public class DeliveryProductFilterDto : GridQueryModel
    {

        public int ProductId { get; set; }
        public Province? Province { get; set; }
        public DeliveryType? DeliveryType { get; set; }

    }

    public class DeliveryProductInputDto
    {

        public int ProductId { get; set; }

        public List<DeliveryProductDto> Items { get; set; }
    }


    public class DeliveryProductDto
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string DeliveryTypeTitle { get; set; }
        public int ProductId { get; set; }
        public CountType CountType { get; set; }
        public Province Province { get; set; }
        public string ProvinceTitle { get; set; }

        public long Cost { get; set; }
        public int? Count { get; set; }

        public int? SmallerEqual { get; set; }
        public int? GreaterEqual { get; set; }

        public bool NeedToCall { get; set; } = false;
    }

    public class FinancialProductDto
    {
        public int ProductId { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public bool GetInventoryFromApi { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }
        public string CreateDate { get; set; }
    }


    public class FinancialProductFilterDto : GridQueryModel
    {

        public int ProductId { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
    }

    public class ProductAttachmentDto
    {
        public int ProductId { get; set; }
        public FileItemDto CoverAttachment { get; set; }

        public List<ProductFileAttachmentDto> ListProductAttachment { get; set; }
        public List<VideoProductAttachmentDto> ListVideoProductAttachmentModel { get; set; }

    }

    public class ProductAttachmentInputDto
    {
        public int ProductId { get; set; }
        public List<VideoProductAttachmentDto> ListVideoProductAttachmentModel { get; set; }
        public List<IFormFile> CatalogFiles { get; set; }
        public List<IFormFile> CoverFiles { get; set; }

        public List<IFormFile> OtherFiles { get; set; }
        public List<IFormFile> OrginalFiles { get; set; }

    }


    public class ArticleProductInputDto
    {
        public int ProductId { get; set; }
        public List<ArticleSelectedDto> Articles { get; set; }
        public List<int> ArticleSelected { get; set; }

    }

    public class ArticleSelectedDto
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }

    }


    public class VideoProductInputDto
    {
        public int ProductId { get; set; }
        public List<VideoSelectedDto> Videos { get; set; }
        public List<int> VideoSelected { get; set; }

    }

    public class VideoSelectedDto
    {
        public int VideoId { get; set; }
        public string VideoTitle { get; set; }

    }


    public class SimilarProductInputDto
    {
        public int ProductId { get; set; }
        public string Remark { get; set; }
        public List<SimilarProductSelectedDto> SimilarProducts { get; set; }
        public List<int> ProductSelected { get; set; }

    }


    public class UserSimilarProductDto
    {
        public int Id { get; set; }
        public List<SimilarProductSelectedDto> SimilarProducts { get; set; }


    }


    public class SimilarDto
    {

        public SimilarProductModel SimilarArtcle { get; set; }
        public SimilarProductModel SimilarVideo { get; set; }
        //public List<SimilarProductModel> SimilarArtcle { get; set; }
        //public List<SimilarProductModel> SimilarVideo { get; set; }

    }


    public class SimilarProductModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> File { get; set; }
        public string Remark { get; set; }
        public string CoverFile { get; set; }


    }



    public class SimilarProductSelectedDto
    {
        public string EnTitle { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string CoverFile { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }

    }



    public class ProductFileAttachmentDto
    {
        public string ProductAttachmentTypeTitle { get; set; }

        public ProductAttachmentType ProductAttachmentType { get; set; }
        public List<IFormFile> File { get; set; }
        public List<FileItemDto> FileAttachments { get; set; }

    }

    public class VideoProductAttachmentDto
    {
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string VideoLink { get; set; }

        public List<IFormFile> File { get; set; }
        public List<FileItemDto> FileAttachments { get; set; }

    }


    public class ProductInfoDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
        public int ProductCategoryId { get; set; }
        public List<string> File { get; set; }

    }


    public class ProductGroupCategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<ProductInfoDto> Children { get; set; }
    }



    //public class ProductAttachmentDto
    //{
    //    public Guid Id { get; set; }
    //    public string Title { get; set; }
    //    public string FileName { get; set; }
    //    public string DownloadUrl { get; set; }
    //    public IFormFile File { get; set; }
    //    public byte[] FileContent { get; set; }
    //    public string FileBase64 { get; set; }
    //}

    public class CategoryFeatureDto
    {
        public int Id { get; set; }
        public int ProductCategoryFeatureId { get; set; }
        public string Title { get; set; }
        public int? SortOrder { get; set; }

        public string Value { get; set; }
        public string FeatureValue { get; set; }
        public int? FeatureValueNumber { get; set; }
        public string UnitTypeTitle { get; set; }
        public UnitType UnitType { get; set; }
        public ControlType ControlType { get; set; }


        public int? Max { get; set; }
        public int? Min { get; set; }
        public int? FeatureCategoryId { get; set; }
        public int? FeatureId { get; set; }

        public int? SymbolId { get; set; }
        public string SymbolTitle { get; set; }
        public string Option { get; set; }

    }

    public class ProductUsageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProductId { get; set; }
    }
}
