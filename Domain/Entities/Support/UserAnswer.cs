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
    [Table("UserAnswers", Schema = "Support")]

    public class UserAnswer
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid SenderUserId { get; set; }
        public User SenderUser { get; set; }

        public Guid UserQuestionId { get; set; }
        public virtual CompanyQuestion UserQuestion { get; set; }
        public byte[] AttachmentFile { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
