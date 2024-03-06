using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Support
{
    [Table("UserNewsLetters", Schema = "Support")]
    public class UserNewsLetter
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreateDate { get; set; }


    }

}
