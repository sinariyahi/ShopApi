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
    [Table("ProductUserComments", Schema = "Catalog")]
    public class ProductUserComment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid? UserId { get; set; }
       public virtual User User { get; set; }
        [MaxLength(1024)]
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public bool ShowInSite { get; set; } = false;
    }
}
