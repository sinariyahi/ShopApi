using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IEmailTemplateService
    {
        Task<ShopActionResult<List<EmailTemplateDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(EmailTemplateDto model);
        Task<ShopActionResult<int>> Update(EmailTemplateDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<EmailTemplateDto>> GetById(int id);
        Task<List<ComboItemDto>> GetTemplateTypesForCombo();
        Task<string> GenerateEmailTemplate(EmailTemplateType emailTemplateType, EmailSenderInfoDto emailSenderInfo, params string[] values);
        Task<string> GenerateEmailTemplateWithMessageBody(EmailSenderInfoDto emailSenderInfo, string messageBody);
        Task<ShopActionResult<bool>> ResetTemplate(int id);
    }
}
