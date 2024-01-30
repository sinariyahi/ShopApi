using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("ProductUsages", Schema = "Catalog")]
    public class ProductUsage
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
