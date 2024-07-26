using Application.Interfaces;
using Application.Interfaces.Catalog;
using Domain;
using Domain.Entities.Catalog;
using Infrastructure.Common;
using Infrastructure.Models;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Catalog
{
    public class BrandService : IBrandService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Brand> _queryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public BrandService(BIContext context, IGenericQueryService<Brand> queryService,
             IFileService fileService, IOptions<Configs> options)
        {
            this.context = context;
            _queryService = queryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(BrandInputModel model)
        {
            var result = new ShopActionResult<int>();


            var data = new Brand
            {
                IsActive = model.IsActive,
                Title = model.Title,
                Description = model.Description,
                SortOrder = model.SortOrder,
                CreateDate = DateTime.Now,
                EnTitle = model.EnTitle,
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "BrandAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Catalog", "BrandAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new Brand { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }





        public async Task<ShopActionResult<List<TreeDto>>> GetBrandMegaMenu()
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var items = new List<TreeDto>();
            var brands = await context.Brands.Where(q => q.IsActive).OrderBy(q => q.SortOrder).Take(10).Select(q => new { q.Id, q.Title, q.EnTitle }).ToListAsync();

            foreach (var brand in brands)
            {
                var model = new TreeDto
                {
                    EnTitle = brand.EnTitle,
                    Title = brand.Title,
                    Key = brand.Id,
                    Children = await GetCategoriesWithBrand(brand.Id),
                };
                //if (model.Children.Count > 0)
                //{
                //    items.Add(model);

                //}
                items.Add(model);

            }
            var obj = new TreeDto
            {
                EnTitle = "",
                Title = "همه برندها",
                Key = -1,

            };

            items.Add(obj);

            result.Data = items;
            result.IsSuccess = true;
            return result;
        }


        public async Task<ShopActionResult<List<TreeDto>>> GetAllBrandMegaMenu()
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var items = new List<TreeDto>();
            var brands = await context.Brands.Where(q => q.IsActive).OrderBy(q => q.SortOrder).Select(q => new { q.Id, q.Title, q.EnTitle }).ToListAsync();

            foreach (var brand in brands)
            {
                var model = new TreeDto
                {
                    EnTitle = brand.EnTitle,
                    Title = brand.Title,
                    Key = brand.Id,
                    Children = await GetCategoriesWithBrand(brand.Id),
                };
                //if (model.Children.Count > 0)
                //{
                //    items.Add(model);

                //}
                items.Add(model);

            }
            //var obj = new TreeDto
            //{
            //    EnTitle = "",
            //    Title = "همه برندها",
            //    Key = -1,

            //};

            //items.Add(obj);

            result.Data = items;
            result.IsSuccess = true;
            return result;
        }




        private async Task<List<TreeDto>> GetCategoriesWithBrand(int brandId)
        {
            var categoryIds = await context.Products.Where(w => w.BrandId == brandId && w.IsActive == true).Select(s => s.CategoryId).Distinct().ToListAsync();
            var categories = await context.ProductCategories.Where(q => q.IsActive && categoryIds.Contains(q.Id))
                .Select(q => new { q.Id, q.CategoryName, q.ParentId, q.MainFeatureId, q.EnName }).ToListAsync();

            var children = new List<TreeDto>();
            foreach (var item in categories)
            {
                var model = new TreeDto
                {
                    EnTitle = item.EnName,
                    Title = item.CategoryName,
                    Key = item.Id,
                    Children = await GetSubCategory(item.Id, brandId, item.ParentId, item.MainFeatureId),
                };
                //if (model.Children.Count > 0)
                //{
                //    children.Add(model);

                //}

                if (children.Any(a => a.Title == model.Title))
                {
                    var childItem = children.FirstOrDefault(a => a.Title == model.Title);
                    childItem.Children.AddRange(model.Children);
                }
                else
                {
                    children.Add(model);

                }



            }

            return children;
        }


        public async Task<ShopActionResult<List<BrandForDashboardChart>>> GetBrandProductsForDashboard()
        {
            var result = new ShopActionResult<List<BrandForDashboardChart>>();
            var brands = await context.Brands.Include(q => q.Products).ToListAsync();

            var data = brands.Select(q => new BrandForDashboardChart { X = q.Title, Y = q.Products.Count() })
                .Where(q => q.Y != 0).ToList();

            result.IsSuccess = true;
            result.Data = data.OrderByDescending(o => o.Y).ToList();

            return result;
        }




        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserProductCategoryByBrandId(int brandId)
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();

            var dataList = new List<UserProductCategoryDto>();

            var categoryIds = await context.Products.Where(w => w.BrandId == brandId).Select(s => s.CategoryId).Distinct().ToListAsync();
            var categories = await context.ProductCategories.Include(i => i.CategoryAttachments).Where(q => q.IsActive && categoryIds.Contains(q.Id)).ToListAsync();

            foreach (var item in categories)
            {
                var obj = new UserProductCategoryDto()
                {

                    ParentId = item.ParentId,
                    Id = item.Id,
                    CategoryName = item.CategoryName,
                    File = item.CategoryAttachments.Count() > 0 ? item.CategoryAttachments.FirstOrDefault().FilePath : "",
                    SortOrder = item.SortOrder,
                    Remark = item.Remark,
                    //Children=await GetBrandsForCategory(item.Id)
                };
                dataList.Add(obj);

            }


            result.Data = dataList;
            result.IsSuccess = true;

            return result;
        }


        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserProductCategoryByBrandTitle(string brand)
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();

            var dataList = new List<UserProductCategoryDto>();

            brand = DataUtility.RemoveDashForTitle(brand);

            var brandItem = await context.Brands.FirstOrDefaultAsync(f => f.EnTitle == brand || f.Title == brand);

            var categoryIds = await context.Products.Where(w => w.BrandId == brandItem.Id && w.IsActive == true).Select(s => s.CategoryId).Distinct().ToListAsync();
            var categories = await context.ProductCategories.Include(i => i.CategoryAttachments).Where(q => q.IsActive && categoryIds.Contains(q.Id)).ToListAsync();

            foreach (var item in categories)
            {
                var obj = new UserProductCategoryDto()
                {

                    ParentId = item.ParentId,
                    Id = item.Id,
                    CategoryName = item.CategoryName,
                    File = item.CategoryAttachments.Count() > 0 ? item.CategoryAttachments.FirstOrDefault().FilePath : "",
                    SortOrder = item.SortOrder,
                    Remark = item.Remark,
                    //Children = await GetSubCategories(item.ParentId)
                };
                dataList.Add(obj);

            }


            result.Data = dataList;
            result.IsSuccess = true;

            return result;
        }


        private async Task<List<TreeDto>> GetSubCategories(int? parentId)
        {
            var childRoles = await context.ProductCategories.Where(q => q.Id == parentId && q.IsActive).Select(q => new { q.Id, q.CategoryName, q.EnName }).Distinct().ToListAsync();
            var children = new List<TreeDto>();
            foreach (var item in childRoles)
            {
                var child = new TreeDto
                {
                    Key = item.Id,
                    Title = item.CategoryName,
                    Value = item.Id,
                    Text = item.CategoryName,
                    EnTitle = item.EnName,
                };

                children.Add(child);
            }

            return children;
        }


        private async Task<List<TreeDto>> GetSubCategory(int categoryId, int brandId, int? parentId, int? mainFeatureId)
        {
            var childRoles = await context.ProductCategories.Where(q => q.Id == parentId && q.IsActive).Select(q => new { q.Id, q.CategoryName, q.EnName }).Distinct().ToListAsync();
            var children = new List<TreeDto>();
            foreach (var item in childRoles)
            {
                var child = new TreeDto
                {
                    EnTitle = item.EnName,
                    Key = item.Id,
                    Title = item.CategoryName,
                    Value = item.Id,
                    Text = item.CategoryName,
                    Children = await GetFeatureCategoryProducts(categoryId, brandId, mainFeatureId)
                };

                children.Add(child);
            }

            return children;
        }

        private async Task<List<TreeDto>> GetFeatureCategoryProducts(int? categoryId, int brandId, int? mainFeatureId)
        {
            var childRoles = await context.Products.Include(i => i.Category).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature.Symbol)
                .Where(q => q.CategoryId == categoryId && q.BrandId == brandId && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.EnName, q.ProductName }).ToListAsync();

            var children = new List<TreeDto>();
            foreach (var item in childRoles.DistinctBy(d => d.Id).ToList())
            {
                if (mainFeatureId != null && mainFeatureId != 0)
                {
                    var child = new TreeDto
                    {
                        Key = item.Id,
                        Value = item.Id,
                        Title = item.ProductName,
                        Text = item.FeatureValues.Count > 0 ? item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.FeatureValue : "",
                        EnTitle = item.EnName,
                        SymbolTitle = item.FeatureValues.Count > 0 ? item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.ProductCategoryFeature?.Feature?.Symbol?.Title : "",

                    };

                    if (item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.FeatureValue != null && item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.FeatureValue != "")
                    {
                        children.Add(child);

                    }

                }

            }

            return children.DistinctBy(d => d.Value).DistinctBy(b => b.Text).ToList();
        }


        private async Task<List<TreeDto>> GetProducts(int categoryId, int brandId)
        {
            var childRoles = await context.Products.Where(q => q.BrandId == brandId && q.CategoryId == categoryId && q.IsActive).OrderBy(q => q.Id).Take(10).Select(q => new { q.Id, q.ProductName }).ToListAsync();
            var children = new List<TreeDto>();
            foreach (var item in childRoles)
            {
                var child = new TreeDto
                {
                    Key = item.Id,
                    Title = item.ProductName,
                    Value = item.Id,
                    Text = item.ProductName,
                };

                children.Add(child);
            }

            return children;
        }




        public async Task<ShopActionResult<BrandDto>> GetById(int id)
        {
            var result = new ShopActionResult<BrandDto>();

            var data = await context.Brands.Include(q => q.BrandAttachments).FirstOrDefaultAsync(f => f.Id == id);
            var model = new BrandDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                EnTitle = data.EnTitle,
                Description = data.Description == null ? "" : data.Description,
                File = data.BrandAttachments.Select(s => new FileItemDto { Entity = "BrandAttachments", FilePath = s.FilePath }).ToList(),
                SortOrder = data.SortOrder,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<BrandDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<BrandDto>>();

            var queryResult = await _queryService.QueryAsync(model, null, new List<string>() { "BrandAttachments" });

            result.Data = queryResult.Data.Select(q => new BrandDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                Description = q.Description == null ? "" : q.Description,
                File = q.BrandAttachments.Select(s => new FileItemDto { Entity = "BannerAttachments", FilePath = s.FilePath }).ToList(),
                SortOrder = q.SortOrder,
                EnTitle = q.EnTitle,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<UserBrandDto>>> GetUserList(int count = 8)
        {
            var result = new ShopActionResult<List<UserBrandDto>>();


            result.Data = await context.Brands.Include(i => i.BrandAttachments).Where(w => w.IsActive == true).Select(q => new UserBrandDto
            {
                Id = q.Id,
                Title = q.Title,
                EnTitle = q.EnTitle,
                Description = q.Description == null ? "" : q.Description,
                File = q.BrandAttachments.FirstOrDefault().FilePath,
                SortOrder = q.SortOrder,
            }).Take(count).OrderBy(o => o.SortOrder).ThenBy(o => o.Title).ToListAsync();
            result.IsSuccess = true;

            return result;

        }


        public async Task<ShopActionResult<List<UserBrandDto>>> GetUserListByProductCategoryId(string category)
        {
            var result = new ShopActionResult<List<UserBrandDto>>();

            category = DataUtility.RemoveDashForTitle(category);
            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);

            if (categoryItem != null)
            {
                var data = await context.Products.Include(i => i.Brand).Include(i => i.Brand.BrandAttachments)
                    .Where(w => w.CategoryId == categoryItem.Id && w.Brand.IsActive == true)
                    .Select(q => new UserBrandDto
                    {
                        Id = q.BrandId,
                        Title = q.Brand.Title,
                        EnTitle = q.Brand.EnTitle,
                        Description = q.Brand.Description == null ? "" : q.Brand.Description,
                        File = q.Brand.BrandAttachments.FirstOrDefault().FilePath,
                    }).ToListAsync();

                result.Data = data.DistinctBy(d => d.Id).ToList();
            }

            result.IsSuccess = true;

            return result;

        }


        public async Task<ShopActionResult<int>> Update(BrandInputModel model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.Brands.Include(q => q.BrandAttachments).FirstOrDefaultAsync(f => f.Id == model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.Description = model.Description;
            data.SortOrder = model.SortOrder;
            data.EnTitle = model.EnTitle;
            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "BrandAttachments");

                //foreach (var item in data.BrandAttachments)
                //{
                //    context.BrandAttachments.Remove(item);
                //}
                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Catalog", "BrandAttachments", null, null, data.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }


            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

    }
}
