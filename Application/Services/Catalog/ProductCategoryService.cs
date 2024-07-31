using Application.Interfaces.Catalog;
using Application.Interfaces;
using Domain.Entities.Catalog;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.User;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Catalog
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Category> productCategoryQueryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public ProductCategoryService(BIContext context,
            IGenericQueryService<Category> productCategoryQueryService, IFileService fileService, IOptions<Configs> options
            )
        {
            this.context = context;
            this.productCategoryQueryService = productCategoryQueryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }


        public async Task<ShopActionResult<int>> Add(ProductCategoryDto model)
        {
            var result = new ShopActionResult<int>();
            var item = new Category
            {
                CategoryName = model.CategoryName,
                Code = model.Code,
                IsActive = model.IsActive,
                ParentId = model.ParentId != 0 ? model.ParentId : null,
                CreateDate = DateTime.Now,
                SortOrder = model.SortOrder,
                MainFeatureId = model.MainFeatureId,
                Remark = model.Remark,
                EnName = model.EnName,
            };

            model.Features.DistinctBy(d => d.Id).ToList().ForEach(q =>
                  item.Features.Add(new CategoryFeature
                  {
                      FeatureId = q.Id,
                      SortOrder = 1
                  }));

            ////Add New MainFeatures
            //model.MainFeatures.ForEach(q =>
            //{
            //    if (!item.CategoryMainFeatures.Any(f => f.FeatureId == q))
            //    {
            //        item.CategoryMainFeatures.Add(new CategoryMainFeature
            //        {
            //            FeatureId = q,
            //            SortOrder = 1,
            //        });
            //    }
            //});



            await context.AddAsync(item);
            await context.SaveChangesAsync();


            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "CategoryAttachments");

                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Catalog", "CategoryAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }




            if (model.MainFeatureFile != null && item.MainFeatureId != null && item.MainFeatureId != 0)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "MainFeatureAttachments");

                for (int i = 0; i < model.MainFeatureFile.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.MainFeatureFile[i], "Catalog", "MainFeatureAttachments", null, item.MainFeatureId, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }




            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new Category { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<ProductCategoryDto>> GetById(int id)
        {
            var result = new ShopActionResult<ProductCategoryDto>();

            var item = await context.ProductCategories.Include(q => q.Features).Include(q => q.CategoryMainFeatures).Include(i => i.CategoryAttachments)
                .Include(i => i.MainFeatureAttachments)
                .SingleOrDefaultAsync(q => q.Id == id);
            var features = new List<ProductCategoryFeatureDto>();
            //var mainFeatures = new List<int>();

            foreach (var obj in item.Features)
            {
                var featuresItem = new ProductCategoryFeatureDto
                {
                    Id = obj.FeatureId,
                    Title = context.Features.FirstOrDefault(s => s.Id == obj.FeatureId)?.Title,
                };
                features.Add(featuresItem);
            }
            //foreach (var obj in item.CategoryMainFeatures.Select(s => s.Id).ToList())
            //{

            //    mainFeatures.Add(obj);
            //}


            var model = new ProductCategoryDto
            {
                Id = item.Id,
                CategoryName = item.CategoryName,
                Code = item.Code,
                IsActive = item.IsActive,
                ParentId = item.ParentId,
                Features = features.DistinctBy(d => d.Id).ToList(),
                FeatureCount = features.DistinctBy(d => d.Id).Count(),
                EnName = item.EnName,
                //MainFeatures = mainFeatures,
                MainFeatureId = item.MainFeatureId,
                FileAttachment = item.CategoryAttachments.Select(s => new FileItemDto { Entity = "CategoryAttachments", FilePath = s.FilePath }).ToList(),
                MainFeatureFileAttachment = item.MainFeatureAttachments.Select(s => new FileItemDto { Entity = "MainFeatureAttachments", FilePath = s.FilePath }).ToList(),
                SortOrder = item.SortOrder,
                Remark = item.Remark,

            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<ProductCategoryDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<ProductCategoryDto>>();

            var queryResult = await productCategoryQueryService.QueryAsync(model, includes: new List<string> { "Features" });

            result.Data = queryResult.Data.Select(q => new ProductCategoryDto
            {
                Id = q.Id,
                CategoryName = q.CategoryName,
                Code = q.Code,
                IsActive = q.IsActive,
                ParentId = q.ParentId,
                ParentName = q.Parent != null ? q.Parent.CategoryName : string.Empty,
                FeatureCount = q.Features.Count(),
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }


        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserList(int count = 8)
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();


            result.Data = await context.ProductCategories.Include(i => i.CategoryAttachments).Where(w => w.ParentId == null && w.IsActive == true).Select(q => new UserProductCategoryDto
            {
                ParentId = q.ParentId,
                Id = q.Id,
                CategoryName = q.CategoryName,
                File = q.CategoryAttachments.Count() > 0 ? q.CategoryAttachments.FirstOrDefault().FilePath : "",
                SortOrder = q.SortOrder,
                EnName = !string.IsNullOrEmpty(q.EnName) ? q.EnName : q.CategoryName,
            }).Take(count).OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).ToListAsync();



            result.IsSuccess = true;

            return result;
        }




        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserByParentId(int count = 8, string category = "")
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();
            category = DataUtility.RemoveDashForTitle(category);

            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);
            if (categoryItem != null)
            {
                result.Data = await context.ProductCategories.Include(i => i.CategoryAttachments).Where(w => w.ParentId == categoryItem.Id && w.IsActive == true).Select(q => new UserProductCategoryDto
                {
                    ParentId = q.ParentId,
                    Id = q.Id,
                    CategoryName = q.CategoryName,
                    File = q.CategoryAttachments.Count() > 0 ? q.CategoryAttachments.FirstOrDefault().FilePath : context.CategoryAttachments.FirstOrDefault(f => f.CategoryId == q.ParentId) != null ?
                    context.CategoryAttachments.FirstOrDefault(f => f.CategoryId == q.ParentId).FilePath : "",
                    SortOrder = q.SortOrder,
                    Remark = q.Remark,
                    EnName = q.EnName

                }).Take(count).OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).ToListAsync();

            }


            result.IsSuccess = true;

            return result;
        }


        public async Task<ShopActionResult<List<TreeDto>>> GetCategoryByParentId(string category)
        {
            category = DataUtility.RemoveDashForTitle(category);
            var result = new ShopActionResult<List<TreeDto>>();
            var list = new List<TreeDto>();


            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);

            if (categoryItem != null)
            {
                var data = await context.ProductCategories.Where(w => w.ParentId == categoryItem.Id && w.IsActive == true).ToListAsync();
                foreach (var q in data)
                {
                    var obj = new TreeDto()
                    {
                        EnTitle = q.EnName,
                        Title = q.CategoryName,
                        Text = q.CategoryName,
                        Lable = q.CategoryName,
                        Value = q.Id,
                        Key = q.Id,
                        Children = await GetBrands(q.Id)
                    };
                    list.Add(obj);

                }

            }

            await context.UserLogSearchForProducts.AddAsync(new UserLogSearchForProduct
            {
                SubCategoryName = categoryItem.CategoryName,
                BrandName = "",
                CategoryName = "",
                ProductName = "",
                CreateDate = DateTime.Now,
            });
            await context.SaveChangesAsync();



            result.Data = list;
            result.IsSuccess = true;

            return result;
        }


        public async Task<ShopActionResult<List<TreeDto>>> GetSubCategoryByCategoryAndBrand(string category, string brand)
        {
            category = DataUtility.RemoveDashForTitle(category);
            brand = DataUtility.RemoveDashForTitle(brand);

            var result = new ShopActionResult<List<TreeDto>>();
            var list = new List<TreeDto>();

            var brandItem = await context.Brands.FirstOrDefaultAsync(f => f.Title == brand || f.EnTitle == brand);


            var product = await context.Products.Include(i => i.Category).Where(w => w.BrandId == brandItem.Id).ToListAsync();




            var categoryItem = await context.ProductCategories.Where(f => f.CategoryName == category || f.EnName == category).ToListAsync();

            foreach (var item in categoryItem)
            {
                foreach (var q in product.Where(w => w.CategoryId == item.Id).ToList())
                {
                    var data = await context.ProductCategories.Include(i => i.CategoryAttachments).FirstOrDefaultAsync(w => w.Id == q.Category.ParentId && w.IsActive == true);

                    var obj = new TreeDto()
                    {
                        EnTitle = data.EnName,
                        Title = data.CategoryName,
                        Text = data.CategoryName,
                        Value = data.Id,
                        Key = data.Id,
                        File = data.CategoryAttachments.Count > 0 ? data.CategoryAttachments.FirstOrDefault().FilePath : "",
                        //Children = await GetBrandsForSubCategory(q.Id),

                    };
                    list.Add(obj);

                }
            }




            result.Data = list.DistinctBy(d => d.Value).ToList();
            result.IsSuccess = true;

            return result;
        }


        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserById(int id = 1)
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();

            var category = await context.ProductCategories.FirstOrDefaultAsync(w => w.Id == id && w.IsActive == true);

            result.Data = await context.ProductCategories.Include(i => i.CategoryAttachments).Where(w => w.Id == category.ParentId && w.IsActive == true).Select(q => new UserProductCategoryDto
            {
                ParentId = q.ParentId,
                Id = q.Id,
                CategoryName = !string.IsNullOrEmpty(q.EnName) ? q.EnName : q.CategoryName,
                File = q.CategoryAttachments.Count() > 0 ? q.CategoryAttachments.FirstOrDefault().FilePath : "",
                SortOrder = q.SortOrder,
                Remark = q.Remark,

            }).OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).ToListAsync();


            result.IsSuccess = true;

            return result;
        }



        public async Task<ShopActionResult<List<UserProductCategoryDto>>> GetUserAllList()
        {
            var result = new ShopActionResult<List<UserProductCategoryDto>>();


            result.Data = await context.ProductCategories.Where(w => w.IsActive == true).Select(q => new UserProductCategoryDto
            {
                EnName = q.EnName,
                ParentId = q.ParentId,
                Id = q.Id,
                CategoryName = q.CategoryName,
                File = q.CategoryAttachments.Count() > 0 ? q.CategoryAttachments.FirstOrDefault().FilePath : "",
                SortOrder = q.SortOrder,
                Remark = q.Remark
            }).ToListAsync();



            result.IsSuccess = true;

            return result;
        }




        private async Task<TreeDto> GetParentForTree(List<TreeDto> items, Category child)
        {
            var model = new TreeDto
            {
                Code = child.Code,
                ParentId = child.ParentId,
                Key = child.Id,
                Value = child.Id,
                //Title = child.EnName = !string.IsNullOrEmpty(child.EnName) ? child.EnName : child.CategoryName,
                Title = child.CategoryName,

            };

            if (child.ParentId == null)
            {
                var parentNode = items.FirstOrDefault(q => q.Value == child.Id);
                if (parentNode == null)
                {
                    model.Children.AddRange(items);
                    return model;
                }
                else
                {
                    parentNode.Children.AddRange(items);
                    return parentNode;
                }

            }

            model.Children = items;
            var newItems = new List<TreeDto>() { model };

            var parent = await context.ProductCategories.FirstOrDefaultAsync(q => q.Id == child.ParentId && q.IsActive == true);

            var result = await GetParentForTree(newItems, parent);
            return result;
        }


        public async Task<ShopActionResult<List<TreeDto>>> Search(string text, string code)
        {
            var result = new ShopActionResult<List<TreeDto>>();

            var categoryList = new List<TreeDto>();


            var categoriesTemp = await context.ProductCategories
                  .Where(q => (string.IsNullOrEmpty(text) || q.CategoryName.Contains(text)) &&
                              (string.IsNullOrEmpty(code) || q.Code == code)).ToListAsync();

            foreach (var item in categoriesTemp)
            {
                var parentResult = await GetParentForTree(new List<TreeDto>(), item);

                var parentNode = categoryList.FirstOrDefault(q => q.Value == parentResult.Value);
                if (parentNode == null)
                {
                    categoryList.Add(parentResult);
                }
            }

            result.Data = categoryList;
            result.IsSuccess = true;
            return result;
        }



        public async Task<ShopActionResult<int>> Update(ProductCategoryDto model)
        {
            var result = new ShopActionResult<int>();

            if (model.Id == model.ParentId)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.ParentIdIsNotValid;
                return result;
            }

            var item = await context.ProductCategories.Include(q => q.Features).ThenInclude(t => t.Feature).Include(i => i.CategoryMainFeatures).Include(i => i.CategoryAttachments).SingleOrDefaultAsync(q => q.Id == model.Id);
            item.CategoryName = model.CategoryName;
            item.Code = model.Code;
            item.IsActive = model.IsActive;
            item.ParentId = model.ParentId == 0 ? null : model.ParentId;
            item.SortOrder = model.SortOrder;
            item.MainFeatureId = model.MainFeatureId;
            item.Remark = model.Remark;
            item.EnName = model.EnName;
            #region Manage Features


            //Remove Features

            foreach (var featuresItem in item.Features)
            {
                if (!model.Features.Select(q => q.Id).Contains(featuresItem.FeatureId))
                {

                    var featuresItemDeleted = await context.ProductCategoryFeatures.FirstOrDefaultAsync(f => f.FeatureId == featuresItem.FeatureId && f.CategoryId == item.Id);

                    var featuresItemValueDeleted = await context.ProductFeatureValues.Where(w => w.ProductCategoryFeatureId == featuresItemDeleted.Id).ToListAsync();

                    context.ProductFeatureValues.RemoveRange(featuresItemValueDeleted);
                    await context.SaveChangesAsync();

                    context.ProductCategoryFeatures.Remove(featuresItemDeleted);
                    await context.SaveChangesAsync();

                }
            }

            //Add New Features
            model.Features.DistinctBy(d => d.Id).ToList().ForEach(q =>
            {
                if (!item.Features.Any(f => f.FeatureId == q.Id))
                {
                    item.Features.Add(new CategoryFeature
                    {
                        FeatureId = q.Id,
                        SortOrder = q.SortOrder,
                    });
                }
            });







            ////Remove MainFeatures
            //item.CategoryMainFeatures.ToList().ForEach(q =>
            //{
            //    if (!model.MainFeatures.Select(q => q).Contains(q.FeatureId))
            //    {
            //        item.CategoryMainFeatures.Remove(q);
            //    }
            //});

            ////Add New MainFeatures
            //model.MainFeatures.ForEach(q =>
            //{
            //    if (!item.CategoryMainFeatures.Any(f => f.FeatureId == q))
            //    {
            //        item.CategoryMainFeatures.Add(new CategoryMainFeature
            //        {
            //            FeatureId = q,
            //            SortOrder = 1,
            //        });
            //    }
            //});



            #endregion

            if (model.File != null)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "CategoryAttachments");
                //context.CategoryAttachments.RemoveRange(item.CategoryAttachments);
                for (int i = 0; i < model.File.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.File[i], "Catalog", "CategoryAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }




            if (model.MainFeatureFile != null && item.MainFeatureId != null && item.MainFeatureId != 0)
            {
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "MainFeatureAttachments");

                for (int i = 0; i < model.MainFeatureFile.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.MainFeatureFile[i], "Catalog", "MainFeatureAttachments", null, item.MainFeatureId, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }



            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

        public async Task<ShopActionResult<List<TreeDto>>> GetTree(int? parentId = null)
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var items = new List<TreeDto>();
            var categories = await context.ProductCategories.Where(q => parentId == null || (q.ParentId == parentId || q.Id == parentId)).OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).ToListAsync();
            var rootElements = categories.OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).Where(q => q.ParentId == null);

            foreach (var rootElement in rootElements)
            {
                var model = new TreeDto
                {
                    //Title = !string.IsNullOrEmpty(rootElement.EnName) ? rootElement.EnName : rootElement.CategoryName,
                    //Text = !string.IsNullOrEmpty(rootElement.EnName) ? rootElement.EnName : rootElement.CategoryName,
                    Title = rootElement.CategoryName,
                    Text = rootElement.CategoryName,
                    Key = rootElement.Id,
                    Value = rootElement.Id,
                    ParentId = parentId,
                    Children = GenerateRecuresive(rootElement, categories),
                    Code = rootElement.Code
                };
                items.Add(model);
            }

            result.IsSuccess = true;
            result.Data = items.ToList();
            return result;
        }

        private List<TreeDto> GenerateRecuresive(Category parentRole, List<Category> categories, int level = 2)
        {
            var childRoles = categories.OrderBy(o => o.SortOrder).ThenBy(o => o.CategoryName).Where(q => q.ParentId == parentRole.Id);
            var children = new List<TreeDto>();
            foreach (var item in childRoles)
            {
                var child = new TreeDto
                {
                    Key = item.Id,
                    Value = item.Id,
                    //Title = !string.IsNullOrEmpty(item.EnName) ? item.EnName : item.CategoryName,
                    //Text = !string.IsNullOrEmpty(item.EnName) ? item.EnName : item.CategoryName,
                    Title = item.CategoryName,
                    Text = item.CategoryName,
                    Children = GenerateRecuresive(item, categories, 2),
                    Rate = level == 1 ? 30 : 20,
                    Code = item.Code
                };

                children.Add(child);
            }

            return children;
        }

        public async Task<ShopActionResult<List<FeatureDto>>> GetFeatures(int categoryId)
        {
            var result = new ShopActionResult<List<FeatureDto>>();
            var features = await context.ProductCategoryFeatures.Include(q => q.Feature).Include(q => q.Feature.Symbol)
                .Where(q => q.CategoryId == categoryId)
                .Select(q => new FeatureDto
                {
                    Id = q.Id,
                    Title = q.Feature.Title,
                    ControlType = q.Feature.ControlType,
                    UnitType = q.Feature.UnitType,
                    SymbolTitle = q.Feature.Symbol.Title,
                    Max = q.Feature.Max,
                    Min = q.Feature.Min,
                    Option = q.Feature.Option,
                    FeatureId = q.Feature.Id
                }).ToListAsync();

            result.Data = features.DistinctBy(d => d.FeatureId).ToList();
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<UserFeatureDto>>> GetFeatureOptionByCategoryId(int categoryId)
        {
            var result = new ShopActionResult<List<UserFeatureDto>>();


            var features = await context.Products.Include(i => i.Brand).Include(i => i.Category.Features).ThenInclude(i => i.Feature).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature)
                .Where(q => q.CategoryId == categoryId && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.Category, q.ProductName, q.Category.Features, q.Brand }).ToListAsync();


            var list = new List<UserFeatureDto>();
            foreach (var q in features.DistinctBy(d => d.Id).ToList())
            {
                var featureValue = new UserFeatureDto()
                {
                    ProductId = q.Id,
                    ProductName = q.ProductName,
                    CategoryName = !string.IsNullOrEmpty(q.Category.EnName) ? q.Category.EnName : q.Category.CategoryName,
                    BrandName = q.Brand.Title,
                    MainFeatureId = q.Category.MainFeatureId,
                    FeatureValue = q.Category.MainFeatureId != null && q.Category.MainFeatureId != 0 ?
                    q.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == q.Category.MainFeatureId).FeatureValue : "",

                    FeatureTitle = q.Features.FirstOrDefault(f => f.CategoryId == categoryId && f.FeatureId == q.Category.MainFeatureId).Feature.Title,



                };

                list.Add(featureValue);

            }


            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<FeatureProductDto>>> GetFeatureOptionByCategory(string category)
        {
            var result = new ShopActionResult<List<FeatureProductDto>>();

            var list = new List<TreeDto>();

            category = DataUtility.RemoveDashForTitle(category);

            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);



            var childRoles = await context.Products.Include(i => i.Category.Features).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature)
                .Where(q => q.CategoryId == categoryItem.Id && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.EnName, q.ProductName, q.Category.Features, q.Brand.Title }).ToListAsync();

            var featureValues = new List<FeatureProductDto>();

            childRoles.ForEach(q => featureValues.Add(new FeatureProductDto
            {
                ProductEnName = q.EnName,
                ProductName = q.ProductName,
                BrandName = q.Title,
                FeatureValue = q.FeatureValues.FirstOrDefault(q => q.ProductCategoryFeature.FeatureId == categoryItem.MainFeatureId)?.FeatureValue
            }));

            featureValues = featureValues.GroupBy(q => q.FeatureValue).Select(q => new FeatureProductDto
            {
                Key = q.Key,
                FeatureValue = q.Key,
                Brands = q.Select(r => new FeatureProductDto()
                {
                    BrandName = r.BrandName,
                    ProductName = r.ProductName,
                    FeatureValue = r.FeatureValue,
                    ProductEnName = r.ProductEnName,
                }).ToList(),

            }).ToList();



            result.Data = featureValues;
            result.IsSuccess = true;
            return result;
        }




        public async Task<ShopActionResult<List<UserFeatureDto>>> GetFeaturesByCategoryId(string category, string brand)
        {
            var result = new ShopActionResult<List<UserFeatureDto>>();

            category = DataUtility.RemoveDashForTitle(category);

            brand = DataUtility.RemoveDashForTitle(brand);


            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);
            var list = new List<UserFeatureDto>();

            var brandItem = await context.Brands.FirstOrDefaultAsync(f => f.Title == brand || f.EnTitle == brand);


            var features = await context.Products.Include(i => i.Category.Features).ThenInclude(i => i.Feature).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature)
                .Include(i => i.Category.MainFeatureAttachments)
                .Include(i => i.Category.CategoryAttachments)
                .Where(q => q.CategoryId == categoryItem.Id && q.BrandId == brandItem.Id && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.Category, q.ProductName, q.Category.Features, q.EnName }).ToListAsync();


            foreach (var q in features.DistinctBy(d => d.Id).ToList())
            {
                var featureValue = new UserFeatureDto()
                {
                    ProductId = q.Id,
                    ProductName = q.ProductName,
                    CategoryName = q.Category.EnName != null && !string.IsNullOrEmpty(q.Category.EnName) ? q.Category.EnName : q.Category.CategoryName,
                    MainFeatureFile = q.Category.MainFeatureAttachments.Count > 0 ? q.Category.MainFeatureAttachments.FirstOrDefault(s => s.CategoryId == categoryItem.Id).FilePath :
                    q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault(s => s.CategoryId == categoryItem.Id).FilePath : "",
                    MainFeatureId = q.Category.MainFeatureId,
                    FeatureValue = q.Category.MainFeatureId != null && q.Category.MainFeatureId != 0 ?
                    q.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == q.Category.MainFeatureId).FeatureValue : "",

                    FeatureTitle = q.Category.MainFeatureId != null && q.Category.MainFeatureId != 0 ? q.Features.FirstOrDefault(f => f.CategoryId == categoryItem.Id && f.FeatureId == q.Category.MainFeatureId).Feature.Title : "",
                    EnTitle = q.EnName

                };

                list.Add(featureValue);

            }


            result.Data = list;
            result.IsSuccess = true;

            return result;
        }

        public async Task<ShopActionResult<List<UserFeatureDto>>> GetFeaturesBySubCategoryId(string category, string brand, string subCategory)
        {
            var result = new ShopActionResult<List<UserFeatureDto>>();

            category = DataUtility.RemoveDashForTitle(category);

            brand = DataUtility.RemoveDashForTitle(brand);

            subCategory = DataUtility.RemoveDashForTitle(subCategory);



            var categories = new List<int>();

            var subCategoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == subCategory || f.EnName == subCategory);

            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => (f.CategoryName == category || f.EnName == category) && f.ParentId == subCategoryItem.Id);

            //categories = await context.ProductCategories.Where(w => w.ParentId == categoryItem.ParentId).Select(s => s.Id).ToListAsync();

            var list = new List<UserFeatureDto>();

            var brandItem = await context.Brands.FirstOrDefaultAsync(f => f.Title == brand || f.EnTitle == brand);


            var features = await context.Products.Include(i => i.Category.Features).ThenInclude(i => i.Feature).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature)

                .Include(i => i.Category.MainFeatureAttachments)
                .Include(i => i.Category.CategoryAttachments)
                .Where(q => q.CategoryId == categoryItem.Id && q.BrandId == brandItem.Id && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.Category, q.ProductName, q.Category.Features, q.EnName }).ToListAsync();


            foreach (var q in features.DistinctBy(d => d.Id).ToList())
            {
                var featureValue = new UserFeatureDto()
                {
                    ProductId = q.Id,
                    ProductName = q.ProductName,
                    CategoryName = q.Category.EnName != null && !string.IsNullOrEmpty(q.Category.EnName) ? q.Category.EnName : q.Category.CategoryName,
                    MainFeatureFile = q.Category.MainFeatureAttachments.Count > 0 ? q.Category.MainFeatureAttachments.FirstOrDefault(s => s.CategoryId == categoryItem.Id).FilePath :
                    q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault(s => s.CategoryId == categoryItem.Id).FilePath : "",
                    MainFeatureId = q.Category.MainFeatureId,
                    FeatureValue = q.Category.MainFeatureId != null && q.Category.MainFeatureId != 0 ?
                    q.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == q.Category.MainFeatureId).FeatureValue : "",

                    FeatureTitle = q.Category.MainFeatureId != null && q.Category.MainFeatureId != 0 ? context.Features.FirstOrDefault(f => f.Id == q.Category.MainFeatureId)?.Title : "",
                    EnTitle = q.EnName

                };

                list.Add(featureValue);

            }


            result.Data = list;
            result.IsSuccess = true;

            return result;
        }



        public async Task<ShopActionResult<FeatureDto>> GetFeaturesByTitle(string category)
        {
            var result = new ShopActionResult<FeatureDto>();

            category = DataUtility.RemoveDashForTitle(category);

            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);

            if (categoryItem.MainFeatureId != null && categoryItem.MainFeatureId != 0)
            {
                var features = await context.ProductCategoryFeatures.Include(q => q.Feature).Include(q => q.Feature.Symbol)
                    .FirstOrDefaultAsync(q => q.CategoryId == categoryItem.Id && q.Feature.ShowInFilter == true && q.Feature.Id == categoryItem.MainFeatureId);

                if (features != null)
                {
                    var featureItem = new FeatureDto()
                    {
                        Id = features.Id,
                        Title = features.Feature.Title,
                        ControlType = features.Feature.ControlType,
                        UnitType = features.Feature.UnitType,
                        SymbolTitle = features.Feature.Symbol.Title,
                        Max = features.Feature.Max,
                        Min = features.Feature.Min,
                        Option = features.Feature.Option,
                        FeatureId = features.Feature.Id,

                    };
                    result.Data = featureItem;

                }


            }

            result.IsSuccess = true;
            return result;
        }



        public async Task<ShopActionResult<List<FeatureDto>>> GetFeaturesForSearch(int categoryId)
        {
            var result = new ShopActionResult<List<FeatureDto>>();
            var features = await context.ProductCategoryFeatures.Include(q => q.Feature).Include(q => q.Feature.Symbol)
                .Where(q => q.CategoryId == categoryId)
                .Select(q => new FeatureDto
                {
                    Id = q.Id,
                    Title = q.Feature.Title,
                    ControlType = q.Feature.ControlType,
                    UnitType = q.Feature.UnitType,
                    SymbolTitle = q.Feature.Symbol.Title,
                    Max = q.Feature.Max,
                    Min = q.Feature.Min,
                    Option = q.Feature.Option,
                    FeatureId = q.Feature.Id,

                }).ToListAsync();

            result.Data = features.DistinctBy(d => d.FeatureId).OrderBy(o => o.ControlType).ToList();
            result.IsSuccess = true;
            return result;
        }


        public int GetCategoryChildCountRecursive(int parentId, List<Category> categories, int total)
        {
            var childCategories = categories.Where(q => q.ParentId == parentId);
            foreach (var item in childCategories)
            {
                total += 1;
                var children = categories.Where(q => q.ParentId == parentId);
                total = GetCategoryChildCountRecursive(item.Id, categories, total);
            }
            return total;
        }

        public async Task<ShopActionResult<ProductCategoryForDashboardDto>> GetProductCategoryForDashboard()
        {
            var result = new ShopActionResult<ProductCategoryForDashboardDto>();
            result.Data = new ProductCategoryForDashboardDto();



            var productCategories = await context.ProductCategories.Include(q => q.Features).OrderBy(o => o.SortOrder).Where(w => w.IsActive == true).ToListAsync();
            var topCategories = productCategories.Where(q => q.ParentId == null);

            var totalProductCategoryCount = 0;
            foreach (var category in topCategories)
            {
                var item = new ProductCategoryForDashboardItemDto
                {
                    Id = category.Id,
                    Title = category.CategoryName,
                };
                item.Value = GetCategoryChildCountRecursive(category.Id, productCategories, 0);
                totalProductCategoryCount += item.Value;
                result.Data.Items.Add(item);
            }

            result.Data.ProductCategoryCount = totalProductCategoryCount;

            result.IsSuccess = true;
            return result;
        }






        public async Task<ShopActionResult<List<TreeDto>>> GetCategoriesForMegaMenu()
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var items = new List<TreeDto>();
            var categories = await context.ProductCategories.Where(q => q.IsActive).OrderBy(o => o.SortOrder)
                .Select(q => new TreeItemDto { Id = q.Id, Title = q.CategoryName, ParentId = q.ParentId, SortOrder = q.SortOrder, EnTitle = q.EnName }).ToListAsync();
            var rootElements = categories.Where(q => q.ParentId == null);

            foreach (var rootElement in rootElements)
            {
                var model = new TreeDto
                {
                    EnTitle = rootElement.EnTitle,
                    Title = rootElement.Title,
                    Text = rootElement.Title,
                    Key = rootElement.Id,
                    Value = rootElement.Id,
                    Children = await GenerateProductRecuresive(rootElement.Id, categories),
                };
                //if (model.Children.Count > 0)
                //{
                //    items.Add(model);

                //}
                items.Add(model);
            }

            result.IsSuccess = true;
            result.Data = items;
            return result;
        }
        private async Task<List<TreeDto>> GenerateProductRecuresive(int categoryId, List<TreeItemDto> categories, int level = 2)
        {
            var childRoles = categories.Where(q => q.ParentId == categoryId);
            var children = new List<TreeDto>();
            foreach (var item in childRoles.OrderBy(o => o.SortOrder).ThenBy(o => o.Title).ToList())
            {
                var child = new TreeDto
                {
                    EnTitle = item.EnTitle,
                    ParentId = categoryId,
                    Key = item.Id,
                    Title = item.Title,
                    Value = item.Id,
                    Text = item.Title,
                    //Children = await GetProducts(item.Id),
                    Children = await GetBrands(item.Id),

                };
                //if (child.Children.Count > 0)
                //{
                //    children.Add(child);

                //}
                children.Add(child);
            }

            return children;
        }


        private async Task<List<TreeDto>> GetBrandsForSubCategory(int categoryId)
        {

            var categories = await context.ProductCategories.Where(w => w.ParentId == categoryId).Select(s => s.Id).ToListAsync();

            var products = await context.Products.Include(i => i.Brand).Where(w => categories.Contains(w.CategoryId) && w.Brand.IsActive == true)
                .Select(s => new { s.CategoryId, s.Brand.Id, s.Brand.Title, s.BrandId, s.ProductName, s.Category.MainFeatureId, s.Brand.EnTitle }).Distinct().ToListAsync();

            var children = new List<TreeDto>();
            foreach (var item in products.DistinctBy(d => d.BrandId).ToList())
            {
                var child = new TreeDto
                {
                    EnTitle = item.EnTitle,
                    Key = item.BrandId,
                    Title = item.Title,
                    Value = item.BrandId,
                    Text = item.Title,
                };

                children.Add(child);
            }

            return children;
        }

        private async Task<List<TreeDto>> GetBrands(int categoryId)
        {
            var products = await context.Products.Include(i => i.Brand).Where(w => w.CategoryId == categoryId && w.Brand.IsActive == true)
                .Select(s => new { s.CategoryId, s.Brand.Id, s.Brand.Title, s.BrandId, s.ProductName, s.Category.MainFeatureId, s.Brand.EnTitle }).Distinct().ToListAsync();

            var children = new List<TreeDto>();
            foreach (var item in products.DistinctBy(d => d.BrandId).ToList())
            {
                var child = new TreeDto
                {
                    EnTitle = item.EnTitle,
                    Key = item.BrandId,
                    Title = item.Title,
                    Value = item.BrandId,
                    Text = item.Title,
                    Children = await GetFeatureCategoryProducts(categoryId, item.BrandId, item.MainFeatureId),
                };

                children.Add(child);
            }

            return children;
        }

        private async Task<List<TreeDto>> GetFeatureCategoryProducts(int categoryId, int brandId, int? mainFeatureId)
        {
            var childRoles = await context.Products.Include(i => i.Category).Include(q => q.FeatureValues).ThenInclude(q => q.ProductCategoryFeature.Feature.Symbol)
                .Where(q => q.CategoryId == categoryId && q.BrandId == brandId && q.IsActive).Select(q => new { q.Id, q.FeatureValues, q.EnName, q.ProductName }).ToListAsync();

            var brandItem = await context.Brands.FirstOrDefaultAsync(f => f.Id == brandId);
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
                        BrandTitle = brandItem.Title,
                        SymbolTitle = item.FeatureValues.Count > 0 ? item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.ProductCategoryFeature?.Feature?.Symbol?.Title : "",

                    };

                    if (item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.FeatureValue != null && item.FeatureValues.FirstOrDefault(f => f.ProductCategoryFeature.FeatureId == mainFeatureId.Value)?.FeatureValue != "")
                    {
                        children.Add(child);

                    }

                }

            }

            return children;
        }


        public async Task<List<TreeDto>> GetCategoryWithBrands(string category)
        {
            category = DataUtility.RemoveDashForTitle(category);

            var categories = await context.ProductCategories.Where(q => q.IsActive && (q.CategoryName == category || q.EnName == category)).Select(s => s.Id).ToListAsync();

            var products = await context.Products.Include(i => i.Brand).Include(i => i.Category)
                .Where(w =>
                (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling) &&
                w.IsActive == true && (w.Category.ParentId != null && categories.Contains(w.Category.ParentId.Value)) && w.Brand.IsActive == true)
                .Select(s => new { s.CategoryId, s.Brand.Id, s.Brand.Title, s.BrandId, s.ProductName, s.Category.MainFeatureId }).Distinct().ToListAsync();

            var children = new List<TreeDto>();
            foreach (var item in products.DistinctBy(d => d.BrandId).ToList())
            {
                var child = new TreeDto
                {
                    Key = item.BrandId,
                    Title = item.Title,
                    Value = item.BrandId,
                    Text = item.Title,
                    Children = await GetcategoryForBrands(item.BrandId),
                };

                children.Add(child);
            }

            return children;
        }

        private async Task<List<TreeDto>> GetcategoryForBrands(int brandId)
        {
            var childRoles = await context.Products.Include(i => i.Category).Where(q =>
            (q.Inventory != null && q.Inventory.Value > 0 && q.SaleStatus == SaleStatus.InProgressSelling) &&
            q.BrandId == brandId && q.IsActive).OrderBy(q => q.Id).Select(q => new { q.CategoryId, q.Category.CategoryName, q.Category.EnName }).ToListAsync();
            var children = new List<TreeDto>();

            foreach (var item in childRoles.DistinctBy(d => d.CategoryId).ToList())
            {
                var child = new TreeDto
                {
                    Key = item.CategoryId,
                    Title = item.CategoryName,
                    Value = item.CategoryId,
                    Text = item.CategoryName,
                    EnTitle = item.EnName,
                };

                children.Add(child);
            }

            return children;
        }





        private async Task<List<TreeDto>> GetMainFeatures(int categoryId)
        {
            var products = await context.CategoryMainFeatures.Include(i => i.Feature).Where(w => w.CategoryId == categoryId).Select(s => new { s.Feature.Title, s.FeatureId }).Distinct().ToListAsync();



            var children = new List<TreeDto>();
            foreach (var item in products)
            {
                var child = new TreeDto
                {
                    Key = item.FeatureId,
                    Title = item.Title,
                    Value = item.FeatureId,
                    Text = item.Title,
                };

                children.Add(child);
            }

            return children;
        }
        private async Task<List<TreeDto>> GetProducts(int categoryId)
        {
            var childRoles = await context.Products.Where(q => q.CategoryId == categoryId && q.IsActive).OrderBy(q => q.Id).Take(10).Select(q => new { q.Id, q.ProductName }).ToListAsync();
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

    }
}
