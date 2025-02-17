using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Support
{
    [Table("InternalMessages", Schema = "Support")]
    public class InternalMessage
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Subject { get; set; }
        public string Body { get; set; }
        [MaxLength(256)]
        public string ActionLink { get; set; }
        public DateTime SendDate { get; set; }
        public InternalMessageType InternalMessageType { get; set; }
        public bool SendEmail { get; set; }
        public DateTime? VisitedEmailDate { get; set; }
        public DateTime? VisitedFromAppDate { get; set; }
        public int? ReciverRoleId { get; set; }
        public virtual Role ReciverRole { get; set; }
        public Guid? ReciverUserId { get; set; }
        public virtual User ReciverUser { get; set; }
        public Guid SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        public int SenderUserRoleId { get; set; }
        public virtual Role SenderUserRole { get; set; }
    }
}
