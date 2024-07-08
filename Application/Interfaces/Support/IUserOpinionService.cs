using Infrastructure.Common;
using Infrastructure.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Support
{
    public interface IUserOpinionService
    {
        Task<ShopActionResult<List<ListUserOpinionDto>>> GetAll(FilterUserOpinionDto model = null);
        Task<ShopActionResult<UserOpinionDto>> GetById(Guid id);
        Task<ShopActionResult<List<UserOpinionDto>>> GetByProductId(int ProductId);
        Task<ShopActionResult<int>> Add(UserOpinionDto model);
        Task<ShopActionResult<int>> Update(UserInputOpinionModel model);
        Task<ShopActionResult<int>> Delete(Guid id);
        Task<ShopActionResult<List<UserOpinionDto>>> GetByArticleId(int articleId);
    }
}
