using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class EmailTemplateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Template { get; set; }
        public string DefaultTemplate { get; set; }
        public bool IsActive { get; set; }
        public string EmailCC { get; set; }
        public int? ProjectId { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
        public string EmailTemplateTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RegisterUserId { get; set; }
        public string ProjectName { get; set; }
    }

    public class EmailSenderInfoDto
    {
        public string SenderName { get; set; }
        public string SenderRole { get; set; }
        public string SenderEmail { get; set; }
        public string Website { get; set; }
        public string ReceiverEmail { get; set; }
        public string EmailSignature { get; set; }
    }

    public class InternalMessageFilter
    {
        public string Subject { get; set; }
        public UserMessageType? UserMessageType { get; set; }
        public string Company { get; set; }
        public string ReceiverEmail { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
