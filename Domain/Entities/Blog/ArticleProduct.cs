using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Catalog;

namespace Domain.Entities.Blog
{
    [Table("ArticleProducts", Schema = "Blog")]
    public class ArticleProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }


    }
}
