using Infrastructure.Common;
using Infrastructure.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Support
{
    public interface IContactFormService
    {
        Task<ShopActionResult<List<ContactFormDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(ContactFormInputModel model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ContactFormDto>> GetById(int id);
    }
}
