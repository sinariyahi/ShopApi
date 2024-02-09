using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("Cartables", Schema = "Security")]
    public class Cartable
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(150)]
        public string EnTitle { get; set; }
        [MaxLength(500)]
        public string Url { get; set; }
        //public CartableType CartableType { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
        public ICollection<UserCartable> UserCartables { get; set; }
        //public ICollection<CartableRecord> CartableRecords { get; set; }

        public Cartable()
        {
           // CartableRecords = new HashSet<CartableRecord>();
         //   UserCartables = new HashSet<UserCartable>();
        }

    }

    [Table("UserCartables", Schema = "Security")]
    public class UserCartable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
       // public virtual User User { get; set; }
        public int CartableId { get; set; }
        public virtual Cartable Cartable { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RegisterUserId { get; set; }
     //   public virtual User RegisterUser { get; set; }
    }
}
