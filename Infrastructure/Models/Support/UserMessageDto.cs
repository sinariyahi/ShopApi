using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Support
{
    public class UserMessageDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ActionLink { get; set; }
        public DateTime SendDate { get; set; }
        public string SendDateString { get; set; }
        public UserMessageType UserMessageType { get; set; }
        public bool SendEmail { get; set; }
        public DateTime? VisitedEmailDate { get; set; }
        public string VisitedEmailDateString { get; set; }
        public DateTime? VisitedFromAppDate { get; set; }
        public string VisitedFromAppDateString { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public Guid? SenderUserId { get; set; }
        public int? SenderUserRoleId { get; set; }
        public bool SendWithSystem { get; set; }
        public string To { get; set; }
        public string SenderUserName { get; set; }
        public string UserMessageTypeTitle { get; set; }
        public string SenderUserRoleName { get; set; }
        public string ReceiverUserName { get; set; }
        public List<string> CC { get; set; } = new List<string>();
    }

    public class UserMessageStatistics
    {
        public int Total { get; set; }
        public int NotVisitedCount { get; set; }
    }
}
