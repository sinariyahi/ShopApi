using Infrastructure.Common;
using Infrastructure.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Blog
{
    public interface IArticleService
    {
        Task<ShopActionResult<List<ArticleDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<List<UserArticleDto>>> GetLastArticle();

        Task<ShopActionResult<int>> Add(ArticleDto model);
        Task<ShopActionResult<int>> Update(ArticleDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ArticleDto>> GetById(int id);
        Task<ShopActionResult<List<UserArticleDto>>> GetUserList(GridQueryModel model = null);
        Task<ShopActionResult<List<int>>> GetAllId();
    }
}
