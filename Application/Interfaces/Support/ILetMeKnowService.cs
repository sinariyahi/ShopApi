using Infrastructure.Common;
using Infrastructure.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Support
{
    public interface ILetMeKnowService
    {
        Task<ShopActionResult<List<UserRegisterLetMeKnowDtoDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Register(UserLetMeKnowDto model, Guid? userId);
        Task<byte[]> GetListForExcel(GridQueryModel model = null, string fileName = null);
    }
}
