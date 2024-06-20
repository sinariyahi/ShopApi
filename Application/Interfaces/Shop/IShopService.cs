using Infrastructure.Common;
using Infrastructure.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Shop
{
    public interface IShopService
    {
        Task<ShopActionResult<PartBalanceInfoDto>> GetPartBalanceInfo(string partNo);
        Task<ShopActionResult<List<KeyValueDto>>> GetBasicData(int datatype, int parentId);
        Task<ShopActionResult<List<ParishItemDto>>> GetParishList(int cityId, int regionId, string term);
        Task<ShopActionResult<string>> RegisterOnlineSale(RegisterOnlineSaleDto data);
    }
}
