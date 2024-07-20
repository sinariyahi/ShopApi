using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class JobOpportunityService : IJobOpportunityService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<JobOpportunity> _queryService;

        public JobOpportunityService(BIContext context, IGenericQueryService<JobOpportunity> queryService
 )
        {
            this.context = context;
            _queryService = queryService;
        }

        public async Task<ShopActionResult<int>> Add(JobOpportunityDto model)
        {
            var result = new ShopActionResult<int>();
            var data = new JobOpportunity
            {
                IsActive = model.IsActive,
                Title = model.Title,
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new JobOpportunity { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<JobOpportunityDto>> GetById(int id)
        {
            var result = new ShopActionResult<JobOpportunityDto>();

            var data = await context.JobOpportunities.FindAsync(id);
            var model = new JobOpportunityDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<JobOpportunityDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<JobOpportunityDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new JobOpportunityDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال"
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(JobOpportunityDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.JobOpportunities.FindAsync(model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
