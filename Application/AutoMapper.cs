using AutoMapper;
using Domain.Entities.Base;
using Infrastructure.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
