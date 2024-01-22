using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("FinancialProducts", Schema = "Catalog")]
    public class FinancialProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProductId { get; set; }
     //   public virtual Product Product { get; set; }
        public string Remark { get; set; }

        public long Price { get; set; }
        public long DiscountedPrice { get; set; } = 0;
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public bool GetInventoryFromApi { get; set; } = false;
    }
}
