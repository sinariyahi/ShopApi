using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Support
{
    [Table("CartableRecords", Schema = "Support")]

    public class CartableRecord
    {
        public Guid Id { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        public CartableType? CartableType { get; set; }
        public int CartableId { get; set; }
        public virtual Cartable Cartable { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        [MaxLength(512)]
        public string Url { get; set; }
        [MaxLength(64)]
        public string RecordId { get; set; }
        public bool Visited { get; set; } = false;
        public DateTime? VisitedDate { get; set; }
    }
}
