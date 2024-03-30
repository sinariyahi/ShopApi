using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class StepModel
    {
        public object CurrentStep { get; set; }

        public List<StepDto> ListSteps { get; set; }
    }

    public class StepDto
    {
        public object Value { get; set; }

        public string Status { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

    }

}
