using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("UserLogSearchForProducts", Schema = "Catalog")]
    public class UserLogSearchForProduct
    {
        [Key]
        public Guid Id { get; set; }
        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }
        public string BrandName { get; set; }

        public string ProductName { get; set; }

        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        public DateTime CreateDate { get; set; }


    }
}
