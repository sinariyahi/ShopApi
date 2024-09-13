using Application.Interfaces.Base;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.BaseInfo.EmailTemplate
{
    public class EmailTemplateController : BaseController
    {
        private readonly IEmailTemplateService _service;
        public EmailTemplateController(IEmailTemplateService service)
        {
            _service = service;
        }


        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
            var result = await _service.GetList(model);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        [HttpGet("ResetTemplate/{id}")]
        public async Task<IActionResult> ResetTemplate(int id)
        {
            var result = await _service.ResetTemplate(id);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(EmailTemplateDto model)
        {
            model.RegisterUserId = UserId;
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPost("DuplicateRow")]
        public async Task<IActionResult> DuplicateRow(EmailTemplateDto model)
        {
            model.RegisterUserId = UserId;
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(EmailTemplateDto model)
        {
            model.RegisterUserId = UserId;
            var result = await _service.Update(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetForCombo")]
        public async Task<IActionResult> GetForCombo()
        {
            var result = await _service.GetTemplateTypesForCombo();
            return Ok(result);
        }
    }
}
