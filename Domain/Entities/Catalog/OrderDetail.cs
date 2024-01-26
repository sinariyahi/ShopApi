using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("OrderDetails", Schema = "Catalog")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
 //       public virtual Product Product { get; set; }

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
        public long ItemCount { get; set; }
        public long Price { get; set; }

    }
}
