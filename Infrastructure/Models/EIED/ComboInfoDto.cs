using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.EIED
{
    public class ComboInfoDto
    {
        public List<ComboItemDto> OrganizationUnits { get; set; }
        public List<ComboItemDto> ResponsiblePersonDataCollections { get; set; }
        public List<ComboItemDto> Discpilines { get; set; }
        public List<ComboItemDto> OrganizationLevel { get; set; }
        public List<ComboItemDto> Projects { get; set; }
        public List<ComboItemDto> Roles { get; set; }
        public List<EIEDEnums> Years { get; set; }
        public List<EIEDEnums> Months { get; set; }
        public List<EIEDEnums> Operators { get; set; }
        public List<EIEDEnums> TimeDurations { get; set; }
        public List<EIEDEnums> UserTypes { get; set; }
        public List<EIEDEnums> CalcTypes { get; set; }
        public ApplicationConfigDto ApplicationConfig { get; set; }
        public List<EIEDEnums> EquipmentTypes { get; set; }
        public List<EIEDEnums> PersonnelPositions { get; set; }
        public List<ComboItemDto> CompanyActicityTypes { get; set; }
        public List<EIEDEnums> UnitTypes { get; set; }
        public List<EIEDEnums> ControlTypes { get; set; }
    }

    public class ComboItemDto
    {
        public string EnTitle { get; set; }
        public object Value { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public object ParentId { get; set; }
        public string ParentTitle { get; set; }
    }

    public class IndexTemplateComboItemDto : ComboItemDto
    {
        public List<EIEDEnums> ShowTypes { get; set; }
        public ShowType DefaultShowType { get; set; }
    }
}
