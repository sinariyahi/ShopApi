using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IPageService
    {
        Task<ShopActionResult<List<string>>> GetAllLink();

        Task<ShopActionResult<List<PageDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(PageInputModel model);
        Task<ShopActionResult<int>> Update(PageInputModel model);
        Task<ShopActionResult<int>> Delete(Guid id);
        Task<ShopActionResult<PageDto>> GetById(Guid id);
        Task<ShopActionResult<PageDto>> GetByTitle(string title);
        Task<ShopActionResult<List<UserPageDto>>> GetListUserPages();
    }
}
