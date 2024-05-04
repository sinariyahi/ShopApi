using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Media
{
    public class BannerDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SortOrder { get; set; }
        public string SeoTitle { get; set; }
        public string Link { get; set; }
        public PositionPlace PositionPlace { get; set; }
        public string IsActiveTitle { get; set; }

        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<FileItemDto> FileAttachment { get; set; }
        public string PositionPlaceTitle { get; set; }
    }

    public class UserBannerDto
    {
        public List<UserModelBannerDto> List { get; set; }
        public UserModelBannerDto Model { get; set; }
    }


    public class UserModelBannerDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Link { get; set; }
        public PositionPlace PositionPlace { get; set; }
        public List<string> FileAttachment { get; set; }
    }



}
