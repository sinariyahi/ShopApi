using Infrastructure.Common;
using Infrastructure.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Blog
{
    public interface IArticleCategoryService
    {
        Task<ShopActionResult<List<ArticleCategoryDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(ArticleCategoryDto model);
        Task<ShopActionResult<int>> Update(ArticleCategoryDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ArticleCategoryDto>> GetById(int id);
        Task<ShopActionResult<List<ArticleCategoryDto>>> GetUserList();
    }
}
