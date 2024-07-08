using Infrastructure.Common;
using Infrastructure.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Support
{
    public interface ICooperationFormService
    {
        Task<ShopActionResult<List<CooperationFormDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(CooperationFormInputModel model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<CooperationFormDto>> GetById(int id);
    }
}
