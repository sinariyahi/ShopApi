using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface INewsLetterService
    {
        Task<ShopActionResult<List<NewsLetterDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(NewsLetterDto model);
        Task<ShopActionResult<int>> Update(NewsLetterDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<NewsLetterDto>> GetById(int id);
        Task<ShopActionResult<List<UserRegisterNewsLetterDto>>> GetUserRegisterNewsLetter(GridQueryModel model = null);
        Task<ShopActionResult<int>> Register(UserNewsLetterDto model, Guid? userId);
    }
}
