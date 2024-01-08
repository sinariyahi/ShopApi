using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Blog
{
    [Table("Articles", Schema = "Blog")]
    public class Article
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string ShortDescription { get; set; }

        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int ArticleCategoryId { get; set; }
        public virtual ArticleCategory ArticleCategory { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }

        public virtual ICollection<ArticleAttachment> ArticleAttachments { get; set; }
        public Article()
        {
            ArticleAttachments = new HashSet<ArticleAttachment>();
        }
    }
}
