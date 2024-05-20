using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.User
{
    public class ChangePasswordInputModel
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public Guid? UserId { get; set; }
    }

    public class ChangeEmailSignatureInputModel
    {
        public string EmailSignuature { get; set; }
        public Guid? UserId { get; set; }
    }
}
