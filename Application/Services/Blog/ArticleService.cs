using Application.Interfaces.Blog;
using Application.Interfaces;
using Domain.Entities.Blog;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Blog;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Blog
{
    public class ArticleService : IArticleService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Article> articleQueryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public ArticleService(BIContext context,
            IGenericQueryService<Article> articleQueryService,
                  IFileService fileService, IOptions<Configs> options
            )
        {
            this.context = context;
            this.articleQueryService = articleQueryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(ArticleDto model)
        {
            var result = new ShopActionResult<int>();
            var item = new Article
            {
                Remark = model.Remark,
                CreateDate = DateTime.Now,
                ArticleCategoryId = model.ArticleCategoryId,
                Title = model.Title,
                IsActive = model.IsActive,
                ShortDescription = model.ShortDescription,
                SeoTitle = model.SeoTitle,
                SeoDescription = model.SeoDescription,
            };
            await context.AddAsync(item);
            await context.SaveChangesAsync();

            #region ArticleAttachments
            if (model.File != null)
            {

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Blog", "ArticleAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Blog", "ArticleAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }
            #endregion






            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new Article { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<ArticleDto>> GetById(int id)
        {
            var result = new ShopActionResult<ArticleDto>();

            var item = await context.Articles
                .Include(q => q.ArticleCategory)
                .Include(q => q.ArticleAttachments)
                .SingleOrDefaultAsync(q => q.Id == id);

            var model = new ArticleDto
            {
                Title = item.Title,
                ArticleCategoryId = item.ArticleCategoryId,
                ArticleCategoryTitle = item.ArticleCategory.Title,
                IsActive = item.IsActive,
                IsActiveTitle = item.IsActive == true ? "فعال" : "غیرفعال",
                Id = item.Id,
                Remark = item.Remark,
                ShortDescription = item.ShortDescription,
                ArticleAttachments = item.ArticleAttachments.Select(s => new FileItemDto { Entity = "ArticleAttachments", FilePath = s.FilePath }).ToList(),
                SeoTitle = item.SeoTitle,
                SeoDescription = item.SeoDescription,
                CreateDate = item.CreateDate,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }


        public async Task<ShopActionResult<List<int>>> GetAllId()
        {
            var result = new ShopActionResult<List<int>>();

            result.Data = await context.Articles.Where(w => w.IsActive == true && w.ArticleCategory.IsActive == true).Select(q => q.Id).ToListAsync();


            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<UserArticleDto>>> GetLastArticle()
        {

            var result = new ShopActionResult<List<UserArticleDto>>();

            var queryResult = await context.Articles
                .Include(i => i.ArticleCategory).Include(i => i.ArticleAttachments).Where(w => w.IsActive == true && w.ArticleCategory.IsActive == true).OrderByDescending(o => o.CreateDate).ToListAsync();


            result.Data = queryResult.Select(q => new UserArticleDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title != null ? q.Title : "",
                ShortDescription = q.ShortDescription != null ? q.ShortDescription : "",
                File = q.ArticleAttachments.Count > 0 ? q.ArticleAttachments.FirstOrDefault().FilePath : "",
                SeoDescription = q.SeoDescription != null ? q.SeoDescription : "",
                SeoTitle = q.SeoTitle != null ? q.SeoTitle : "",
                ArticleCategoryId = q.ArticleCategoryId,
                ArticleCategoryTitle = q.ArticleCategory.Title,
            }).Take(5).ToList();

            result.IsSuccess = true;

            return result;

        }

        public async Task<ShopActionResult<List<ArticleDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<ArticleDto>>();

            var queryResult = await articleQueryService.QueryAsync(model, includes: new List<string> { "ArticleCategory", "ArticleAttachments" });

            result.Data = queryResult.Data.Select(q => new ArticleDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title != null ? q.Title : "",
                ShortDescription = q.ShortDescription != null ? q.ShortDescription : "",
                IsActive = q.IsActive,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                ArticleAttachments = q.ArticleAttachments.Select(s => new FileItemDto { Entity = "BannerAttachments", FilePath = s.FilePath }).ToList(),
                ArticleCategoryId = q.ArticleCategoryId,
                ArticleCategoryTitle = q.ArticleCategory.Title
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }


        public async Task<ShopActionResult<List<UserArticleDto>>> GetUserList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserArticleDto>>();
            int articleCategoryId = 0;
            int skip = (model.Page - 1) * model.Size;

            foreach (var item in model.Filtered)
            {
                if (item.column == "articleCategoryId")
                {
                    articleCategoryId = Convert.ToInt32(item.value);
                }
            }

            var queryResult = await context.Articles.Include(i => i.ArticleCategory).Include(i => i.ArticleAttachments).Where(w => w.IsActive == true && w.ArticleCategory.IsActive == true
            && (articleCategoryId == 0 || w.ArticleCategoryId == articleCategoryId)).ToListAsync();

            result.Data = queryResult.Select(q => new UserArticleDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title != null ? q.Title : "",
                ShortDescription = q.ShortDescription != null ? q.ShortDescription : "",
                File = q.ArticleAttachments.Count > 0 ? q.ArticleAttachments.FirstOrDefault().FilePath : "",
                SeoDescription = q.SeoDescription != null ? q.SeoDescription : "",
                SeoTitle = q.SeoTitle != null ? q.SeoTitle : "",
                ArticleCategoryId = q.ArticleCategoryId,
                ArticleCategoryTitle = q.ArticleCategory.Title
            }).Skip(skip).Take(model.Size).ToList();

            result.IsSuccess = true;
            result.Total = queryResult.Count;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }



        public async Task<ShopActionResult<int>> Update(ArticleDto model)
        {
            var result = new ShopActionResult<int>();

            var item = await context.Articles
                .Include(q => q.ArticleCategory)
                .Include(q => q.ArticleAttachments)
                .SingleOrDefaultAsync(q => q.Id == model.Id);

            if (item == null)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.ItemNotFound;
                return result;
            }

            item.ShortDescription = model.ShortDescription;
            item.Remark = model.Remark;
            item.ArticleCategoryId = model.ArticleCategoryId;
            item.IsActive = model.IsActive;
            item.Title = model.Title;
            item.SeoTitle = model.SeoTitle;
            item.SeoDescription = model.SeoDescription;
            #region ArticleAttachments
            if (model.File != null)
            {
                context.ArticleAttachments.RemoveRange(item.ArticleAttachments);

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Blog", "ArticleAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Blog", "ArticleAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }
            #endregion
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
