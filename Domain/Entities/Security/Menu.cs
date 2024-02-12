using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("Menus", Schema = "Security")]
    public class Menu
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(256)]

        public string Icon { get; set; }
        [MaxLength(256)]
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        [MaxLength(128)]
        public string Tag { get; set; }
    }
}
