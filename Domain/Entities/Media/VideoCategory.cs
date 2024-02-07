using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Media
{
    [Table("VideoCategories", Schema = "Media")]
    public class VideoCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Title { get; set; }

        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }

        public string Remark { get; set; }
    }
}
