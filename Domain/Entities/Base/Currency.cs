using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("Currencies", Schema = "Base")]
    public class Currency
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(8)]
        public string Symbol { get; set; }
        public bool IsActive { get; set; }
    }
}
