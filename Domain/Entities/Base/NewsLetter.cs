using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("NewsLetters", Schema = "Base")]
    public class NewsLetter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Template { get; set; }
        public string Remark { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
