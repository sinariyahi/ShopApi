using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Support
{
    [Table("UserMessages", Schema = "Support")]
    public class UserMessage
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Subject { get; set; }
        public string Body { get; set; }
        [MaxLength(256)]
        public string ActionLink { get; set; }
        [MaxLength(128)]
        public string To { get; set; }
        [MaxLength(512)]
        public string CC { get; set; }
        public DateTime SendDate { get; set; }
     //   public UserMessageType UserMessageType { get; set; }
        public bool SendEmail { get; set; }
        public bool? Visited { get; set; }
        public DateTime? VisitedEmailDate { get; set; }
        public DateTime? VisitedFromAppDate { get; set; }
        public bool SendWithSystem { get; set; }
        public Guid? SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public virtual User ReceiverUser { get; set; }
        public int? SenderUserRoleId { get; set; }
        public virtual Role SenderUserRole { get; set; }
    }
}
