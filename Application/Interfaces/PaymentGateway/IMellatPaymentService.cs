using Infrastructure.Common;
using Infrastructure.Models.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.PaymentGateway
{
    public interface IMellatPaymentService
    {
        Task<ShopActionResult<GatewaySubmissionFormDto>> SalePayment(SalePaymentRequestModel model);
        Task<ShopActionResult<bool>> SalePaymentConfirm(GatewayResponseDto model);
    }
}
