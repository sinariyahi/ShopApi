using Infrastructure.Common;
using Infrastructure.Models.Authorization;
using Infrastructure.Models.Customer;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface ICustomerService
    {
        Task<ShopActionResult<List<CustomerDto>>> GetCustomers(GridQueryModel model = null);
        Task<ShopActionResult<CustomerInputModel>> AddCustomer(CustomerInputModel model, bool isAdmin = true);
        Task<ShopActionResult<CustomerInputModel>> UpdateCustomer(CustomerInputModel model, bool isAdmin = true);
        Task<ShopActionResult<CustomerDto>> GetCustomerById(Guid userId);
        Task<ShopActionResult<Guid>> UpdatePassword(ChangePasswordInputModel model, bool isCurrentUser = false);
        Task<ShopActionResult<AuthenticateModel>> RegisterCustomer(RegisterCustomerInputModel model);
        Task<ShopActionResult<CustomerInputModel>> UpdateCustomerByUser(CustomerInputModel model);
        Task<ShopActionResult<CustomerInputModel>> UpdateCustomerInfoByUser(CustomerInputModel model);

    }
}
