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
    [Table("CompanyQuestions", Schema = "Support")]

    public class CompanyQuestion
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
       // public CompanyQuestionStatus CompanyQuestionStatus { get; set; }
        public byte[] AttachmentFile { get; set; }
     //   public ICollection<UserAnswer> UserAnswers { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateUserId { get; set; }
        public virtual User CreateUser { get; set; }
    }
}
