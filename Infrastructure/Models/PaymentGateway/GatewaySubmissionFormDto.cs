using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PaymentGateway
{
    public class GatewaySubmissionFormDto
    {
        public string RefId { get; set; }
    }

    public class SalePaymentRequestModel
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string CustomerMobileNumber { get; set; }
    }

    public class GatewayResponseDto
    {
        public string ResCode { get; set; }
        public long SaleReferenceId { get; set; }
        public long SaleOrderId { get; set; }
        public long OrderId { get; set; }
    }
}
