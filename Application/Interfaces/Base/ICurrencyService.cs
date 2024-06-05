using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface ICurrencyService
    {
        Task<ShopActionResult<List<CurrencyDto>>> GetList(GridQueryModel model = null);
        Task<List<ComboItemDto>> GetForCombo();
        Task<ShopActionResult<int>> Add(CurrencyDto model);
        Task<ShopActionResult<int>> Update(CurrencyDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<CurrencyDto>> GetById(int id);
    }
}
