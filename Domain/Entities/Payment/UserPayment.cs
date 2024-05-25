using Domain.Entities.Catalog;
using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Payment
{
    [Table("UserPayments", Schema = "Payment")]
    public class UserPayment
    {
        public Guid Id { get; set; }
        public long Amount { get; set; }
        public Guid UserId { get; set; }
       public virtual User User { get; set; }
        public string OrderValue { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentResult { get; set; }
        public string BankConfirmResult { get; set; }

        public string TrackingCode { get; set; }
        public bool? IsSuccess { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public long? RefId { get; set; }
    }
}
