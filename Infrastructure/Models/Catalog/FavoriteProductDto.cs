using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class FavoriteProductDto
    {
        public int ProductId { get; set; }
        public Guid Id { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string ProductName { get; set; }
        public string EnTitle { get; set; }

        public string Code { get; set; }
        public string Remark { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool IsActive { get; set; } = true;
        public string IsActiveTitle { get; set; }
        public string ShortDescription { get; set; }
        public ProductAttachmentType ProductAttachmentType { get; set; }
//        public List<CategoryFeatureDto> ProductFeatureValues { get; set; } = new List<CategoryFeatureDto>();
//        public List<ProductUsageDto> ProductUsages { get; set; } = new List<ProductUsageDto>();
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


    }

    public class FavoriteProductModel
    {
        public bool IsSelected { get; set; }
        public int ProductId { get; set; }

    }

}
