using Domain.Entities.Blog;
using Domain.Entities.Catalog;
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
    [Table("UserOpinions", Schema = "Support")]
    public class UserOpinion
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }


        public int? ArticleId { get; set; }
        public virtual Article Article { get; set; }


        [Required, MaxLength(512)]
        public string Remark { get; set; }
     //   public UserOpinionType UserOpinionType { get; set; }

       // public ShowStatus ShowStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Score { get; set; }

        public virtual ICollection<UserOpinionDetail> UserOpinionDetails { get; set; }

        public UserOpinion()
        {
            UserOpinionDetails = new HashSet<UserOpinionDetail>();

        }
    }
    [Table("UserOpinionDetails", Schema = "Support")]

    public class UserOpinionDetail
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserOpinionId { get; set; }

        public virtual UserOpinion UserOpinion { get; set; }

        //public UseOpinionStatus UseOpinionStatus { get; set; }
        [Required, MaxLength(512)]
        public string Remark { get; set; }
    }
}
