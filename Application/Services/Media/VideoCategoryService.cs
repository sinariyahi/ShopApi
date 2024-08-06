using Application.Interfaces.Media;
using Application.Interfaces;
using Domain.Entities.Media;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Media
{
    public class VideoCategoryService : IVideoCategoryService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<VideoCategory> videoCategoryService;
        public VideoCategoryService(BIContext context,
            IGenericQueryService<VideoCategory> videoCategoryService
            )
        {
            this.context = context;
            this.videoCategoryService = videoCategoryService;
        }

        public async Task<ShopActionResult<int>> Add(VideoCategoryDto model)
        {
            var result = new ShopActionResult<int>();
            var item = new VideoCategory
            {
                Title = model.Title,
                IsActive = model.IsActive,
                SortOrder = model.SortOrder,
                Remark = model.Remark,
            };

            await context.AddAsync(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }


        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new VideoCategory { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<VideoCategoryDto>> GetById(int id)
        {
            var result = new ShopActionResult<VideoCategoryDto>();

            var item = await context.VideoCategories.SingleOrDefaultAsync(q => q.Id == id);

            var model = new VideoCategoryDto
            {
                Id = item.Id,
                Title = item.Title,
                SortOrder = item.SortOrder,
                IsActive = item.IsActive,
                Remark = item.Remark,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<VideoCategoryDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<VideoCategoryDto>>();

            var queryResult = await videoCategoryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new VideoCategoryDto
            {
                Id = q.Id,
                Title = q.Title,
                SortOrder = q.SortOrder,
                IsActive = q.IsActive,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                Remark = q.Remark,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<VideoCategoryDto>>> GetUserList()
        {
            var result = new ShopActionResult<List<VideoCategoryDto>>();


            result.Data = await context.VideoCategories.Where(w => w.IsActive == true).Select(q => new VideoCategoryDto
            {
                Id = q.Id,
                Title = q.Title,
                SortOrder = q.SortOrder,
                IsActive = q.IsActive,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",

            }).ToListAsync();
            result.IsSuccess = true;

            return result;
        }

        public async Task<ShopActionResult<int>> Update(VideoCategoryDto model)
        {
            var result = new ShopActionResult<int>();


            var item = await context.VideoCategories.SingleOrDefaultAsync(q => q.Id == model.Id);
            item.Title = model.Title;
            item.IsActive = model.IsActive;
            item.SortOrder = model.SortOrder;
            item.Remark = model.Remark;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
          //  result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
