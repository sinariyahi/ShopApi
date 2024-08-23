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
    public class ContactFormService : IContactFormService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<ContactForm> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public ContactFormService(BIContext context, IGenericQueryService<ContactForm> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(ContactFormInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new ContactForm
            {

                CreateDate = DateTime.Now,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Remark = model.Remark,
                Subject = model.Subject,

            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Support", "ContactFormAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Support", "ContactFormAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new ContactForm { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }




        public async Task<ShopActionResult<ContactFormDto>> GetById(int id)
        {
            var result = new ShopActionResult<ContactFormDto>();

            var data = await context.ContactForms.Include(q => q.ContactFormAttachments).FirstOrDefaultAsync(f => f.Id == id);
            var model = new ContactFormDto
            {
                Id = data.Id,
                Email = data.Email,
                FullName = data.FullName,
                PhoneNumber = data.PhoneNumber,
                Remark = data.Remark,
                Subject = data.Subject,
                File = data.ContactFormAttachments.Select(s => new FileItemDto { Entity = "ContactFormAttachments", FilePath = s.FilePath }).ToList(),

            };
            data.IsVisited = true;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<ContactFormDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<ContactFormDto>>();

            var queryResult = await _queryService.QueryAsync(model, null, new List<string>() { "ContactFormAttachments" });

            result.Data = queryResult.Data.Select(q => new ContactFormDto
            {
                Id = q.Id,
                Email = q.Email,
                FullName = q.FullName,
                PhoneNumber = q.PhoneNumber,
                Remark = q.Remark,
                Subject = q.Subject,
                IsVisited = q.IsVisited,
                CreateDate = DateUtility.ToShamsi(q.CreateDate),

                File = q.ContactFormAttachments.Select(s => new FileItemDto { Entity = "ContactFormAttachments", FilePath = s.FilePath }).ToList(),
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
