using Application.Interfaces.Media;
using Application.Interfaces;
using Domain.Entities.Media;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Media
{
    public class VideoService : IVideoService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Video> videoQueryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public VideoService(BIContext context,
            IGenericQueryService<Video> videoQueryService,
                  IFileService fileService, IOptions<Configs> options
            )
        {
            this.context = context;
            this.videoQueryService = videoQueryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }
        public async Task<ShopActionResult<int>> Add(VideoDto model)
        {
            var result = new ShopActionResult<int>();
            var item = new Video
            {
                Remark = model.Remark,
                CreateDate = DateTime.Now,
                VideoCategoryId = model.VideoCategoryId,
                Title = model.Title,
                IsActive = model.IsActive,
                SortOrder = model.SortOrder,
                VideoSource = model.VideoSource,
                VideoLink = model.VideoLink,
                SeoDescription = model.SeoDescription,
                SeoTitle = model.SeoTitle,
                ShortDescription = model.ShortDescription,
            };
            await context.AddAsync(item);
            await context.SaveChangesAsync();

            #region Attachments
            if (model.File != null)
            {

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "VideoAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Media", "VideoAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }


            if (model.CoverFile != null)
            {

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "VideoCoverAttachments");

                for (int i = 0; i < model.CoverFile.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.CoverFile[i], "Media", "VideoCoverAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }
            #endregion



            result.IsSuccess = true;
           // result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new Video { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<List<int>>> GetAllId()
        {
            var result = new ShopActionResult<List<int>>();

            result.Data = await context.Videos.Select(x => x.Id).ToListAsync();

            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<VideoDto>> GetById(int id)
        {
            var result = new ShopActionResult<VideoDto>();

            var item = await context.Videos
                .Include(q => q.VideoCategory)
                .Include(q => q.VideoAttachments)
                .Include(q => q.VideoCoverAttachments)
                .SingleOrDefaultAsync(q => q.Id == id);

            var model = new VideoDto
            {
                Title = item.Title,
                VideoCategoryId = item.VideoCategoryId,
                IsActive = item.IsActive,
                Id = item.Id,
                Remark = item.Remark,
                SortOrder = item.SortOrder,
                FileAttachment = item.VideoAttachments.Select(s => new FileItemDto { Entity = "VideoAttachments", FilePath = s.FilePath }).ToList(),
                CoverAttachment = item.VideoCoverAttachments.Select(s => new FileItemDto { Entity = "VideoCoverAttachments", FilePath = s.FilePath }).ToList(),
                VideoLink = item.VideoLink,
                VideoSource = item.VideoSource,
                SeoTitle = item.SeoTitle,
                SeoDescription = item.SeoDescription,
                ShortDescription = item.ShortDescription,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<UserVideoDto>>> GetLastVideo()
        {
            var result = new ShopActionResult<List<UserVideoDto>>();


            var queryResult = await context.Videos.Include(q => q.VideoCategory)
                .Include(i => i.VideoAttachments)
                .Include(i => i.VideoCoverAttachments).Where(w => w.IsActive == true && w.VideoCategory.IsActive == true).OrderByDescending(o => o.CreateDate).ToListAsync();


            result.Data = queryResult.Select(q => new UserVideoDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title != null ? q.Title : "",
                File = q.VideoAttachments.Count > 0 ? q.VideoAttachments.FirstOrDefault().FilePath : "",
                Cover = q.VideoCoverAttachments.Count > 0 ? q.VideoCoverAttachments.FirstOrDefault().FilePath : "",
                SeoDescription = q.SeoDescription != null ? q.SeoDescription : "",
                SeoTitle = q.SeoTitle != null ? q.SeoTitle : "",
                VideoCategoryId = q.VideoCategoryId,
                VideoCategoryTitle = q.VideoCategory.Title,
                CreateDate = q.CreateDate,
                ShortDescription = q.ShortDescription
            }).Take(5).ToList();


            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<VideoDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<VideoDto>>();

            var queryResult = await videoQueryService.QueryAsync(model, includes: new List<string> { "VideoCategory" });

            result.Data = queryResult.Data.Select(q => new VideoDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title,
                VideoLink = q.VideoLink,
                IsActive = q.IsActive,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                VideoCategoryId = q.VideoCategoryId,
                SortOrder = q.SortOrder,
                VideoCategoryTitle = q.VideoCategory.Title,
                VideoSourceTitle = EnumHelpers.GetNameAttribute<VideoSource>(q.VideoSource).ToString(),
                SeoDescription = q.SeoDescription,
                ShortDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<UserVideoDto>>> GetUserList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserVideoDto>>();


            int videoCategoryId = 0;
            int skip = (model.Page - 1) * model.Size;

            foreach (var item in model.Filtered)
            {
                if (item.column == "videoCategoryId")
                {
                    videoCategoryId = Convert.ToInt32(item.value);
                }
            }
            //var queryResult = await videoQueryService.QueryAsync(model, includes: new List<string> { "VideoCategory", "VideoAttachments", "VideoCoverAttachments" });


            var queryResult = await context.Videos.Include(i => i.VideoCategory).Include(i => i.VideoAttachments)
                .Include(i => i.VideoCoverAttachments).Where(w => w.IsActive == true && w.VideoCategory.IsActive == true && (videoCategoryId == 0 || w.VideoCategoryId == videoCategoryId)).OrderBy(o => o.SortOrder).ToListAsync();


            result.Data = queryResult.Select(q => new UserVideoDto
            {
                Id = q.Id,
                Remark = q.Remark,
                Title = q.Title,
                VideoLink = q.VideoLink,
                VideoCategoryId = q.VideoCategoryId,
                SortOrder = q.SortOrder,
                File = q.VideoAttachments.Count() > 0 ? q.VideoAttachments.FirstOrDefault().FilePath : null,
                Cover = q.VideoCoverAttachments.Count() > 0 ? q.VideoCoverAttachments.FirstOrDefault().FilePath : null,
                VideoCategoryTitle = q.VideoCategory.Title,
                VideoSourceTitle = EnumHelpers.GetNameAttribute<VideoSource>(q.VideoSource).ToString(),
                SeoDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
                ShortDescription = q.ShortDescription,
            }).Skip(skip).Take(model.Size).ToList();


            result.IsSuccess = true;
            result.Total = queryResult.Count;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(VideoDto model)
        {
            var result = new ShopActionResult<int>();

            var item = await context.Videos
                .Include(q => q.VideoCategory)
                .Include(q => q.VideoAttachments)
                .Include(q => q.VideoCoverAttachments)
                .FirstOrDefaultAsync(q => q.Id == model.Id);

            if (item == null)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.ItemNotFound;
                return result;
            }

            item.VideoSource = model.VideoSource;
            item.Remark = model.Remark;
            item.VideoCategoryId = model.VideoCategoryId;
            item.IsActive = model.IsActive;
            item.Title = model.Title;
            item.VideoLink = model.VideoLink;
            item.SortOrder = model.SortOrder;
            item.SeoTitle = model.SeoTitle;
            item.SeoDescription = model.SeoDescription;
            item.ShortDescription = model.ShortDescription;



            #region Attachments
            if (model.File != null)
            {
                //context.VideoAttachments.RemoveRange(item.VideoAttachments);

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "VideoAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Media", "VideoAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }

            if (model.CoverFile != null)
            {
                //context.VideoCoverAttachments.RemoveRange(item.VideoCoverAttachments);

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "VideoCoverAttachments");

                for (int i = 0; i < model.CoverFile.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.CoverFile[i], "Media", "VideoCoverAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }
            #endregion
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
