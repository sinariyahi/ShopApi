using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("Kopons", Schema = "Catalog")]
    public class Kopon
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Remark { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int Percent { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
