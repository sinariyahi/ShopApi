using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Support
{
    public class InternalMessageDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ActionLink { get; set; }
        public DateTime SendDate { get; set; }
        public InternalMessageType InternalMessageType { get; set; }
        public bool SendEmail { get; set; }
        public DateTime? VisitedEmailDate { get; set; }
        public DateTime? VisitedFromAppDate { get; set; }
        public int? ReciverRoleId { get; set; }
        public Guid? ReciverUserId { get; set; }
        public Guid SenderUserId { get; set; }
        public int SenderUserRoleId { get; set; }
    }
}
