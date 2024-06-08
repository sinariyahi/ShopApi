using Infrastructure.Common;
using Infrastructure.Models.EIED;
using Infrastructure.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IProjectService
    {
        Task<ShopActionResult<List<ComboItemDto>>> GetForCombo();
        Task<ShopActionResult<List<ProjectDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(ProjectDto model);
        Task<ShopActionResult<int>> Update(ProjectDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<ProjectDto>> GetById(int id);
    }
}
