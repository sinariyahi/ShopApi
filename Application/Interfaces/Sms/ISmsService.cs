using Infrastructure.Common;
using Infrastructure.Models.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Sms
{
    public interface ISmsService
    {
        Task<ShopActionResult<bool>> SendSmsForgetPasswordForAdmin(Guid userid);
        Task<ShopActionResult<bool>> SendSmsForAlerts(string username, string password);
        Task<ShopActionResult<bool>> SendSmsForgetPassword(string userName);
        Task<ShopActionResult<bool>> SendSmsForConfirmPhoneNumber(Guid userid, string phoneNumber);
        Task<ShopActionResult<bool>> CheckCodeSmSForPhoneNumber(string code, Guid userId);
        Task<ShopActionResult<bool>> SendSmsForRegisterUser(string phoneNumber);
        Task<ShopActionResult<Guid>> CheckCodeSmSForForgetPasswordUser(string code, string userName);
        Task<ShopActionResult<List<SmsDto>>> GetList(GridQueryModel model);
        Task<ShopActionResult<bool>> CheckCodeSmSForRegisterUser(string code, string phoneNumber);
        Task<ShopActionResult<bool>> SendSmsForRequestAlerts(Guid userId, string alert);
        Task<ShopActionResult<bool>> SendSmsForActiveUser(string phoneNumber);
        Task<ShopActionResult<bool>> CheckCodeSmSForActiveUser(string code, string phoneNumber);
    }
}
