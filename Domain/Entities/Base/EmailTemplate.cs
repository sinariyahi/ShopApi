using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Security;

namespace Domain.Entities.Base
{
    [Table("EmailTemplates", Schema = "Base")]
    public class EmailTemplate
    {
        public int Id { get; set; }
        [Required, MaxLength(256)]
        public string Title { get; set; }
        public string Template { get; set; }
        public string DefaultTemplate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(1024)]
        public string EmailCC { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RegisterUserId { get; set; }
        public virtual User RegisterUser { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
