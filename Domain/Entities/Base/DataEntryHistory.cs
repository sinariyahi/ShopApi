using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("DataEntryHistories", Schema = "Base")]
    public class DataEntryHistory
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? UserId { get; set; }
       // public virtual User User { get; set; }
        public bool? IsAdminUser { get; set; }
    }
}
