using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Catalog
{
    public interface ISymbolService
    {
        Task<ShopActionResult<List<SymbolDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(SymbolDto model);
        Task<ShopActionResult<int>> Update(SymbolDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<SymbolDto>> GetById(int id);
        Task<ShopActionResult<List<TreeDto>>> GetTree(int? parentId);
    }
}
