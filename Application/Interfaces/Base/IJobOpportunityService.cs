using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IJobOpportunityService
    {
        Task<ShopActionResult<List<JobOpportunityDto>>> GetList(GridQueryModel model = null);
        Task<ShopActionResult<int>> Add(JobOpportunityDto model);
        Task<ShopActionResult<int>> Update(JobOpportunityDto model);
        Task<ShopActionResult<int>> Delete(int id);
        Task<ShopActionResult<JobOpportunityDto>> GetById(int id);
    }
}
