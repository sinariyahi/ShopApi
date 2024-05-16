using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Support
{
    public class CompanyQuestionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public CompanyQuestionStatus CompanyQuestionStatus { get; set; }
        public int SenderCompanyId { get; set; }
        public IFormFile AttachmentFile { get; set; }
        public string AttachmentFileBase64 { get; set; }
        public string CreateDateStr { get; set; }
        public DateTime CreateDate { get; set; }

        public string CompanyQuestionStatusTitle { get; set; }
        public QuesitonAnswerDto Answer { get; set; } = new QuesitonAnswerDto();
        public string CompanyName { get; set; }
        public string UserName { get; set; }
    }

    public class QuesitonFilterDto : GridQueryModel
    {
        public string Title { get; set; }

        public CompanyQuestionStatus? CompanyQuestionStatus { get; set; }
    }

    public class QuesitonAnswerDto
    {
        public Guid Id { get; set; }
        public Guid UserQuestionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IFormFile AttachmentFile { get; set; }
        public string AttachmentFileBase64 { get; set; }
    }


    public class CompanyAnswerQuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionTitle { get; set; }
        public string AnswerTitle { get; set; }

        public string Body { get; set; }
        public CompanyQuestionStatus CompanyQuestionStatus { get; set; }
        public int SenderCompanyId { get; set; }
        public IFormFile AttachmentFile { get; set; }
        public string AttachmentFileBase64 { get; set; }
        public string QuestionCreateDate { get; set; }
        public string CompanyQuestionStatusTitle { get; set; }

        public string AnswerCreateDate { get; set; }

    }
}
