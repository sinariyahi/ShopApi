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
    public class OriginService : IOriginService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Origin> _queryService;

        public OriginService(BIContext context, IGenericQueryService<Origin> queryService
 )
        {
            this.context = context;
            _queryService = queryService;
        }

        public async Task<ShopActionResult<int>> Add(OriginDto model)
        {
            var result = new ShopActionResult<int>();
            var data = new Origin
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

            var item = new Origin { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<OriginDto>> GetById(int id)
        {
            var result = new ShopActionResult<OriginDto>();

            var data = await context.Origins.FindAsync(id);
            var model = new OriginDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<OriginDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<OriginDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new OriginDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "Active" : "InActive"
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(OriginDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.Origins.FindAsync(model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
