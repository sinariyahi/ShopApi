using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Security
{
    public class UserOpinionDto
    {
        public Guid Id { get; set; }
        public Guid SenderUserId { get; set; }
        public string SenderUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ShamsiCreateDate { get; set; }
        public UserOpinionType UserOpinionType { get; set; }
        public int? ArticleId { get; set; }

        public ShowStatus ShowStatus { get; set; }
        public string Remark { get; set; }
        public int? ProductId { get; set; }
        public List<UserOpinionModel> NegativeOpinions { get; set; }
        public List<UserOpinionModel> PositiveOpinions { get; set; }

    }

    public class FilterUserOpinionDto : GridQueryModel
    {
        public string ProductTitle { get; set; }

        public UserOpinionType? UseOpinionType { get; set; }
        public ShowStatus? ShowStatus { get; set; }

    }




    public class ListUserOpinionDto
    {
        public Guid Id { get; set; }
        public string UserOpinionTypeTitle { get; set; }
        public Guid SenderUserId { get; set; }
        public string SenderUser { get; set; }
        public string CreateDate { get; set; }
        public string Status { get; set; }
        public string ProductTitle { get; set; }
        public string ArticleTitle { get; set; }
        public string Title { get; set; }

        public string Remark { get; set; }
        public int? ProductId { get; set; }
        public List<UserOpinionModel> NegativeOpinions { get; set; }
        public List<UserOpinionModel> PositiveOpinions { get; set; }

    }






    public class UserOpinionDetailDto
    {


        public string Text { get; set; }


    }



    public class UserInputOpinionModel
    {
        public Guid Id { get; set; }
        public ShowStatus ShowStatus { get; set; }

    }


    public class UserOpinionModel
    {
        public UseOpinionStatus UseOpinionStatus { get; set; }
        public UserOpinionType UseOpinionType { get; set; }

        public string Text { get; set; }

    }
}
