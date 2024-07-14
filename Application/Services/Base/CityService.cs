using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Base
{
    public class CityService : ICityService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<City> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public CityService(BIContext context, IGenericQueryService<City> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(CityInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new City
            {
                IsActive = model.IsActive,
                Title = model.Title,
                ProvinceId = model.ProvinceId,

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

            var item = new City { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }



        public async Task<ShopActionResult<CityDto>> GetById(int id)
        {
            var result = new ShopActionResult<CityDto>();

            var data = await context.Cities.FirstOrDefaultAsync(f => f.Id == id);
            var model = new CityDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                ProvinceId = data.ProvinceId,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }


        public async Task<ShopActionResult<List<CityDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<CityDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new CityDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                ProvinceTitle = EnumHelpers.GetNameAttribute<Province>(q.ProvinceId),

            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }



        public async Task<ShopActionResult<int>> Update(CityInputModel model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.Cities.FirstOrDefaultAsync(f => f.Id == model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.ProvinceId = model.ProvinceId;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = Messages.UpdateSuccessful;
            return result;
        }

    }
}
