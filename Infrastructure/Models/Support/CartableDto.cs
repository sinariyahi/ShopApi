using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Support
{
    public class CartableDto
    {
        public int CartableId { get; set; }
        public string Title { get; set; }
        public string EnTitle { get; set; }
        public List<CartableRecordDto> CartableRecords { get; set; } = new List<CartableRecordDto>();
    }
    public class CartableRecordDto
    {
        public Guid Id { get; set; }
        public string RecordId { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public string SendDate { get; set; }
        public string SenderUser { get; set; }
        public bool Visited { get; set; }
        public string VisitedTitle { get; set; }
        public string VisitedDate { get; set; }
    }

    public class CartableRecordInsertModel
    {
        public string Title { get; set; }
        public string RecordId { get; set; }
        public Guid SenderUserId { get; set; }
        public CartableType CartableType { get; set; }
    }
}
