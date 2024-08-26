using Application.Interfaces.Support;
using Application.Interfaces;
using Domain.Entities.Support;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Support;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Support
{
    public class CooperationFormService : ICooperationFormService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<CooperationForm> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public CooperationFormService(BIContext context, IGenericQueryService<CooperationForm> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(CooperationFormInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new CooperationForm
            {

                JobOpportunityId = model.JobOpportunityId,
                Education = model.Education,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreateDate = DateTime.Now,

            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Support", "CooperationFormAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Support", "CooperationFormAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }

            result.IsSuccess = true;
           // result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new CooperationForm { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }




        public async Task<ShopActionResult<CooperationFormDto>> GetById(int id)
        {
            var result = new ShopActionResult<CooperationFormDto>();

            var data = await context.CooperationForms.Include(q => q.CooperationFormAttachments).Include(i => i.JobOpportunity).FirstOrDefaultAsync(f => f.Id == id);
            var model = new CooperationFormDto
            {
                Id = data.Id,
                PhoneNumber = data.PhoneNumber,
                FullName = data.FullName,
                Email = data.Email,
                Education = data.Education,
                JobOpportunityTitle = data.JobOpportunity.Title,
                File = data.CooperationFormAttachments.Select(s => new FileItemDto { Entity = "CooperationFormAttachments", FilePath = s.FilePath }).ToList(),
            };

            data.IsVisited = true;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<CooperationFormDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<CooperationFormDto>>();

            var queryResult = await _queryService.QueryAsync(model, null, new List<string>() { "CooperationFormAttachments", "JobOpportunity" });

            result.Data = queryResult.Data.Select(q => new CooperationFormDto
            {
                Id = q.Id,
                PhoneNumber = q.PhoneNumber,
                FullName = q.FullName,
                Email = q.Email,
                Education = q.Education,
                JobOpportunityTitle = q.JobOpportunity.Title,
                IsVisited = q.IsVisited,
                CreateDate = DateUtility.ToShamsi(q.CreateDate),
                File = q.CooperationFormAttachments.Select(s => new FileItemDto { Entity = "CooperationFormAttachments", FilePath = s.FilePath }).ToList(),
            }).ToList();


            result.Data = result.Data.OrderByDescending(q => q.CreateDate).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }




    }
}
