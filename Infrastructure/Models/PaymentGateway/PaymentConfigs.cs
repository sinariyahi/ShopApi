using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PaymentGateway
{
    public class PaymentConfigs
    {
        public long TerminalId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string CallBackUrl { get; set; }
        public string PayerId { get; set; }
        public string AppGatewayResult { get; set; }
        public string PaymentPageUrl { get; set; }
    }
}
