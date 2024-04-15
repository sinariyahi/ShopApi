using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Blog
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public int ArticleCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public string IsActiveTitle { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string ArticleCategoryTitle { get; set; }
        public Guid UserId { get; set; }
        public List<IFormFile> File { get; set; }
        public List<FileItemDto> ArticleAttachments { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreateDate { get; set; }
    }


    public class UserArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public int ArticleCategoryId { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string ArticleCategoryTitle { get; set; }
        public string File { get; set; }
        public string ShortDescription { get; set; }

    }


}
