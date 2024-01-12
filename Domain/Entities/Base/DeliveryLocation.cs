using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("DeliveryLocations", Schema = "Base")]

    public class DeliveryLocation
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }
}
