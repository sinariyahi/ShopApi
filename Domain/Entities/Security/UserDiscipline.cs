using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserDisciplines", Schema = "Security")]
    public class UserDiscipline
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int DisciplineId { get; set; }
        public virtual Role Discipline { get; set; }
    }
}
