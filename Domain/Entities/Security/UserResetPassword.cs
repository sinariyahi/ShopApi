using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserResetPasswords", Schema = "Security")]
    public class UserResetPassword
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid ResetToken { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime UsedDate { get; set; }
    }
}
