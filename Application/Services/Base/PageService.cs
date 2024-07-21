using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Base
{
    public class PageService : IPageService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Page> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public PageService(BIContext context, IGenericQueryService<Page> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(PageInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new Page
            {
                IsActive = model.IsActive,
                Title = model.Title,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Link = model.Link,
                SortOrder = model.SortOrder,
                PagesLinkType = model.PagesLinkType,
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();


            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(Guid id)
        {
            var result = new ShopActionResult<int>();

            var item = new Page { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<List<string>>> GetAllLink()
        {
            var result = new ShopActionResult<List<string>>();

            var data = await context.Pages.Select(s => s.Link).ToListAsync();

            result.IsSuccess = true;
            result.Data = data;
            return result;
        }

        public async Task<ShopActionResult<PageDto>> GetById(Guid id)
        {
            var result = new ShopActionResult<PageDto>();

            var data = await context.Pages.FirstOrDefaultAsync(f => f.Id == id);
            var model = new PageDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                Description = data.Description == null ? "" : data.Description,
                Link = data.Link,
                SortOrder = data.SortOrder,
                PagesLinkType = data.PagesLinkType,

            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<PageDto>> GetByTitle(string link)
        {
            var result = new ShopActionResult<PageDto>();

            var data = await context.Pages.FirstOrDefaultAsync(f => f.Link.Contains(link));
            var model = new PageDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                Description = data.Description == null ? "" : data.Description,
                Link = data.Link,
                PagesLinkType = data.PagesLinkType,

            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<PageDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<PageDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new PageDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                Description = q.Description == null ? "" : q.Description,
                Link = q.Link,
                PagesLinkType = q.PagesLinkType,
                PagesLinkTypeTitle = q.PagesLinkType.GetNameAttribute()
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<UserPageDto>>> GetListUserPages()
        {
            var result = new ShopActionResult<List<UserPageDto>>();


            result.Data = context.Pages.Where(w => w.IsActive == true).OrderBy(o => o.SortOrder).Select(q => new UserPageDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description == null ? "" : q.Description,
                Link = q.Link,
                PagesLinkType = q.PagesLinkType,

            }).ToList();
            result.IsSuccess = true;

            return result;
        }


        public async Task<ShopActionResult<int>> Update(PageInputModel model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.Pages.FirstOrDefaultAsync(f => f.Id == model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.Description = model.Description;
            data.Link = model.Link;
            data.SortOrder = model.SortOrder;
            data.PagesLinkType = model.PagesLinkType;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

    }
}
