using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Media
{
    public class VideoDto
    {
        public int Id { get; set; }
        public int VideoCategoryId { get; set; }
        public string VideoCategoryTitle { get; set; }

        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public int? SortOrder { get; set; }
        public string Remark { get; set; }
        public string VideoLink { get; set; }
        public VideoSource VideoSource { get; set; }
        public List<IFormFile> CoverFile { get; set; }

        public List<IFormFile> File { get; set; }
        public List<FileItemDto> FileAttachment { get; set; }
        public List<FileItemDto> CoverAttachment { get; set; }

        public string VideoSourceTitle { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string ShortDescription { get; set; }

    }

    public class UserVideoDto
    {
        public int Id { get; set; }
        public int VideoCategoryId { get; set; }
        public string VideoCategoryTitle { get; set; }

        public string Title { get; set; }
        public int? SortOrder { get; set; }
        public string Remark { get; set; }
        public string VideoLink { get; set; }
        public VideoSource VideoSource { get; set; }
        public string VideoSourceTitle { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string File { get; set; }
        public string Cover { get; set; }
        public DateTime CreateDate { get; set; }
        public string ShortDescription { get; set; }

    }

}
