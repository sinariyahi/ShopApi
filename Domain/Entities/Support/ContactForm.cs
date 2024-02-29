using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Support
{
    [Table("ContactForms", Schema = "Support")]
    public class ContactForm
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Remark { get; set; }
        public string Subject { get; set; }


        public bool IsVisited { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<ContactFormAttachment> ContactFormAttachments { get; set; }


        public ContactForm()
        {
            ContactFormAttachments = new HashSet<ContactFormAttachment>();
        }
    }

    [Table("ContactFormAttachments", Schema = "Support")]
    public class ContactFormAttachment
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
     //   public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }
        public int ContactFormId { get; set; }
        public virtual ContactForm ContactForm { get; set; }

    }
}
