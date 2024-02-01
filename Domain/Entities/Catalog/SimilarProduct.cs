using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("SimilarProducts", Schema = "Catalog")]
    public class SimilarProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int SimilarId { get; set; }
        public virtual Product Similar { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Remark { get; set; }


    }
}
