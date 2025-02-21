using Domain.Entities.Base;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Support
{
    [Table("CooperationForms", Schema = "Support")]
    public class CooperationForm
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Education { get; set; }

        public int JobOpportunityId { get; set; }

        public bool IsVisited { get; set; }
        public JobOpportunity JobOpportunity { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<CooperationFormAttachment> CooperationFormAttachments { get; set; }


        public CooperationForm()
        {
            CooperationFormAttachments = new HashSet<CooperationFormAttachment>();
        }
    }

    [Table("CooperationFormAttachments", Schema = "Support")]
    public class CooperationFormAttachment
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }
        public int CooperationFormId { get; set; }
        public virtual CooperationForm CooperationForm { get; set; }

    }
}
