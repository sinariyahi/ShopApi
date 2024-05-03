using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Shop
{
    public class KeyValueDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }

        public string Title { get; set; }
    }

    public class ParishItemDto
    {
        public int Id { get; set; }
        public string ParishName { get; set; }
        public int Value { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

    }
}
