using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class CurrencyService : ICurrencyService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Currency> _queryService;

        public CurrencyService(BIContext context, IGenericQueryService<Currency> queryService
 )
        {
            this.context = context;
            _queryService = queryService;
        }

        public async Task<ShopActionResult<int>> Add(CurrencyDto model)
        {
            var result = new ShopActionResult<int>();
            var data = new Currency
            {
                IsActive = model.IsActive,
                Title = model.Title,
                Symbol = model.Symbol,
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

            var item = new Currency { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<CurrencyDto>> GetById(int id)
        {
            var result = new ShopActionResult<CurrencyDto>();

            var data = await context.Currencies.FindAsync(id);
            var model = new CurrencyDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                Symbol = data.Symbol,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<CurrencyDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<CurrencyDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new CurrencyDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                Symbol = q.Symbol,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<List<ComboItemDto>> GetForCombo()
        {
            var result = new List<ComboItemDto>();

            result = context.Currencies.Where(q => q.IsActive).Select(q => new ComboItemDto
            {
                Text = q.Title,
                Value = q.Id
            }).ToList();
            return result;
        }

        public async Task<ShopActionResult<int>> Update(CurrencyDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.Currencies.FindAsync(model.Id);
            data.Title = model.Title;
            data.Symbol = model.Symbol;
            data.IsActive = model.IsActive;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
