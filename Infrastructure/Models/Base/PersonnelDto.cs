using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class PersonnelDto
    {
        public int Id { get; set; }
        public string NationalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public Sex Sex { get; set; }
        public Guid DataEntryHistoryCode { get; set; }
    }

}
