using Application.Interfaces.Base;
using Application.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models.EIED;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Base;

namespace Application.Services.Base
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<EmailTemplate> _queryService;
        private readonly Configs configs;

        public EmailTemplateService(BIContext context, IOptions<Configs> options, IGenericQueryService<EmailTemplate> queryService
 )
        {
            this.context = context;
            _queryService = queryService;
            this.configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(EmailTemplateDto model)
        {
            var result = new ShopActionResult<int>();
            var data = new EmailTemplate
            {
                IsActive = model.IsActive,
                Title = model.Title,
                CreateDate = DateTime.Now,
                EmailTemplateType = model.EmailTemplateType,
                RegisterUserId = model.RegisterUserId,
                Template = model.Template,
                EmailCC = model.EmailCC,
                ProjectId = model.ProjectId,
                DefaultTemplate = model.Template,
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           
            
           // result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new EmailTemplate { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<EmailTemplateDto>> GetById(int id)
        {
            var result = new ShopActionResult<EmailTemplateDto>();

            var data = await context.EmailTemplates.FindAsync(id);
            var model = new EmailTemplateDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                CreateDate = data.CreateDate,
                EmailTemplateType = data.EmailTemplateType,
                Template = data.Template,
                RegisterUserId = data.RegisterUserId,
                EmailCC = data.EmailCC,
                ProjectId = data.ProjectId,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<bool>> ResetTemplate(int id)
        {
            var result = new ShopActionResult<bool>();

            var data = await context.EmailTemplates.FindAsync(id);
            data.Template = data.DefaultTemplate;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.ResetEmailTemplate;
            return result;
        }

        public async Task<ShopActionResult<List<EmailTemplateDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<EmailTemplateDto>>();

            var queryResult = await _queryService.QueryAsync(model, null, new List<string> { "Project" });

            result.Data = queryResult.Data.Select(q => new EmailTemplateDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                CreateDate = q.CreateDate,
                EmailTemplateType = q.EmailTemplateType,
                EmailTemplateTypeName = Enum.GetName(q.EmailTemplateType),
                RegisterUserId = q.RegisterUserId,
                ProjectId = q.ProjectId,
                ProjectName = q.ProjectId != null ? q.Project.Title : String.Empty,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(EmailTemplateDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.EmailTemplates.FindAsync(model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.Template = model.Template;
            data.EmailTemplateType = model.EmailTemplateType;
            data.EmailCC = model.EmailCC;
            data.ProjectId = model.ProjectId;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

        public async Task<List<ComboItemDto>> GetTemplateTypesForCombo()
        {
            var list = new List<ComboItemDto>();
            var values = Enum.GetValues(typeof(EmailTemplateType)).Cast<EmailTemplateType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute(item).ToString(),
                };
                list.Add(model);
            }
            return list;
        }

        public async Task<string> GenerateEmailTemplate(EmailTemplateType emailTemplateType, EmailSenderInfoDto emailSenderInfo, params string[] values)
        {
            var footerTemplate = await context.EmailTemplates.FirstOrDefaultAsync(q => q.EmailTemplateType == EmailTemplateType.FooterEmailTemplate);
            var footer = footerTemplate.Template;
            footer = footer.Replace("SenderName", emailSenderInfo.SenderName);
            footer = footer.Replace("SenderEmail", emailSenderInfo.SenderEmail);
            footer = footer.Replace("SenderRole", emailSenderInfo.SenderRole);
            footer = footer.Replace("Website", configs.WebSiteAddress);

            var emailTemplate = await context.EmailTemplates.FirstOrDefaultAsync(q => q.EmailTemplateType == emailTemplateType);
            var emailBody = string.Format(emailTemplate.Template, values);
            return emailBody + footer;
        }

        public async Task<string> GenerateEmailTemplateWithMessageBody(EmailSenderInfoDto emailSenderInfo, string messageBody)
        {
            var footerTemplate = await context.EmailTemplates.FirstOrDefaultAsync(q => q.EmailTemplateType == EmailTemplateType.FooterEmailTemplate);
            var footer = footerTemplate.Template;
            footer = footer.Replace("SenderName", emailSenderInfo.SenderName);
            footer = footer.Replace("SenderEmail", emailSenderInfo.SenderEmail);
            footer = footer.Replace("SenderRole", emailSenderInfo.SenderRole);
            footer = footer.Replace("WebsiteUrl", emailSenderInfo.Website);
            footer = footer.Replace("ReceiverEmail", emailSenderInfo.ReceiverEmail);

            return messageBody + footer;
        }
    }
}
