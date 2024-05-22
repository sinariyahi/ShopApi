using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Security;

namespace Domain.Entities.Catalog
{
    [Table("FavoriteProducts", Schema = "Catalog")]
    public class FavoriteProduct
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
