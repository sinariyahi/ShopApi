using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Security
{

    [Table("SmsLogs", Schema = "Security")]
    public class SmsLog
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime ActionTime { get; set; }
        public bool Status { get; set; }

         public SmsType SmsType { get; set; }
    }
}