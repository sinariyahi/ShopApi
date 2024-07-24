using Application.Interfaces.Base;
using Application.Interfaces;
using Domain;
using Domain.Entities.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<SocialMedia> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public SocialMediaService(BIContext context, IGenericQueryService<SocialMedia> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(SocialMediaInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new SocialMedia
            {
                IsActive = model.IsActive,
                Title = model.Title,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Link = model.Link,

            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Base", "SocialMediaAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Base", "SocialMediaAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new SocialMedia { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }




        public async Task<ShopActionResult<SocialMediaDto>> GetById(int id)
        {
            var result = new ShopActionResult<SocialMediaDto>();

            var data = await context.SocialMedias.Include(q => q.SocialMediaAttachments).FirstOrDefaultAsync(f => f.Id == id);
            var model = new SocialMediaDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                Link = data.Link,
                Description = data.Description == null ? "" : data.Description,
                File = data.SocialMediaAttachments.Select(s => new FileItemDto { Entity = "SocialMediaAttachments", FilePath = s.FilePath }).ToList(),
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<SocialMediaDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<SocialMediaDto>>();

            var queryResult = await _queryService.QueryAsync(model, null, new List<string>() { "SocialMediaAttachments" });

            result.Data = queryResult.Data.Select(q => new SocialMediaDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                Description = q.Description == null ? "" : q.Description,
                Link = q.Link,
                File = q.SocialMediaAttachments.Select(s => new FileItemDto { Entity = "SocialMediaAttachments", FilePath = s.FilePath }).ToList(),
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<UserSocialMediaDto>>> GetUserList(int count = 8)
        {
            var result = new ShopActionResult<List<UserSocialMediaDto>>();


            result.Data = await context.SocialMedias.Include(i => i.SocialMediaAttachments).Where(w => w.IsActive == true).Select(q => new UserSocialMediaDto
            {
                Id = q.Id,
                Title = q.Title,
                Link = q.Link,
                File = q.SocialMediaAttachments.FirstOrDefault().FilePath,
            }).Take(count).ToListAsync();
            result.IsSuccess = true;

            return result;

        }



        public async Task<ShopActionResult<int>> Update(SocialMediaInputModel model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.SocialMedias.Include(q => q.SocialMediaAttachments).FirstOrDefaultAsync(f => f.Id == model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.Description = model.Description;
            data.Link = model.Link;

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Base", "SocialMediaAttachments");

                foreach (var item in data.SocialMediaAttachments)
                {
                    context.SocialMediaAttachments.Remove(item);
                }
                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Base", "SocialMediaAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }


            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

    }
}
