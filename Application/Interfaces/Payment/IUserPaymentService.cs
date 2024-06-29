using Infrastructure.Common;
using Infrastructure.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Payment
{
    public interface IUserPaymentService
    {
        Task<ShopActionResult<List<UserPaymenstDto>>> GetUserAllPayments(UserPaymentFilterDto model, Guid userId);
        Task<ShopActionResult<List<UserPaymenstModel>>> GetUserAllPaymentsForUser(Guid userId);
    }
}
