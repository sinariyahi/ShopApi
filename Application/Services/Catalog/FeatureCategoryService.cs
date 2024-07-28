using Application.Interfaces;
using Domain.Entities.Catalog;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;
using Application.Interfaces.Catalog;

namespace Application.Services.Catalog
{
    public class FeatureCategoryService : IFeatureCategoryService
    {
        private readonly BIContext context;
    private readonly IGenericQueryService<FeatureCategory> _queryService;

    public FeatureCategoryService(BIContext context, IGenericQueryService<FeatureCategory> queryService
)
    {
        this.context = context;
        _queryService = queryService;
    }

    public async Task<ShopActionResult<int>> Add(FeatureCategoryDto model)
    {
        var result = new ShopActionResult<int>();
        var data = new FeatureCategory
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

        var item = new FeatureCategory { Id = id };
        context.Remove(item);
        await context.SaveChangesAsync();

        result.IsSuccess = true;
        //result.Message = MessagesFA.DeleteSuccessful;
        return result;
    }

    public async Task<ShopActionResult<FeatureCategoryDto>> GetById(int id)
    {
        var result = new ShopActionResult<FeatureCategoryDto>();

        var data = await context.FeatureCategories.FindAsync(id);
        var model = new FeatureCategoryDto
        {
            Id = data.Id,
            IsActive = data.IsActive,
            Title = data.Title,
        };

        result.IsSuccess = true;
        result.Data = model;
        return result;
    }

    public async Task<ShopActionResult<List<FeatureCategoryDto>>> GetList(GridQueryModel model = null)
    {
        var result = new ShopActionResult<List<FeatureCategoryDto>>();

        var queryResult = await _queryService.QueryAsync(model);

        result.Data = queryResult.Data.Select(q => new FeatureCategoryDto
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

    public async Task<ShopActionResult<int>> Update(FeatureCategoryDto model)
    {
        var result = new ShopActionResult<int>();

        var data = await context.FeatureCategories.FindAsync(model.Id);
        data.Title = model.Title;
        data.IsActive = model.IsActive;

        await context.SaveChangesAsync();

        result.IsSuccess = true;
        //result.Message = MessagesFA.UpdateSuccessful;
        return result;
    }
}
}
