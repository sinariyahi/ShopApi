using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserRefreshTokenHistories", Schema = "Security")]
    public class UserRefreshTokenHistory
    {

        public int Id { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsValid { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string UserIP { get; set; }
        public string UserBrowser { get; set; }
        public User User { get; set; }

    }
}
