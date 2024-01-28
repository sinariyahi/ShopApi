using Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("Products", Schema = "Catalog")]
    public class Product
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string ProductName { get; set; }
        [Required, MaxLength(256)]
        public string Code { get; set; }


        public string EnName { get; set; }

        public string Remark { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
     //   public SaleStatus? SaleStatus { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int BrandId { get; set; }
        public int? Inventory { get; set; }
        public bool GetInventoryFromApi { get; set; } = false;
        public string ShortDescription { get; set; }
        public bool IsTopVisited { get; set; } = false;
        public double VisitedCount { get; set; } = 0;
        public double Weight { get; set; } = 0;

        public bool IsTopSale { get; set; } = false;
        public double SaleCount { get; set; } = 0;

        public bool IsTopNew { get; set; } = false;
        public bool IsSpecialOffer { get; set; } = false;
        public long? APIAmount { get; set; }
        public int? APIQuantity { get; set; }

        public virtual Brand Brand { get; set; }
       // public virtual ICollection<ProductFeatureValue> FeatureValues { get; set; }
        //public virtual ICollection<ProductUsage> ProductUsages { get; set; }
        //public virtual ICollection<ProductAttachment> ProductAttachments { get; set; }
       // public virtual ICollection<ProductCoverAttachment> ProductCoverAttachments { get; set; }
        public virtual ICollection<FinancialProduct> FinancialProducts { get; set; }
        //public virtual ICollection<VideoProduct> VideoProducts { get; set; }
        public virtual ICollection<ArticleProduct> ArticleProducts { get; set; }
        public DateTime? UpdateApiDate { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; } = true;

        public Product()
        {
            //FeatureValues = new HashSet<ProductFeatureValue>();
            //ProductUsages = new HashSet<ProductUsage>();
            //ProductAttachments = new HashSet<ProductAttachment>();
            //ProductCoverAttachments = new HashSet<ProductCoverAttachment>();
            //FinancialProducts = new HashSet<FinancialProduct>();
            //ArticleProducts = new HashSet<ArticleProduct>();
            //VideoProducts = new HashSet<VideoProduct>();

        }
    }
}
