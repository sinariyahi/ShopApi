using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class FileDto
    {
        public IFormFile File { get; set; }

    }
    public class FileItemDto
    {
        public string FilePath { get; set; }
        public string Entity { get; set; }

    }

    public class FileModel
    {

        public int CompanyId { get; set; }
        public string FilePath { get; set; }
        public string Title { get; set; }
        public CompanyType CompanyType { get; set; }
        public string CompanyTypeTitle { get; set; }
        public double Key { get; set; }
        public double Value { get; set; }
        public double? ParentId { get; set; }

        public CompanyAttachmentType CompanyAttachmentType { get; set; }
        public SupplierAttachmentType SupplierAttachmentType { get; set; }
        public VendorAttachmentType VendorAttachmentType { get; set; }

        public List<FileModel> Children { get; set; } = new List<FileModel>();
    }
}
