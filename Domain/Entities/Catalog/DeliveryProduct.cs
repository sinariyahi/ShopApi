using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Catalog
{
    [Table("DeliveryProducts", Schema = "Catalog")]
    public class DeliveryProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Remark { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public CountType CountType { get; set; }

        public Province Province { get; set; }
        public long Cost { get; set; }
        public int? Count { get; set; }

        public int? SmallerEqual { get; set; }
        public int? GreaterEqual { get; set; }

        public bool NeedToCall { get; set; } = false;
    }
}
