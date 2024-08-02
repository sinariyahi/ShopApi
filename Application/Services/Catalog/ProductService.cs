using Application.Interfaces.Catalog;
using Application.Interfaces;
using Domain.Entities.Blog;
using Domain.Entities.Catalog;
using Domain.Entities.Media;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Infrastructure.Models.EIED;
using Infrastructure.Models.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Catalog
{
    public class ProductService : IProductService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Product> productQueryService;
        private readonly IGenericQueryService<UserLogSearchForProduct> userLogSearchForProductQueryService;
        private readonly IGoldiranService goldiranService;

        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        private readonly ILogger logger;
        public ProductService(BIContext context,
            IGenericQueryService<Product> productQueryService,
            IGenericQueryService<UserLogSearchForProduct> userLogSearchForProductQueryService,

                  IFileService fileService, IOptions<Configs> options, IGoldiranService goldiranService,
            ILogger<ProductService> logger)
        {
            this.context = context;
            this.productQueryService = productQueryService;
            this.userLogSearchForProductQueryService = userLogSearchForProductQueryService;

            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
            this.goldiranService = goldiranService;
            this.logger = logger;
        }

        #region Admin - Product
        public async Task<GoldiranActionResult<int>> Add(ProductDto model)
        {
            var result = new GoldiranActionResult<int>();

            if (context.Products.Any(a => a.Code == model.Code))
            {
                result.IsSuccess = false;
                result.Message = MessagesFA.CodeExists;
                return result;
            }

            var item = new Product
            {
                Code = model.Code,
                EnName = model.EnName,
                Remark = model.Remark,
                CreateDate = DateTime.Now,
                CategoryId = model.CategoryId,
                ProductName = model.ProductName,
                BrandId = model.BrandId,
                Weight = model.Weight,
                IsActive = model.IsActive,
                GetInventoryFromApi = model.GetInventoryFromApi,
                Inventory = model.GetInventoryFromApi == true ? 0 : model.Inventory,
                SeoDescription = model.SeoDescription,
                SeoTitle = model.SeoTitle,
                SaleStatus = model.SaleStatus,
                SaleCount = model.SaleCount,
                IsTopNew = model.IsTopNew,
                IsTopSale = model.IsTopSale,
                VisitedCount = model.VisitedCount,
                IsTopVisited = model.IsTopVisited,
                IsSpecialOffer = model.IsSpecialOffer,
                ShortDescription = model.ShortDescription
            };

            if (context.Products.Any(a => a.EnName == model.EnName))
            {
                result.IsSuccess = false;
                result.Message = MessagesFA.EnNameAlreadyExists;
                return result;

            }


            #region FeatureValues
            if (model.ProductFeatureValues != null)
                model.ProductFeatureValues.ForEach(q =>
                  item.FeatureValues.Add(new ProductFeatureValue
                  {
                      CreateDate = DateTime.Now,
                      ProductCategoryFeatureId = q.ProductCategoryFeatureId,
                      FeatureValue = q.Value,
                      FeatureValueNumber = q.FeatureValueNumber,
                      UserId = model.UserId,
                      ProductId = item.Id,
                  }));

            #endregion

            #region ProductUsages
            if (model.ProductUsages != null)
                model.ProductUsages.ForEach(q => item.ProductUsages.Add(new ProductUsage
                {
                    CreateDate = DateTime.Now,
                    Title = q.Title,
                }));

            await context.AddAsync(item);
            await context.SaveChangesAsync();

            #endregion


            result.IsSuccess = true;
            result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        public async Task<GoldiranActionResult<int>> Delete(int id)
        {
            var result = new GoldiranActionResult<int>();

            var item = new Product { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }


        public async Task<GoldiranActionResult<ProductDto>> GetById(int id)
        {
            var result = new GoldiranActionResult<ProductDto>();

            var item = await context.Products
                //.Include(q => q.ProductAttachments)
                //.Include(q => q.ProductAtt)
                .Include(q => q.ProductUsages)
                .Include(q => q.Category)
                .Include(q => q.Brand)
                .Include(q => q.FeatureValues)
                .ThenInclude(q => q.ProductCategoryFeature)
                .ThenInclude(q => q.Feature)
                .ThenInclude(q => q.Symbol)
                .SingleOrDefaultAsync(q => q.Id == id);

            var model = new ProductDto
            {
                Remark = item.Remark,
                Id = item.Id,
                EnName = item.EnName,
                MainFeatureId = item.Category.MainFeatureId,
                CategoryName = item.Category.CategoryName,
                Code = item.Code,
                BrandId = item.BrandId,
                BrandName = item.Brand.Title,
                ProductName = item.ProductName,
                CategoryId = item.CategoryId,
                IsActive = item.IsActive,
                Inventory = item.Inventory,
                GetInventoryFromApi = item.GetInventoryFromApi,
                SeoDescription = item.SeoDescription,
                SeoTitle = item.SeoTitle,
                SaleStatus = item.SaleStatus,
                SaleCount = item.SaleCount,
                IsTopNew = item.IsTopNew,
                IsTopSale = item.IsTopSale,
                VisitedCount = item.VisitedCount,
                Weight = item.Weight,
                IsTopVisited = item.IsTopVisited,
                IsSpecialOffer = item.IsSpecialOffer,
                ShortDescription = item.ShortDescription,
                APIAmount = item.APIAmount,
                APIQuantity = item.APIQuantity,
                //ProductAttachments = item.ProductAttachments.Select(u =>u.FilePath).ToList(),
                ProductUsages = item.ProductUsages.Select(u => new ProductUsageDto
                {
                    Id = u.Id,
                    Title = u.Title,
                }).ToList(),

                ProductFeatureValues = item.FeatureValues.Select(f => new CategoryFeatureDto
                {
                    Id = f.ProductCategoryFeatureId,
                    Title = f.ProductCategoryFeature.Feature.Title,
                    ProductCategoryFeatureId = f.ProductCategoryFeatureId,
                    SortOrder = f.ProductCategoryFeature.SortOrder,
                    FeatureValue = f.FeatureValue,
                    FeatureValueNumber = f.FeatureValueNumber,
                    UnitTypeTitle = f.ProductCategoryFeature.Feature.UnitType.GetNameAttribute(),
                    ControlType = f.ProductCategoryFeature.Feature.ControlType,
                    UnitType = f.ProductCategoryFeature.Feature.UnitType,
                    SymbolTitle = f.ProductCategoryFeature.Feature.Symbol.Title,
                    Max = f.ProductCategoryFeature.Feature.Max,
                    Min = f.ProductCategoryFeature.Feature.Min,
                    Option = f.ProductCategoryFeature.Feature.Option,
                    FeatureId = f.ProductCategoryFeature.Feature.Id,
                    Value = f.ProductCategoryFeature.Feature.ControlType == ControlType.Number ? f.FeatureValueNumber.ToString() : f.FeatureValue
                }).ToList(),
            };


            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<byte[]> GetListForExcel(GridQueryModel model = null, string fileName = null)
        {
            var queryResult = await productQueryService.QueryAsync(model, includes: new List<string> { "Category", "Brand", "ProductAttachments" }, exportToExcel: true);

            var data = queryResult.Data.Select(q => new ProductExportToExcelDto
            {
                Id = q.Id,
                CategoryName = q.Category.CategoryName,
                Code = q.Code,
                ProductName = q.ProductName,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                BrandName = q.Brand.Title,
                SaleStatusTitle = q.SaleStatus != null ? EnumHelpers.GetNameAttribute<SaleStatus>(q.SaleStatus.Value) : ""
            }).ToList();

            var exportData = ExcelUtility.ExportToExcel<ProductExportToExcelDto>(data, fileName);
            return exportData;
        }

        public async Task<ShopActionResult<int>> ImportPriceListWithExcel(IFormFile file, Guid userId)
        {
            //import excel file columns priority :
            //ProductName	Code	Price	Quantity
            var result = new ShopActionResult<int>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                IExcelDataReader reader;
                reader = ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                var products = await context.Products.ToListAsync();

                var dataSet = reader.AsDataSet(conf);

                var dataTable = dataSet.Tables[0];
                foreach (DataRow item in dataTable.Rows)
                {
                    if (item["code"] is DBNull) continue;

                    var code = item["code"].ToString();
                    var product = products.FirstOrDefault(q => q.Code == code);
                    if (product != null)
                    {
                        if (item["price"] is not DBNull)
                            product.APIAmount = Convert.ToInt64(item["price"]);
                        if (item["quantity"] is not DBNull)
                        {
                            var value = Convert.ToInt32(item["quantity"]); ;
                            product.APIQuantity = value;
                            product.Inventory = value;
                        }
                    }
                }
                result.Data = await context.SaveChangesAsync();
            }
            result.IsSuccess = true;
            //result.Message = MessagesFA.SuccessTheOperation;
            return result;
        }

        public async Task<ShopActionResult<List<UserLogSearchForProductDto>>> GetListUserLogSearchForProduct(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserLogSearchForProductDto>>();
            int skip = (model.Page - 1) * model.Size;

            var brandName = "";
            var categoryName = "";
            var productName = "";
            var subCategoryName = "";


            foreach (var item in model.Filtered)
            {
                if (item.column == "productName" && !String.IsNullOrEmpty(item.value))
                {
                    productName = item.value;
                }
                if (item.column == "brandName" && !String.IsNullOrEmpty(item.value))
                {
                    brandName = item.value;
                }
                if (item.column == "categoryName" && !String.IsNullOrEmpty(item.value))
                {
                    categoryName = item.value;
                }

                if (item.column == "subCategoryName" && !String.IsNullOrEmpty(item.value))
                {
                    subCategoryName = item.value;
                }

            }
            //var queryResult = await userLogSearchForProductQueryService.QueryAsync(model, includes: new List<string> { "Product", "Product.Category", "Product.Brand" });
            var queryResult = context.UserLogSearchForProducts.Include(i => i.Product.Category).Include(i => i.Product.Brand);



            var data = queryResult.Select(q => new UserLogSearchForProductDto
            {
                BrandName = !string.IsNullOrEmpty(q.BrandName) ? q.BrandName : !string.IsNullOrEmpty(q.Product.Brand.Title) ? q.Product.Brand.Title : "",
                ProductName = !string.IsNullOrEmpty(q.ProductName) ? q.ProductName : !string.IsNullOrEmpty(q.Product.ProductName) ? q.Product.ProductName : "",
                CategoryName = !string.IsNullOrEmpty(q.CategoryName) ? q.CategoryName : !string.IsNullOrEmpty(q.Product.Category.CategoryName) ? q.Product.Category.CategoryName : "",
                SubCategoryName = !string.IsNullOrEmpty(q.SubCategoryName) ? q.SubCategoryName : !string.IsNullOrEmpty(q.Product.Category.CategoryName) ? q.Product.Category.CategoryName : "",
                CreateDate = DateUtility.CovertToShamsi(q.CreateDate)
            }).ToList();

            data = data.Skip(skip).Take(model.Size).ToList();


            result.Data = data.Where(w =>
             w.ProductName.Contains(productName) &&
            w.CategoryName.Contains(categoryName) &&
           w.BrandName.Contains(brandName) &&
           w.SubCategoryName.Contains(subCategoryName)


             ).ToList();

            result.IsSuccess = true;
            result.Total = queryResult.Count();
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }

        public async Task<byte[]> GetListUserLogSearchForExcel(GridQueryModel model = null, string fileName = null)
        {

            var brandName = "";
            var categoryName = "";
            var productName = "";
            var subCategoryName = "";


            //foreach (var item in model.Filtered)
            //{
            //    if (item.column == "productName" && !String.IsNullOrEmpty(item.value))
            //    {
            //        productName = item.value;
            //    }
            //    if (item.column == "brandName" && !String.IsNullOrEmpty(item.value))
            //    {
            //        brandName = item.value;
            //    }
            //    if (item.column == "categoryName" && !String.IsNullOrEmpty(item.value))
            //    {
            //        categoryName = item.value;
            //    }

            //    if (item.column == "subCategoryName" && !String.IsNullOrEmpty(item.value))
            //    {
            //        subCategoryName = item.value;
            //    }

            //}
            //var queryResult = await userLogSearchForProductQueryService.QueryAsync(model, includes: new List<string> { "Product", "Product.Category", "Product.Brand" });
            var queryResult = context.UserLogSearchForProducts.Include(i => i.Product.Category).Include(i => i.Product.Brand);



            var data = queryResult.Select(q => new UserLogSearchForProductDto
            {
                BrandName = !string.IsNullOrEmpty(q.BrandName) ? q.BrandName : !string.IsNullOrEmpty(q.Product.Brand.Title) ? q.Product.Brand.Title : "",
                ProductName = !string.IsNullOrEmpty(q.ProductName) ? q.ProductName : !string.IsNullOrEmpty(q.Product.ProductName) ? q.Product.ProductName : "",
                CategoryName = !string.IsNullOrEmpty(q.CategoryName) ? q.CategoryName : !string.IsNullOrEmpty(q.Product.Category.CategoryName) ? q.Product.Category.CategoryName : "",
                SubCategoryName = !string.IsNullOrEmpty(q.SubCategoryName) ? q.SubCategoryName : !string.IsNullOrEmpty(q.Product.Category.CategoryName) ? q.Product.Category.CategoryName : "",
                CreateDate = DateUtility.CovertToShamsi(q.CreateDate)
            }).ToList();

            var exportData = ExcelUtility.ExportToExcel<UserLogSearchForProductDto>(data, fileName);
            return exportData;
        }




        public async Task<ShopActionResult<List<ProductDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<ProductDto>>();

            var queryResult = await productQueryService.QueryAsync(model, includes: new List<string> { "Category", "Brand", "ProductAttachments" });

            result.Data = queryResult.Data.Select(q => new ProductDto
            {
                Id = q.Id,
                Remark = q.Remark,
                CategoryName = q.Category.CategoryName,
                Code = q.Code,
                ProductName = q.ProductName,
                IsActive = q.IsActive,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیر فعال",
                BrandName = q.Brand.Title,
                ProductAttachments = q.ProductAttachments.Select(s => s.FilePath).ToList(),
                Inventory = q.Inventory,
                GetInventoryFromApi = q.GetInventoryFromApi,
                SaleStatusTitle = q.SaleStatus != null ? EnumHelpers.GetNameAttribute<SaleStatus>(q.SaleStatus.Value) : ""
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }
        public async Task<ShopActionResult<List<ComboItemDto>>> GetListWithCategory(int categoryId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();

            result.Data = await context.Products
                .Where(q => q.CategoryId == categoryId)
                .Select(q => new ComboItemDto
                {
                    Text = q.ProductName,
                    Value = q.Id,
                    ParentId = q.CategoryId,
                }).ToListAsync();
            result.IsSuccess = true;
            return result;
        }
        public async Task<ShopActionResult<int>> Update(ProductDto model)
        {
            var result = new ShopActionResult<int>();

            if (context.Products.Any(a => a.Code == model.Code && a.Id != model.Id))
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.CodeExists;
                return result;
            }

            var item = await context.Products
                .Include(q => q.ProductUsages)
                .Include(q => q.Category)
                .Include(q => q.FeatureValues)
                .Include(f => f.ProductAttachments)
                .SingleOrDefaultAsync(q => q.Id == model.Id);

            if (item == null)
            {
                result.IsSuccess = false;
                //result.Message = MessagesFA.ItemNotFound;
                return result;
            }


            if (context.Products.Any(a => a.EnName == model.EnName && a.Id != model.Id))
            {
                result.IsSuccess = false;
              //  result.Message = MessagesFA.EnNameAlreadyExists;
                return result;

            }

            #region Manage Usages
            if (model.ProductUsages != null)
            {
                //remove usages
                if (item.ProductUsages != null)
                    item.ProductUsages.ToList().ForEach(q => context.ProductUsages.Remove(q));
                //add usages

                model.ProductUsages.ForEach(q => item.ProductUsages.Add(new ProductUsage
                {
                    CreateDate = DateTime.Now,
                    Title = q.Title
                }));
            }
            #endregion

            await context.SaveChangesAsync();



            item.ProductName = model.ProductName;
            item.Code = model.Code;
            item.CategoryId = model.CategoryId;
            item.Remark = model.Remark;
            item.IsActive = model.IsActive;
            item.BrandId = model.BrandId;
            item.GetInventoryFromApi = model.GetInventoryFromApi;
            item.Inventory = model.GetInventoryFromApi == true ? 0 : model.Inventory;
            item.SeoDescription = model.SeoDescription;
            item.SeoTitle = model.SeoTitle;
            item.SaleStatus = model.SaleStatus;
            item.SaleCount = model.SaleCount;
            item.IsTopNew = model.IsTopNew;
            item.IsTopSale = model.IsTopSale;
            //item.VisitedCount = model.VisitedCount;
            item.IsTopVisited = model.IsTopVisited;
            item.IsSpecialOffer = model.IsSpecialOffer;
            item.ShortDescription = model.ShortDescription;
            item.EnName = model.EnName;
            item.Weight = model.Weight;
            #region Manage Features

            if (model.ProductFeatureValues != null)
            {
                //remove feature 
                item.FeatureValues.ToList().ForEach(q => context.ProductFeatureValues.Remove(q));

                //add feature 

                model.ProductFeatureValues.ForEach(q =>
                  item.FeatureValues.Add(new ProductFeatureValue
                  {
                      CreateDate = DateTime.Now,
                      ProductCategoryFeatureId = q.ProductCategoryFeatureId,
                      FeatureValue = q.FeatureValue,
                      FeatureValueNumber = q.FeatureValueNumber,
                      UserId = model.UserId,
                      ProductId = item.Id,
                  }));
            }

            #endregion


            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
        #endregion

        #region Product - Attachment
        public async Task<ShopActionResult<ProductAttachmentDto>> GetProductAttachment(int id)
        {
            var result = new ShopActionResult<ProductAttachmentDto>();

            result.Data = new ProductAttachmentDto();

            result.Data.ListProductAttachment = context.ProductAttachments
           .AsEnumerable().Where(q => q.ProductId == id).GroupBy(r => new
           {
               Group = r.ProductAttachmentType,
           }).Select(q => new ProductFileAttachmentDto
           {
               ProductAttachmentType = q.Key.Group,
               ProductAttachmentTypeTitle = EnumHelpers.GetNameAttribute<ProductAttachmentType>(q.Key.Group).ToString(),
               FileAttachments = q.Select(s => new FileItemDto { Entity = "ProductAttachment", FilePath = s.FilePath }).ToList()

           }).ToList();
            result.Data.CoverAttachment = new FileItemDto();
            result.Data.CoverAttachment.FilePath = context.ProductCoverAttachments.FirstOrDefault(f => f.ProductId == id)?.FilePath;
            result.Data.CoverAttachment.Entity = "ProductCoverAttachments";
            if (String.IsNullOrEmpty(result.Data.CoverAttachment.FilePath))
            {
                result.Data.CoverAttachment = new FileItemDto();
            }

            result.Data.ListVideoProductAttachmentModel = await context.VideoProductAttachments
                .Include(i => i.VideoFileProductAttachments).Where(w => w.ProductId == id).Select(s => new VideoProductAttachmentDto
                {
                    Id = s.Id,
                    VideoLink = s.VideoLink,
                    VideoTitle = s.VideoTitle,
                    FileAttachments = s.VideoFileProductAttachments.Select(s => new FileItemDto { Entity = "VideoFileProductAttachments", FilePath = s.FilePath }).ToList()

                }).ToListAsync();



            result.IsSuccess = true;
            return result;
        }
        public async Task<ShopActionResult<int>> UpdateProductAttachment(ProductAttachmentInputDto model)
        {
            var result = new ShopActionResult<int>();


            if (model.CoverFiles != null)
            {
                var productCoverAttachments = await context.ProductCoverAttachments.Where(w => w.ProductId == model.ProductId).ToListAsync();
                //context.ProductCoverAttachments.RemoveRange(productCoverAttachments);

                foreach (var item in model.CoverFiles)
                {


                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductCoverAttachments");
                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "ProductCoverAttachments");
                    await _fileService.SaveNewItemInFolder(item, "Catalog" + "\\" + "ProductAttachments", "ProductCoverAttachments", "ProductCoverAttachments", null, model.ProductId, DateTime.Now, Guid.NewGuid(), false);



                }
            }




            if (model.CatalogFiles != null)
            {
                var catalogueAndBrochure = await context.ProductAttachments.Where(w => w.ProductId == model.ProductId && w.ProductAttachmentType == ProductAttachmentType.CatalogueAndBrochure).ToListAsync();
                //context.ProductAttachments.RemoveRange(catalogueAndBrochure);

                foreach (var item in model.CatalogFiles)
                {


                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductAttachments");
                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "Catalog");
                    await _fileService.SaveNewItemInFolder(item, "Catalog" + "\\" + "ProductAttachments", "Catalog", "FileProductAttachments", ProductAttachmentType.CatalogueAndBrochure, model.ProductId, DateTime.Now, Guid.NewGuid(), false);



                }
            }


            if (model.OtherFiles != null)
            {
                var otherfiles = await context.ProductAttachments.Where(w => w.ProductId == model.ProductId && w.ProductAttachmentType == ProductAttachmentType.Other).ToListAsync();
                //context.ProductAttachments.RemoveRange(otherfiles);

                foreach (var item in model.OtherFiles)
                {


                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductAttachments");
                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "OtherFiles");
                    await _fileService.SaveNewItemInFolder(item, "Catalog" + "\\" + "ProductAttachments", "OtherFiles", "FileProductAttachments", ProductAttachmentType.Other, model.ProductId, DateTime.Now, Guid.NewGuid(), false);

                }
            }



            if (model.OrginalFiles != null)
            {


                var orginalFiles = await context.ProductAttachments.Where(w => w.ProductId == model.ProductId && w.ProductAttachmentType == ProductAttachmentType.Orginal).ToListAsync();
                //context.ProductAttachments.RemoveRange(orginalFiles);
                foreach (var item in model.OrginalFiles)
                {

                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductAttachments");
                    await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "OrginalFiles");
                    await _fileService.SaveNewItemInFolder(item, "Catalog" + "\\" + "ProductAttachments", "OrginalFiles", "FileProductAttachments", ProductAttachmentType.Orginal, model.ProductId, DateTime.Now, Guid.NewGuid(), false);

                }
            }



            var videoProductAttachmentData = await context.VideoProductAttachments.Include(i => i.VideoFileProductAttachments).Where(w => w.ProductId == model.ProductId).ToListAsync();

            if (model.ListVideoProductAttachmentModel != null)
            {
                foreach (var item in model.ListVideoProductAttachmentModel)
                {

                    var deletedItem = videoProductAttachmentData.Select(s => s.Id).ToList().Except(model.ListVideoProductAttachmentModel.Select(s => s.Id).ToList());

                    foreach (var obj in deletedItem)
                    {
                        var deleteItem = await context.VideoProductAttachments.FindAsync(obj);
                        context.VideoProductAttachments.Remove(deleteItem);
                        await context.SaveChangesAsync();

                    }





                    if (item.Id == 0)
                    {

                        var videoProduct = new VideoProductAttachment
                        {
                            ProductId = model.ProductId,
                            VideoLink = item.VideoLink,
                            VideoTitle = item.VideoTitle
                        };
                        await context.VideoProductAttachments.AddAsync(videoProduct);
                        await context.SaveChangesAsync();

                        if (item.File != null)
                        {


                            await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductAttachments");
                            await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "VideoFileProductAttachments");

                            for (int i = 0; i < item.File.Count(); i++)
                            {
                                await _fileService.SaveNewItemInFolder(item.File[i], "Catalog" + "\\" + "ProductAttachments", "VideoFileProductAttachments", null, null, videoProduct.Id, DateTime.Now, Guid.NewGuid(), false);

                            }
                        }
                    }
                    else
                    {
                        var videoProductAttachment = await context.VideoProductAttachments.Include(i => i.VideoFileProductAttachments).FirstOrDefaultAsync(w => w.Id == item.Id);
                        videoProductAttachment.ProductId = model.ProductId;
                        videoProductAttachment.VideoLink = item.VideoLink;
                        videoProductAttachment.VideoTitle = item.VideoTitle;
                        await context.SaveChangesAsync();

                        if (item.File != null)
                        {
                            //if (videoProductAttachment.VideoFileProductAttachments.Count > 0)
                            //{
                            //    context.VideoFileProductAttachments.RemoveRange(videoProductAttachment.VideoFileProductAttachments);

                            //}

                            await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog", "ProductAttachments");
                            await _fileService.CreateFolderNewItem(_filePath + "\\" + "Catalog" + "\\" + "ProductAttachments", "VideoFileProductAttachments");

                            for (int i = 0; i < item.File.Count(); i++)
                            {
                                await _fileService.SaveNewItemInFolder(item.File[i], "Catalog" + "\\" + "ProductAttachments", "VideoFileProductAttachments", null, null, videoProductAttachment.Id, DateTime.Now, Guid.NewGuid(), false);

                            }
                        }


                    }


                }
            }
            else
            {
                var videoProductAttachments = await context.VideoProductAttachments.Include(i => i.VideoFileProductAttachments).Where(w => w.ProductId == model.ProductId).ToListAsync();
                context.VideoProductAttachments.RemoveRange(videoProductAttachments);

            }



            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
        public async Task<ShopActionResult<bool>> DeleteAttachment(Guid attachmentId)
        {
            var result = new ShopActionResult<bool>();

            var item = new ProductAttachment { Id = attachmentId };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        #endregion

        #region Article - Product
        public async Task<ShopActionResult<ArticleProductInputDto>> GetArticleProductById(int id)
        {
            var result = new ShopActionResult<ArticleProductInputDto>();


            var articles = await context.ArticleProducts.Include(i => i.Article).Where(w => w.ProductId == id).Select(s => new ArticleSelectedDto
            {

                ArticleId = s.ArticleId,
                ArticleTitle = s.Article.Title,

            }).ToListAsync();


            var model = new ArticleProductInputDto()
            {
                ProductId = id,
                Articles = articles

            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }
        public async Task<ShopActionResult<int>> AddArticleProduct(ArticleProductInputDto model)
        {
            var result = new ShopActionResult<int>();
            var articleProduct = await context.ArticleProducts.Where(w => w.ProductId == model.ProductId).ToListAsync();
            context.ArticleProducts.RemoveRange(articleProduct);

            if (model.ArticleSelected != null)
            {
                foreach (var item in model.ArticleSelected.Distinct())
                {

                    var obj = new ArticleProduct()
                    {
                        ArticleId = item,
                        ProductId = model.ProductId,
                        CreateDate = DateTime.Now,
                    };

                    await context.ArticleProducts.AddAsync(obj);


                }
            }


            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        #endregion

        #region Video - Product
        public async Task<ShopActionResult<VideoProductInputDto>> GetVideoProductById(int id)
        {
            var result = new ShopActionResult<VideoProductInputDto>();
            var videos = await context.VideoProducts.Include(i => i.Video).Where(w => w.ProductId == id).Select(s => new VideoSelectedDto
            {
                VideoId = s.VideoId,
                VideoTitle = s.Video.Title,
            }).ToListAsync();

            var model = new VideoProductInputDto()
            {
                ProductId = id,
                Videos = videos
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }
        public async Task<ShopActionResult<int>> AddVideoProduct(VideoProductInputDto model)
        {
            var result = new ShopActionResult<int>();
            var articleProduct = await context.VideoProducts.Where(w => w.ProductId == model.ProductId).ToListAsync();
            context.VideoProducts.RemoveRange(articleProduct);

            if (model.VideoSelected != null)
            {
                foreach (var item in model.VideoSelected.Distinct())
                {

                    var obj = new VideoProduct()
                    {
                        VideoId = item,
                        ProductId = model.ProductId,
                        CreateDate = DateTime.Now,
                    };

                    await context.VideoProducts.AddAsync(obj);
                }
            }

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        #endregion

        #region Similar - Product
        public async Task<ShopActionResult<SimilarProductInputDto>> GetSimilarProductById(int id)
        {
            var result = new ShopActionResult<SimilarProductInputDto>();
            var similarProducts = await context.SimilarProducts.Include(i => i.Product).OrderByDescending(o => o.CreateDate).Where(w => w.ProductId == id).Select(s => new SimilarProductSelectedDto
            {
                ProductId = s.SimilarId,
                ProductTitle = s.Similar.ProductName,
            }).ToListAsync();

            var model = new SimilarProductInputDto()
            {
                ProductId = id,
                Remark = context.SimilarProducts.FirstOrDefault(f => f.ProductId == id)?.Remark,
                SimilarProducts = similarProducts
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }
        public async Task<ShopActionResult<UserSimilarProductDto>> GetUserSimilarProductByTitle(string title)
        {
            title = DataUtility.RemoveDashForTitle(title);
            var product = await context.Products.FirstOrDefaultAsync(f => f.ProductName == title || f.EnName == title);

            var result = new ShopActionResult<UserSimilarProductDto>();
            var data = await context.SimilarProducts.Include(i => i.Similar.ProductCoverAttachments).Include(i => i.Product)
                .Include(i => i.Similar.FinancialProducts).Where(w => w.ProductId == product.Id).ToListAsync();

            var similarProducts = data.Select(s => new SimilarProductSelectedDto
            {
                ProductId = s.SimilarId,
                ProductTitle = s.Similar.ProductName,
                EnTitle = s.Similar.EnName,
                CoverFile = s.Similar.ProductCoverAttachments.Count > 0 ? s.Similar.ProductCoverAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(s.Similar.GetInventoryFromApi, s.Similar.APIAmount, s.Similar.FinancialProducts),
                DiscountedPrice = s.Similar.FinancialProducts.Count() > 0 ? s.Similar.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
            }).ToList();


            if (similarProducts.Count == 0)
            {
                var similardataTemp = await context.Products.Include(i => i.ProductCoverAttachments)
                .Include(i => i.FinancialProducts).Where(w => w.CategoryId == product.CategoryId && w.Id != product.Id).ToListAsync();

                similarProducts = similardataTemp.Select(s => new SimilarProductSelectedDto
                {
                    ProductTitle = s.ProductName,
                    EnTitle = s.EnName,
                    CoverFile = s.ProductCoverAttachments.Count > 0 ? s.ProductCoverAttachments.FirstOrDefault().FilePath : "",
                    Price = GetUserProductPrice(s.GetInventoryFromApi, s.APIAmount, s.FinancialProducts),
                    DiscountedPrice = s.FinancialProducts.Count() > 0 ? s.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                }).ToList();

            }
            var model = new UserSimilarProductDto()
            {
                Id = product.Id,
                SimilarProducts = similarProducts
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }
        public async Task<ShopActionResult<int>> AddSimilarProduct(SimilarProductInputDto model)
        {
            var result = new ShopActionResult<int>();
            var similarProducts = await context.SimilarProducts.Where(w => w.ProductId == model.ProductId).ToListAsync();
            context.SimilarProducts.RemoveRange(similarProducts);

            if (model.ProductSelected != null)
            {
                foreach (var item in model.ProductSelected.Distinct())
                {

                    var obj = new SimilarProduct()
                    {
                        SimilarId = item,
                        ProductId = model.ProductId,
                        CreateDate = DateTime.Now,
                        Remark = model.Remark
                    };

                    await context.SimilarProducts.AddAsync(obj);
                }
            }

            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        #endregion

        #region Financial - Product
        public async Task<ShopActionResult<FinancialProductDto>> GetFinancialProductById(int id)
        {
            var result = new ShopActionResult<FinancialProductDto>();
            var financialProduct = await context.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefaultAsync(w => w.ProductId == id);

            if (financialProduct != null)
            {
                var model = new FinancialProductDto()
                {
                    ProductId = id,
                    DiscountedPrice = financialProduct.DiscountedPrice,
                    Price = financialProduct.Price,
                    FromDate = financialProduct.FromDate != null ? financialProduct.FromDate.Value.ToShamsi().ToString() : "",
                    ToDate = financialProduct.ToDate != null ? financialProduct.ToDate.Value.ToShamsi().ToString() : "",
                    GetInventoryFromApi = financialProduct.GetInventoryFromApi,
                    Remark = financialProduct.Remark
                };
                result.Data = model;
            }



            result.IsSuccess = true;
            return result;
        }
        public async Task<ShopActionResult<List<FinancialProductDto>>> GetFinancialProductList(int productId)
        {
            var result = new ShopActionResult<List<FinancialProductDto>>();

            //int skip = (model.Page - 1) * model.Size;

            //foreach (var item in model.Filtered)
            //{
            //    if (item.column == "fromDate")
            //    {
            //        model.FromDate = item.value;
            //    }
            //    if (item.column == "toDate")
            //    {
            //        model.ToDate = item.value;
            //    }

            //}

            var data = await context.FinancialProducts.Where(w => w.ProductId == productId).ToListAsync();


            result.Data = data.Select(q => new FinancialProductDto
            {
                Id = q.Id,
                ProductId = q.ProductId,
                DiscountedPrice = q.DiscountedPrice,
                Price = q.Price,
                FromDate = q.FromDate != null ? q.FromDate.Value.ToShamsi().ToString() : "",
                ToDate = q.ToDate != null ? q.ToDate.Value.ToShamsi().ToString() : "",
                GetInventoryFromApi = q.GetInventoryFromApi,
                Remark = q.Remark,
                CreateDate = q.CreateDate.ToShamsi().ToString()
                //}).Skip(skip).Take(model.Size).ToList();

            }).ToList();

            result.IsSuccess = true;
            //result.Total = data.Count;
            //result.Size = model.Size;
            //result.Page = model.Page; 
            return result;
        }
        public async Task<ShopActionResult<int>> UpdateFinancialProduct(FinancialProductDto model)
        {
            var result = new ShopActionResult<int>();
            //var financialProduct = await context.FinancialProducts.FirstOrDefaultAsync(w => w.ProductId == model.ProductId);
            //if (financialProduct != null)
            //{
            //    financialProduct.ProductId = model.ProductId;
            //    financialProduct.DiscountedPrice = model.DiscountedPrice;
            //    financialProduct.Price = model.Price;
            //    financialProduct.FromDate = model.FromDate != null ? DateUtility.ConvertToMiladi(model.FromDate) : null;
            //    financialProduct.ToDate = model.ToDate != null ? DateUtility.ConvertToMiladi(model.ToDate) : null;
            //    financialProduct.GetInventoryFromApi = model.GetInventoryFromApi;
            //    financialProduct.Remark = model.Remark;
            //}
            //else
            //{
            var obj = new FinancialProduct()
            {
                CreateDate = DateTime.Now,
                DiscountedPrice = model.DiscountedPrice,
                FromDate = model.FromDate != null ? DateUtility.ConvertToMiladi(model.FromDate) : null,
                ToDate = model.ToDate != null ? DateUtility.ConvertToMiladi(model.ToDate) : null,
                GetInventoryFromApi = model.GetInventoryFromApi,
                Price = model.Price,
                Remark = model.Remark,
                ProductId = model.ProductId
            };

            await context.FinancialProducts.AddAsync(obj);
            //}


            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
        #endregion

        #region Delivery - Product
        public async Task<ShopActionResult<List<DeliveryProductDto>>> GetDeliveryProduct(DeliveryProductFilterDto model = null)
        {
            var result = new ShopActionResult<List<DeliveryProductDto>>();
            int skip = (model.Page - 1) * model.Size;

            foreach (var item in model.Filtered)
            {

                if (item.column == "province")
                {
                    model.Province = item.value != null ? EnumHelpers.ParseEnum<Province>(item.value) : null;
                }
                if (item.column == "deliveryType")
                {
                    model.DeliveryType = item.value != null ? EnumHelpers.ParseEnum<DeliveryType>(item.value) : null;
                }
            }
            var queryResult = await context.DeliveryProducts
                  .Where(w =>
                  w.ProductId == model.ProductId &&
                  (model.DeliveryType == null || w.DeliveryType == model.DeliveryType) &&
                  (model.Province == null || w.Province == model.Province)
                  )
                  .Select(s => new DeliveryProductDto
                  {
                      Id = s.Id,
                      Cost = s.Cost,
                      Count = s.Count,
                      DeliveryType = s.DeliveryType,
                      DeliveryTypeTitle = EnumHelpers.GetNameAttribute<DeliveryType>(s.DeliveryType),
                      GreaterEqual = s.GreaterEqual,
                      NeedToCall = s.NeedToCall,
                      Province = s.Province,
                      ProvinceTitle = EnumHelpers.GetNameAttribute<Province>(s.Province),
                      Remark = s.Remark,
                      SmallerEqual = s.SmallerEqual,
                  }).ToListAsync();

            result.Data = queryResult.Skip(skip).Take(model.Size).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Count;
            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }
        public async Task<ShopActionResult<int>> AddDeliveryProduct(DeliveryProductDto model)
        {
            var result = new ShopActionResult<int>();


            var obj = new DeliveryProduct()
            {
                Cost = model.Cost,
                Count = model.Count,
                CreateDate = DateTime.Now,
                DeliveryType = model.DeliveryType,
                GreaterEqual = model.GreaterEqual,
                NeedToCall = model.NeedToCall,
                ProductId = model.ProductId,
                Remark = model.Remark,
                Province = model.Province,
                SmallerEqual = model.SmallerEqual,
                CountType = model.CountType
            };
            await context.DeliveryProducts.AddAsync(obj);



            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        public async Task<ShopActionResult<int>> UpdateDeliveryProduct(DeliveryProductDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.DeliveryProducts.FirstOrDefaultAsync(f => f.Id == model.Id);
            data.Cost = model.Cost;
            data.Count = model.Count;
            data.Remark = model.Remark;
            data.SmallerEqual = model.SmallerEqual;
            data.GreaterEqual = model.GreaterEqual;
            data.DeliveryType = model.DeliveryType;
            data.NeedToCall = model.NeedToCall;
            data.Province = model.Province;
            data.CountType = model.CountType;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }
        public async Task<ShopActionResult<DeliveryProductDto>> GetDeliveryProductById(int id)
        {
            var result = new ShopActionResult<DeliveryProductDto>();


            var deliveryProduct = await context.DeliveryProducts.FirstOrDefaultAsync(w => w.Id == id);



            var model = new DeliveryProductDto()
            {
                Id = deliveryProduct.Id,
                SmallerEqual = deliveryProduct.SmallerEqual,
                GreaterEqual = deliveryProduct.GreaterEqual,
                DeliveryType = deliveryProduct.DeliveryType,
                Province = deliveryProduct.Province,
                Remark = deliveryProduct.Remark,
                Cost = deliveryProduct.Cost,
                Count = deliveryProduct.Count,
                NeedToCall = deliveryProduct.NeedToCall,
                CountType = deliveryProduct.CountType
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }
        public async Task<ShopActionResult<int>> DeleteDeliveryProduct(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new DeliveryProduct { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }
        #endregion

        #region User - Product
        public async Task<ShopActionResult<List<UserProductDto>>> GetTopVisitedProductList(int count = 8)
        {
            var result = new ShopActionResult<List<UserProductDto>>();

            var data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling && w.IsTopVisited == true && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0))).ToListAsync();
            if (data.Count == 0)
            {

                data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                   .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0))).OrderByDescending(o => o.VisitedCount).ToListAsync();
            }

            result.Data = data.Select(q => new UserProductDto
            {
                Id = q.Id,
                IsTopNew = q.IsTopNew,
                IsTopVisited = q.IsTopVisited,
                EnTitle = q.EnName,
                SaleStatus = q.SaleStatus.Value,
                IsTopSale = q.IsTopSale,
                VisitedCount = q.VisitedCount,
                SeoDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
                SaleCount = q.SaleCount,
                ProductName = q.ProductName,
                CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,

            }).ToList();

            result.IsSuccess = true;
            //result.Total = data.Count;
            //result.Size = model.Size;
            //result.Page = model.Page; 
            return result;
        }

        private long GetUserProductPrice(bool getInventoryFromApi, long? aPIAmount, ICollection<FinancialProduct> financialProducts)
        {
            if (getInventoryFromApi)
            {
                return aPIAmount.HasValue ? aPIAmount.Value : -1;
            };
            return financialProducts.Count > 0 ? financialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().Price : 0;
        }

        public async Task<ShopActionResult<List<UserProductDto>>> GetTopSaleProductList(int count = 8)
        {
            var result = new ShopActionResult<List<UserProductDto>>();

            var data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling && w.IsTopSale == true && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0))).ToListAsync();


            result.Data = data.Select(q => new UserProductDto
            {
                Id = q.Id,
                IsTopNew = q.IsTopNew,
                IsTopVisited = q.IsTopVisited,
                SaleStatus = q.SaleStatus.Value,
                IsTopSale = q.IsTopSale,
                VisitedCount = q.VisitedCount,
                SeoDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
                EnTitle = q.EnName,

                SaleCount = q.SaleCount,
                ProductName = q.ProductName,
                CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,

            }).ToList();

            result.IsSuccess = true;
            //result.Total = data.Count;
            //result.Size = model.Size;
            //result.Page = model.Page; 
            return result;
        }
        public async Task<ShopActionResult<List<UserProductDto>>> GetTopNewProductList(int count = 8)
        {
            var result = new ShopActionResult<List<UserProductDto>>();

            var data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling && w.IsTopNew == true && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0))).ToListAsync();

            if (data.Count == 0)
            {
                data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
               .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling).OrderByDescending(o => o.CreateDate).ToListAsync();
            }

            result.Data = data.Select(q => new UserProductDto
            {
                Id = q.Id,
                IsTopNew = q.IsTopNew,
                EnTitle = q.EnName,
                IsTopVisited = q.IsTopVisited,
                SaleStatus = q.SaleStatus.Value,
                IsTopSale = q.IsTopSale,
                VisitedCount = q.VisitedCount,
                SeoDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
                SaleCount = q.SaleCount,
                ProductName = q.ProductName,
                CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,

            }).ToList();

            result.IsSuccess = true;
            //result.Total = data.Count;
            //result.Size = model.Size;
            //result.Page = model.Page; 
            return result;
        }
        public async Task<ShopActionResult<List<UserProductDto>>> GetUserList(GridQueryModel model = null)
        {

            var result = new ShopActionResult<List<UserProductDto>>();

            int productCategoryId = 0;
            var category = string.Empty;
            var brand = string.Empty;
            var product = string.Empty;
            var productCategory = string.Empty;


            var brandId = new List<int>();
            var featuresValues = new List<string>();
            var featuresTitles = new List<string>();
            var featuresIds = new List<int>();
            var hasInventory = string.Empty;
            var topVisited = string.Empty;
            var topSale = string.Empty;
            var isNew = string.Empty;
            var title = string.Empty;
            var isSpecialOffer = string.Empty;

            int skip = (model.Page - 1) * model.Size;

            foreach (var item in model.Filtered)
            {
                if (item.column == "product" && !String.IsNullOrEmpty(item.value))
                {
                    product = item.value;
                }
                if (item.column == "brand" && !String.IsNullOrEmpty(item.value))
                {
                    brand = item.value;

                    brand = DataUtility.RemoveDashForTitle(brand);
                }
                if (item.column == "category" && !String.IsNullOrEmpty(item.value))
                {
                    category = item.value;
                    category = DataUtility.RemoveDashForTitle(category);

                }
                if (item.column == "productCategory" && !String.IsNullOrEmpty(item.value))
                {
                    productCategory = item.value;
                    productCategory = DataUtility.RemoveDashForTitle(productCategory);

                }
                if (item.column == "title" && !String.IsNullOrEmpty(item.value))
                {
                    title = item.value;
                }
                if (item.column == "productCategoryId" && !String.IsNullOrEmpty(item.value))
                {
                    productCategoryId = Convert.ToInt32(item.value);
                }
                if (item.column == "brandId" && !String.IsNullOrEmpty(item.value))
                {
                    var listStr = item.value.Split(new char[] { ',' }).ToList();
                    brandId = listStr.Select(int.Parse).ToList();
                }
                if (item.column == "hasInventory" && !String.IsNullOrEmpty(item.value))
                {
                    hasInventory = item.value == "true" ? item.value : "";
                }
                if (item.column == "topSale" && !String.IsNullOrEmpty(item.value))
                {
                    topSale = item.value == "true" ? item.value : "";
                }
                if (item.column == "topVisited" && !String.IsNullOrEmpty(item.value))
                {
                    topVisited = item.value == "true" ? item.value : "";
                }
                if (item.column == "isSpecialOffer" && !String.IsNullOrEmpty(item.value))
                {
                    isSpecialOffer = item.value == "true" ? item.value : "";
                }
                if (item.column == "isNew" && !String.IsNullOrEmpty(item.value))
                {
                    isNew = item.value == "true" ? item.value : "";
                }
                if (item.column == "featuresIds" && !String.IsNullOrEmpty(item.value))
                {
                    var listStr = item.value.Split(new char[] { ',' }).ToList();
                    featuresIds = listStr.Select(int.Parse).ToList();
                }
                if (item.column == "featuresValues" && !String.IsNullOrEmpty(item.value))
                {
                    var listStr = item.value.Split(new char[] { ',' }).ToList();
                    featuresValues = listStr;
                }
                if (item.column == "featuresTitles" && !String.IsNullOrEmpty(item.value))
                {
                    var listStr = item.value.Split(new char[] { ',' }).ToList();
                    featuresTitles = listStr;
                }
            }

            var categories = new List<int>();

            //if (productCategoryId != 0)
            //{
            //    categories = await context.ProductCategories.Where(w => w.ParentId == productCategoryId).Select(s => s.Id).ToListAsync();
            //}

            if (!String.IsNullOrEmpty(productCategory))
            {
                var productCategoryIdData = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == productCategory || f.EnName == productCategory);
                if (productCategoryIdData != null)
                    categories = await context.ProductCategories.Where(w => w.ParentId == productCategoryIdData.Id).Select(s => s.Id).ToListAsync();
            }

            if (featuresIds.Count > 0)
            {


                var queryResult = context.Products.Include(i => i.Brand).Include(q => q.Category)
                 .ThenInclude(i => i.CategoryAttachments).Include(i => i.ProductAttachments).Include(q => q.FinancialProducts).Include(i => i.ProductCoverAttachments)
                 .Include(i => i.FeatureValues).ThenInclude(q => q.ProductCategoryFeature)
                 .Where(w => w.IsActive == true &&
                   ((String.IsNullOrEmpty(productCategory) || categories.Contains(w.CategoryId))) &&
                  (brandId.Count == 0 || brandId.Contains(w.BrandId)) &&
                  ((String.IsNullOrEmpty(brand) || w.Brand.Title == brand || w.Brand.EnTitle == brand)) &&
                  ((String.IsNullOrEmpty(category) || w.Category.CategoryName == category || w.Category.EnName == category)) &&
                  (w.SaleStatus == SaleStatus.InProgressSelling));


                queryResult = queryResult.Where(w =>
                               ((String.IsNullOrEmpty(hasInventory) || (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling))) &&
                               ((String.IsNullOrEmpty(topSale) || w.IsTopSale == true)) &&
                               ((String.IsNullOrEmpty(topVisited) || w.IsTopVisited == true)) &&
                               ((String.IsNullOrEmpty(isNew) || w.IsTopNew == true)) &&
                               ((String.IsNullOrEmpty(isSpecialOffer) || w.IsSpecialOffer == true)) &&
                                ((String.IsNullOrEmpty(title) || w.ProductName.Contains(title)))
                               );

                #region FeaturesValues

                for (int i = 0; i < featuresIds.Count; i++)
                {
                    var featureId = featuresIds[i];
                    var featureValue = featuresValues[i];
                    var feature = await context.Features.FirstOrDefaultAsync(q => q.Id == featuresIds[i]);


                    switch (feature.ControlType)
                    {

                        case ControlType.Text:
                            {
                                if (!String.IsNullOrEmpty(featuresValues[i]))
                                {
                                    queryResult = queryResult.Where(q => q.FeatureValues.Any(f => f.ProductCategoryFeature.FeatureId == featureId && f.FeatureValue.Contains(featureValue)));
                                }
                                break;
                            }
                        case ControlType.Number:
                            {
                                if (!String.IsNullOrEmpty(featuresValues[i]))
                                {
                                    int featuresValue = Convert.ToInt32(featuresValues[i]);

                                    if (featuresTitles[i].Contains("featureFrom"))
                                    {
                                        queryResult = queryResult.Where(q => q.FeatureValues.Any(f => f.ProductCategoryFeature.FeatureId == featureId &&
                                        f.FeatureValueNumber >= featuresValue)).AsQueryable();
                                    }
                                    if (featuresTitles[i].Contains("featureTo"))
                                    {
                                        queryResult = queryResult.Where(q => q.FeatureValues.Any(f => f.ProductCategoryFeature.FeatureId == featureId &&
                                       f.FeatureValueNumber <= featuresValue)).AsQueryable();
                                    }
                                }
                                break;
                            }
                        case ControlType.Switch:
                            {
                                if (!String.IsNullOrEmpty(featureValue))
                                {
                                    queryResult = queryResult.Where(q => q.FeatureValues.Any(f => f.ProductCategoryFeature.FeatureId == featureId &&
                                     f.FeatureValue.Equals(featureValue))).AsQueryable();
                                }

                                break;
                            }
                        case ControlType.Tag:
                            {
                                queryResult = queryResult.Where(q => q.FeatureValues.Any(f => f.ProductCategoryFeature.FeatureId == featureId &&
                              f.FeatureValue.Contains(featureValue))).AsQueryable();

                                break;
                            }
                    }


                }

                #endregion
                result.Total = queryResult.ToList().Count();

                result.Data = queryResult.ToList().Select(q => new UserProductDto
                {
                    Id = q.Id,
                    IsTopNew = q.IsTopNew,
                    IsTopVisited = q.IsTopVisited,
                    SaleStatus = q.SaleStatus.Value,
                    IsTopSale = q.IsTopSale,
                    VisitedCount = q.VisitedCount,
                    SeoDescription = q.SeoDescription,
                    SeoTitle = q.SeoTitle,
                    SaleCount = q.SaleCount,
                    EnTitle = q.EnName,
                    ProductName = q.ProductName,
                    BrandName = q.Brand.Title,
                    CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                    Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                    DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                    Inventory = q.GetInventoryFromApi == true ? q.APIQuantity : q.Inventory,
                }).Skip(skip).Take(model.Size).ToList();

            }
            else
            {

                var queryResult = context.Products.Include(i => i.Brand).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments).Include(i => i.ProductAttachments).Include(q => q.FinancialProducts)
                 .Include(i => i.ProductCoverAttachments).Where(w => w.IsActive == true &&
                 //(productCategoryId == 0 || w.CategoryId == productCategoryId || categories.Contains(w.CategoryId)) &&
                   ((String.IsNullOrEmpty(productCategory) || categories.Contains(w.CategoryId))) &&

                   (brandId.Count == 0 || brandId.Contains(w.BrandId))
                  && w.SaleStatus == SaleStatus.InProgressSelling).AsQueryable();

                queryResult = queryResult.Where(w =>
                               ((String.IsNullOrEmpty(hasInventory) || (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling))) &&
                               ((String.IsNullOrEmpty(category) || w.Category.CategoryName == category || w.Category.EnName == category)) &&
                               ((String.IsNullOrEmpty(brand) || w.Brand.Title == brand || w.Brand.EnTitle == brand)) &&
                               ((String.IsNullOrEmpty(isSpecialOffer) || w.IsSpecialOffer == true)) &&
                               ((String.IsNullOrEmpty(topSale) || w.IsTopSale == true)) &&
                               ((String.IsNullOrEmpty(topVisited) || w.IsTopVisited == true)) &&
                               ((String.IsNullOrEmpty(isNew) || w.IsTopNew == true)) &&
                                ((String.IsNullOrEmpty(title) || w.ProductName.Contains(title)))
                               ).AsQueryable();

                result.Total = queryResult.ToList().Count;


                if (result.Total == 0)
                {
                    queryResult = context.Products.Include(i => i.Brand).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments).Include(i => i.ProductAttachments).Include(q => q.FinancialProducts)
                        .Include(i => i.ProductCoverAttachments).Where(w => w.IsActive == true &&
                          //(productCategoryId == 0 || w.CategoryId == productCategoryId || categories.Contains(w.CategoryId)) &&
                          ((String.IsNullOrEmpty(productCategory) || categories.Contains(w.CategoryId))) &&

                          (brandId.Count == 0 || brandId.Contains(w.BrandId))
                         && w.SaleStatus == SaleStatus.InProgressSelling).AsQueryable();
                    if (!String.IsNullOrEmpty(topVisited))
                    {


                        queryResult = queryResult.Where(w =>
                                       ((String.IsNullOrEmpty(hasInventory) || (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling))) &&
                                       ((String.IsNullOrEmpty(category) || w.Category.CategoryName == category || w.Category.EnName == category)) &&
                                       ((String.IsNullOrEmpty(brand) || w.Brand.Title == brand || w.Brand.EnTitle == brand)) &&
                                       ((String.IsNullOrEmpty(title) || w.ProductName.Contains(title))) &&
                                       ((String.IsNullOrEmpty(topSale) || w.IsTopSale == true))

                                       ).OrderByDescending(o => o.VisitedCount).AsQueryable();

                    }

                    if (!String.IsNullOrEmpty(isNew))
                    {
                        queryResult = queryResult.Where(w =>
                         ((String.IsNullOrEmpty(hasInventory) || (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling))) &&
                         ((String.IsNullOrEmpty(category) || w.Category.CategoryName == category || w.Category.EnName == category)) &&
                         ((String.IsNullOrEmpty(brand) || w.Brand.Title == brand || w.Brand.EnTitle == brand)) &&
                         ((String.IsNullOrEmpty(title) || w.ProductName.Contains(title))) &&
                         ((String.IsNullOrEmpty(topSale) || w.IsTopSale == true))
                         ).OrderByDescending(o => o.CreateDate).AsQueryable();
                    }


                }







                result.Data = queryResult.Skip(skip).Take(model.Size).ToList().Select(q => new UserProductDto
                {
                    Id = q.Id,
                    IsTopNew = q.IsTopNew,
                    IsTopVisited = q.IsTopVisited,
                    SaleStatus = q.SaleStatus.Value,
                    IsTopSale = q.IsTopSale,
                    VisitedCount = q.VisitedCount,
                    SeoDescription = q.SeoDescription,
                    SeoTitle = q.SeoTitle,
                    SaleCount = q.SaleCount,
                    ProductName = q.ProductName,
                    BrandName = q.Brand.Title,
                    EnTitle = q.EnName,
                    CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                    Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                    DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                    Inventory = q.GetInventoryFromApi == true ? q.APIQuantity : q.Inventory,
                }).ToList();

            }

            if (!String.IsNullOrEmpty(category) || !String.IsNullOrEmpty(productCategory)
                || !String.IsNullOrEmpty(brand) || !String.IsNullOrEmpty(title)
                )
            {
                var brandTemp = "";
                var categoryTemp = "";
                var productCategoryTemp = "";

                if (!String.IsNullOrEmpty(brand))
                {
                    var brandItems = await context.Brands.FirstOrDefaultAsync(f => f.Title == brand || f.EnTitle == brand);
                    brandTemp = brandItems.Title;
                }

                if (!String.IsNullOrEmpty(category))
                {
                    var categoryItems = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);
                    categoryTemp = categoryItems.CategoryName;
                }
                if (!String.IsNullOrEmpty(productCategory))
                {
                    var categoryHeadItems = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == productCategory || f.EnName == productCategory);
                    productCategoryTemp = categoryHeadItems.CategoryName;
                }


                await context.UserLogSearchForProducts.AddAsync(new UserLogSearchForProduct
                {
                    BrandName = !string.IsNullOrEmpty(brandTemp) ? brandTemp : "",
                    SubCategoryName = !string.IsNullOrEmpty(productCategoryTemp) ? productCategoryTemp : "",
                    CategoryName = !string.IsNullOrEmpty(categoryTemp) ? categoryTemp : "",
                    CreateDate = DateTime.Now,
                    ProductName = !string.IsNullOrEmpty(title) ? title : "",
                });
                await context.SaveChangesAsync();
            }

            result.IsSuccess = true;

            result.Size = model.Size;
            result.Page = model.Page;
            return result;
        }
        public async Task<ShopActionResult<List<string>>> GetAllTitle()
        {
            var result = new ShopActionResult<List<string>>();

            result.Data = await context.Products.Where(q => q.IsActive).Select(s => !string.IsNullOrEmpty(s.EnName) ? s.EnName : s.ProductName).ToListAsync();

            result.IsSuccess = true;

            return result;

        }
        public async Task<ShopActionResult<UserProductDetailsDto>> GetProductDetail(int id)
        {
            var result = new ShopActionResult<UserProductDetailsDto>();

            var item = await context.Products
                 .Include(q => q.ProductAttachments)
                 .Include(q => q.ProductCoverAttachments)
                 .Include(q => q.ProductUsages)
                 .Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                 .Include(q => q.Brand)
                 .Include(q => q.FinancialProducts)
                 .Include(q => q.FeatureValues)
                 .ThenInclude(q => q.ProductCategoryFeature)
                 .ThenInclude(q => q.Feature)
                 .ThenInclude(q => q.Symbol)
                 .SingleOrDefaultAsync(q => q.Id == id);

            var files = new List<string>();

            files.Add(item.ProductCoverAttachments.Count > 0 ? item.ProductCoverAttachments.FirstOrDefault().FilePath : item.Category.CategoryAttachments.Count > 0 ? item.Category.CategoryAttachments.FirstOrDefault().FilePath : "");

            var model = new UserProductDetailsDto
            {
                Remark = item.Remark,
                Id = item.Id,
                SubCategoryId = item.Category.ParentId,
                SubCategoryName = item.Category.ParentId != null ? context.ProductCategories.FirstOrDefault(f => f.Id == item.Category.ParentId.Value).CategoryName : "",
                CategoryName = item.Category.CategoryName,
                Code = item.Code,
                BrandId = item.BrandId,
                BrandName = item.Brand.Title,
                ProductName = item.ProductName,
                CategoryId = item.CategoryId,
                IsActive = item.IsActive,
                Inventory = item.Inventory,
                GetInventoryFromApi = item.GetInventoryFromApi,
                SeoDescription = item.SeoDescription,
                SeoTitle = item.SeoTitle,
                SaleStatus = item.SaleStatus,
                ShortDescription = item.ShortDescription,
                OrginalFileAttachments = files.Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Orginal).Select(u => u.FilePath).ToList())
                                              //.Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.CatalogueAndBrochure).Select(u => u.FilePath).ToList())
                                              .Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList()).ToList(),
                CatalogueAndBrochureFileAttachments = item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.CatalogueAndBrochure).Select(u => u.FilePath).ToList(),
                OtherFileAttachments = item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList(),
                CoverFile = item.ProductCoverAttachments.Count > 0 ? item.ProductCoverAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(item.GetInventoryFromApi, item.APIAmount, item.FinancialProducts),
                DiscountedPrice = item.FinancialProducts.Count > 0 ? item.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                SaleStatusTitle = item.SaleStatus.Value.GetNameAttribute(),

                ProductUsages = item.ProductUsages.Select(u => new ProductUsageDto
                {
                    Id = u.Id,
                    Title = u.Title,
                }).ToList(),

                ProductFeatureValues = item.FeatureValues.Select(f => new CategoryFeatureDto
                {
                    Id = f.ProductCategoryFeatureId,
                    Title = f.ProductCategoryFeature.Feature.Title,
                    ProductCategoryFeatureId = f.ProductCategoryFeatureId,
                    SortOrder = f.ProductCategoryFeature.SortOrder,
                    FeatureValue = f.FeatureValue,
                    FeatureValueNumber = f.FeatureValueNumber,
                    UnitTypeTitle = f.ProductCategoryFeature.Feature.UnitType.GetNameAttribute(),
                    ControlType = f.ProductCategoryFeature.Feature.ControlType,
                    UnitType = f.ProductCategoryFeature.Feature.UnitType,
                    SymbolTitle = f.ProductCategoryFeature.Feature.Symbol.Title,
                    Max = f.ProductCategoryFeature.Feature.Max,
                    Min = f.ProductCategoryFeature.Feature.Min,
                    Option = f.ProductCategoryFeature.Feature.Option,
                    FeatureId = f.ProductCategoryFeature.Feature.Id,
                    Value = f.ProductCategoryFeature.Feature.ControlType == ControlType.Number ? f.FeatureValueNumber.ToString() : f.FeatureValue
                }).ToList(),
            };
            model.SimilarModel = new SimilarDto();

            var articleProduct = await context.ArticleProducts.Include(i => i.Article).Include(i => i.Article.ArticleAttachments).OrderBy(o => o.Article.Id).FirstOrDefaultAsync(w => w.ProductId == id);
            if (articleProduct != null)
            {
                model.SimilarModel.SimilarArtcle = new SimilarProductModel()
                {
                    Id = articleProduct.ArticleId,
                    CoverFile = articleProduct.Article.ArticleAttachments.Count > 0 ? articleProduct.Article.ArticleAttachments.FirstOrDefault().FilePath : "",
                    Remark = articleProduct.Article.ShortDescription,
                    Title = articleProduct.Article.Title
                };

            }

            if (articleProduct == null)
            {
                var article = await context.Articles.Include(i => i.ArticleAttachments).OrderByDescending(o => o.Id).FirstOrDefaultAsync(f => f.IsActive == true);
                if (article != null)
                {
                    model.SimilarModel.SimilarArtcle = new SimilarProductModel()
                    {
                        Id = article.Id,
                        CoverFile = article.ArticleAttachments.Count > 0 ? article.ArticleAttachments.FirstOrDefault().FilePath : "",
                        Remark = article.ShortDescription,
                        Title = article.Title
                    };

                }
            }


            var videoProduct = await context.VideoProducts.Include(i => i.Video)
                        .Include(i => i.Video.VideoAttachments).Include(i => i.Video.VideoCoverAttachments).OrderBy(o => o.Video.SortOrder).FirstOrDefaultAsync(f => f.ProductId == id);

            if (videoProduct != null)
            {
                model.SimilarModel.SimilarVideo = new SimilarProductModel()
                {
                    Id = videoProduct.VideoId,
                    CoverFile = videoProduct.Video.VideoCoverAttachments.Count > 0 ? videoProduct.Video.VideoCoverAttachments.FirstOrDefault().FilePath : "",
                    Remark = videoProduct.Video.Remark,
                    Title = videoProduct.Video.Title

                };
            }

            if (videoProduct == null)
            {
                var video = await context.Videos.Include(i => i.VideoAttachments)
                .Include(i => i.VideoCoverAttachments).OrderByDescending(o => o.SortOrder).FirstOrDefaultAsync(f => f.IsActive == true);

                if (video != null)
                {
                    model.SimilarModel.SimilarVideo = new SimilarProductModel()
                    {
                        Id = video.Id,
                        CoverFile = video.VideoCoverAttachments.Count > 0 ? video.VideoCoverAttachments.FirstOrDefault().FilePath : "",
                        Remark = video.Remark,
                        Title = video.Title
                    };
                }
            }



            if (String.IsNullOrEmpty(model.CoverFile))
            {
                model.CoverFile = item.Category.CategoryAttachments.Count > 0 ? item.Category.CategoryAttachments.FirstOrDefault().FilePath : "";
            }

            result.Data = model;
            result.IsSuccess = true;

            return result;

        }

        public async Task<ShopActionResult<UserProductDetailsDto>> GetByTitle(string title)
        {
            var result = new ShopActionResult<UserProductDetailsDto>();
            title = DataUtility.RemoveDashForTitle(title);


            var item = await context.Products

                 .Include(q => q.ProductAttachments)
                 .Include(q => q.ProductCoverAttachments)
                 .Include(q => q.ProductUsages)
                 .Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                 .Include(q => q.Brand)
                 .Include(q => q.FinancialProducts)
                 .Include(q => q.FeatureValues)
                 .ThenInclude(q => q.ProductCategoryFeature)
                 .ThenInclude(q => q.Feature)
                 .ThenInclude(q => q.Symbol)
                 .SingleOrDefaultAsync(q => q.EnName == title || q.ProductName == title);

            var files = new List<string>();

            if (item == null)
            {
                result.IsSuccess = false;
                result.Data = new UserProductDetailsDto();
                return result;
            }

            if (item.GetInventoryFromApi == true)
            {
                logger.LogInformation("call GetPartBalanceInfo with code:{0}", item.Code);
                var callApiResult = await goldiranService.GetPartBalanceInfo(item.Code);
                logger.LogInformation("GetPartBalanceInfo Result with code:{0}, Result{1}", item.Code, callApiResult);

                if (callApiResult.IsSuccess == true)
                {
                    item.UpdateApiDate = DateTime.Now;
                    item.APIAmount = callApiResult.Data.CustomerPrice;
                    item.APIQuantity = callApiResult.Data.LegalAvail;
                }

            }
            await context.SaveChangesAsync();

            //files.Add(item.ProductCoverAttachments.Count > 0 ? item.ProductCoverAttachments.FirstOrDefault().FilePath :
            //    item.Category.CategoryAttachments.Count > 0 ? item.Category.CategoryAttachments.FirstOrDefault().FilePath : "");

            if (item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).Count() == 0)
            {
                files.Add(item.ProductCoverAttachments.FirstOrDefault().FilePath);
            }

            item.VisitedCount = item.VisitedCount + 1;

            var model = new UserProductDetailsDto
            {
                Remark = item.Remark,
                MainFeatureId = item.Category.MainFeatureId,
                Id = item.Id,
                SubCategoryId = item.Category.ParentId,
                SubCategoryName = item.Category.ParentId != null ? context.ProductCategories.FirstOrDefault(f => f.Id == item.Category.ParentId.Value).CategoryName : "",
                CategoryName = item.Category.CategoryName,
                Code = item.Code,
                BrandId = item.BrandId,
                BrandName = item.Brand.Title,
                ProductName = item.ProductName,
                CategoryId = item.CategoryId,
                IsActive = item.IsActive,
                Inventory = item.GetInventoryFromApi == true ? item.APIQuantity : item.Inventory,
                GetInventoryFromApi = item.GetInventoryFromApi,
                SeoDescription = item.SeoDescription,
                SeoTitle = item.SeoTitle,
                SaleStatus = item.SaleStatus,
                ShortDescription = item.ShortDescription,
                EnTitle = item.EnName,
                //OrginalFileAttachments = files.Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Orginal).Select(u => u.FilePath).ToList())
                //                              .Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList()).ToList(),
                OrginalFileAttachments = files.Concat(item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList()).ToList(),

                CatalogueAndBrochureFileAttachments = item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.CatalogueAndBrochure).Select(u => u.FilePath).ToList(),
                OtherFileAttachments = item.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList(),
                CoverFile = item.ProductCoverAttachments.Count > 0 ? item.ProductCoverAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(item.GetInventoryFromApi, item.APIAmount, item.FinancialProducts),
                DiscountedPrice = item.FinancialProducts.Count > 0 ? item.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                SaleStatusTitle = item.SaleStatus.Value.GetNameAttribute(),

                ProductUsages = item.ProductUsages.Select(u => new ProductUsageDto
                {
                    Id = u.Id,
                    Title = u.Title,
                }).ToList(),

                ProductFeatureValues = item.FeatureValues.Select(f => new CategoryFeatureDto
                {
                    Id = f.ProductCategoryFeatureId,
                    Title = f.ProductCategoryFeature.Feature.Title,
                    ProductCategoryFeatureId = f.ProductCategoryFeatureId,
                    SortOrder = f.ProductCategoryFeature.SortOrder,
                    FeatureValue = f.FeatureValue,
                    FeatureValueNumber = f.FeatureValueNumber,
                    UnitTypeTitle = f.ProductCategoryFeature.Feature.UnitType.GetNameAttribute(),
                    ControlType = f.ProductCategoryFeature.Feature.ControlType,
                    UnitType = f.ProductCategoryFeature.Feature.UnitType,
                    SymbolTitle = f.ProductCategoryFeature.Feature.Symbol.Title,
                    Max = f.ProductCategoryFeature.Feature.Max,
                    Min = f.ProductCategoryFeature.Feature.Min,
                    Option = f.ProductCategoryFeature.Feature.Option,
                    FeatureId = f.ProductCategoryFeature.Feature.Id,
                    Value = f.ProductCategoryFeature.Feature.ControlType == ControlType.Number ? f.FeatureValueNumber.ToString() : f.FeatureValue
                }).ToList(),
            };
            model.SimilarModel = new SimilarDto();

            var articleProduct = await context.ArticleProducts.Include(i => i.Article).Include(i => i.Article.ArticleAttachments).OrderBy(o => o.Article.Id).FirstOrDefaultAsync(w => w.ProductId == item.Id);
            if (articleProduct != null)
            {
                model.SimilarModel.SimilarArtcle = new SimilarProductModel()
                {
                    Id = articleProduct.ArticleId,
                    CoverFile = articleProduct.Article.ArticleAttachments.Count > 0 ? articleProduct.Article.ArticleAttachments.FirstOrDefault().FilePath : "",
                    Remark = articleProduct.Article.ShortDescription,
                    Title = articleProduct.Article.Title
                };

            }

            if (articleProduct == null)
            {
                var article = await context.Articles.Include(i => i.ArticleAttachments).OrderByDescending(o => o.Id).FirstOrDefaultAsync(f => f.IsActive == true);
                if (article != null)
                {
                    model.SimilarModel.SimilarArtcle = new SimilarProductModel()
                    {
                        Id = article.Id,
                        CoverFile = article.ArticleAttachments.Count > 0 ? article.ArticleAttachments.FirstOrDefault().FilePath : "",
                        Remark = article.ShortDescription,
                        Title = article.Title
                    };

                }
            }


            var videoProduct = await context.VideoProducts.Include(i => i.Video)
                        .Include(i => i.Video.VideoAttachments).Include(i => i.Video.VideoCoverAttachments).OrderBy(o => o.Video.SortOrder).FirstOrDefaultAsync(f => f.ProductId == item.Id);

            if (videoProduct != null)
            {
                model.SimilarModel.SimilarVideo = new SimilarProductModel()
                {
                    Id = videoProduct.VideoId,
                    CoverFile = videoProduct.Video.VideoCoverAttachments.Count > 0 ? videoProduct.Video.VideoCoverAttachments.FirstOrDefault().FilePath : "",
                    Remark = videoProduct.Video.Remark,
                    Title = videoProduct.Video.Title

                };
            }

            if (videoProduct == null)
            {
                var video = await context.Videos.Include(i => i.VideoAttachments)
                .Include(i => i.VideoCoverAttachments).OrderByDescending(o => o.SortOrder).FirstOrDefaultAsync(f => f.IsActive == true);

                if (video != null)
                {
                    model.SimilarModel.SimilarVideo = new SimilarProductModel()
                    {
                        Id = video.Id,
                        CoverFile = video.VideoCoverAttachments.Count > 0 ? video.VideoCoverAttachments.FirstOrDefault().FilePath : "",
                        Remark = video.Remark,
                        Title = video.Title
                    };
                }
            }



            if (String.IsNullOrEmpty(model.CoverFile))
            {
                model.CoverFile = item.Category.CategoryAttachments.Count > 0 ? item.Category.CategoryAttachments.FirstOrDefault().FilePath : "";
            }


            await context.UserLogSearchForProducts.AddAsync(new UserLogSearchForProduct
            {
                BrandName = model.BrandName,
                SubCategoryName = model.SubCategoryName,
                CategoryName = model.CategoryName,
                CreateDate = DateTime.Now,
                ProductName = model.ProductName,
                ProductId = model.Id
            });
            await context.SaveChangesAsync();

            result.Data = model;
            result.IsSuccess = true;

            return result;
        }

        public async Task<ShopActionResult<SimilarDto>> GetSimilarData(int id)
        {
            var result = new ShopActionResult<SimilarDto>();

            var model = new SimilarDto();

            result.Data = new SimilarDto();

            //result.Data.SimilarArtcle = await context.ArticleProducts.Include(i => i.Article).Include(i => i.Article.ArticleAttachments).Where(w => w.ProductId == id).Select(s => new SimilarProductModel
            //{
            //    File = s.Article.ArticleAttachments.Select(s => s.FilePath).ToList(),
            //    Remark = s.Article.ShortDescription,
            //    Title = s.Article.Title,



            //}).ToListAsync();


            //result.Data.SimilarVideo = await context.VideoProducts.Include(i => i.Video).Include(i => i.Video.VideoAttachments).Where(w => w.ProductId == id).Select(s => new SimilarProductModel
            //{
            //    File = s.Video.VideoAttachments.Select(s => s.FilePath).ToList(),
            //    Remark = s.Video.ShortDescription,
            //    Title = s.Video.Title,



            //}).ToListAsync();


            result.Data = model;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<UserProductDto>>> GetSpecialOfferProductList(int count = 6)
        {
            var result = new ShopActionResult<List<UserProductDto>>();

            var data = await context.Products.Include(i => i.ProductCoverAttachments).Include(i => i.FinancialProducts).Include(q => q.Category).ThenInclude(i => i.CategoryAttachments)
                .Where(w => w.IsActive == true && w.SaleStatus == SaleStatus.InProgressSelling && w.IsSpecialOffer == true && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0))).ToListAsync();


            result.Data = data.Select(q => new UserProductDto
            {
                Id = q.Id,
                IsTopNew = q.IsTopNew,
                EnTitle = q.EnName,
                IsTopVisited = q.IsTopVisited,
                SaleStatus = q.SaleStatus.Value,
                IsTopSale = q.IsTopSale,
                VisitedCount = q.VisitedCount,
                SeoDescription = q.SeoDescription,
                SeoTitle = q.SeoTitle,
                SaleCount = q.SaleCount,
                ProductName = q.ProductName,
                CoverFile = q.ProductCoverAttachments.Count > 0 ? q.ProductCoverAttachments.FirstOrDefault().FilePath : q.Category.CategoryAttachments.Count > 0 ? q.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(q.GetInventoryFromApi, q.APIAmount, q.FinancialProducts),
                DiscountedPrice = q.FinancialProducts.Count > 0 ? q.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,

            }).ToList();

            result.IsSuccess = true;
            return result;

        }

        public async Task<ShopActionResult<List<UserProductDto>>> GetProductsForParentItemsMegaMenu(GridQueryModel filterModel = null)
        {
            var result = new ShopActionResult<List<UserProductDto>>();
            var items = new List<UserProductDto>();
            var category = string.Empty;
            var brand = string.Empty;
            var product = string.Empty;

            int productCategoryId = 0;
            var brandId = new List<int>();
            var hasInventory = string.Empty;
            var topVisited = string.Empty;
            var topSale = string.Empty;
            var isNew = string.Empty;
            var title = string.Empty;

            int skip = (filterModel.Page - 1) * filterModel.Size;

            foreach (var item in filterModel.Filtered)
            {
                if (item.column == "product" && !String.IsNullOrEmpty(item.value))
                {
                    product = DataUtility.RemoveDashForTitle(item.value);
                }
                if (item.column == "brand" && !String.IsNullOrEmpty(item.value))
                {
                    brand = DataUtility.RemoveDashForTitle(item.value);
                }
                if (item.column == "category" && !String.IsNullOrEmpty(item.value))
                {
                    category = DataUtility.RemoveDashForTitle(item.value);

                }
                if (item.column == "title" && !String.IsNullOrEmpty(item.value))
                {
                    title = item.value;
                }
                if (item.column == "productCategoryId" && !String.IsNullOrEmpty(item.value))
                {
                    productCategoryId = Convert.ToInt32(item.value);
                }
                if (item.column == "brandId" && !String.IsNullOrEmpty(item.value))
                {
                    var listStr = item.value.Split(new char[] { ',' }).ToList();
                    brandId = listStr.Select(int.Parse).ToList();
                }
                if (item.column == "hasInventory" && !String.IsNullOrEmpty(item.value))
                {
                    hasInventory = item.value == "true" ? item.value : "";
                }
                if (item.column == "topSale" && !String.IsNullOrEmpty(item.value))
                {
                    topSale = item.value == "true" ? item.value : "";
                }
                if (item.column == "topVisited" && !String.IsNullOrEmpty(item.value))
                {
                    topVisited = item.value == "true" ? item.value : "";
                }
                if (item.column == "isNew" && !String.IsNullOrEmpty(item.value))
                {
                    isNew = item.value == "true" ? item.value : "";
                }
            }

            var children = new List<TreeDto>();



            var categoryItem = await context.ProductCategories.FirstOrDefaultAsync(f => f.CategoryName == category || f.EnName == category);

            if (categoryItem != null)
            {
                var childRoles = await context.ProductCategories.Where(q => q.ParentId == categoryItem.Id).Where(q => q.IsActive).OrderBy(o => o.SortOrder)
                    .Select(q => new TreeItemDto { Id = q.Id, Title = q.CategoryName, ParentId = q.ParentId }).ToListAsync();


                foreach (var item in childRoles)
                {
                    var child = new TreeDto
                    {
                        ParentId = productCategoryId,
                        Key = item.Id,
                        Title = item.Title,
                        Value = item.Id,
                        Text = item.Title,
                        Children = await GetProducts(item.Id),
                    };

                    children.Add(child);
                }

                foreach (var item in children)
                {
                    if (item.Children.Count > 0)
                    {
                        foreach (var obj in item.Children)
                        {
                            var productItem = await context.Products.Include(i => i.Brand).Include(q => q.Category)
                                .ThenInclude(i => i.CategoryAttachments).Include(i => i.ProductAttachments).Include(q => q.FinancialProducts)
                                .Include(i => i.ProductCoverAttachments).FirstOrDefaultAsync(w => w.Id == obj.Key && w.IsActive == true &&
                               ((String.IsNullOrEmpty(hasInventory) || (w.Inventory != null && w.Inventory.Value > 0 && w.SaleStatus == SaleStatus.InProgressSelling))) &&
                               ((String.IsNullOrEmpty(topSale) || w.IsTopSale == true)) &&
                               ((String.IsNullOrEmpty(topVisited) || w.IsTopVisited == true)) &&
                               ((String.IsNullOrEmpty(isNew) || w.IsTopNew == true))
                                );
                            if (productItem != null)
                            {
                                var model = new UserProductDto()
                                {
                                    Id = productItem.Id,
                                    IsTopNew = productItem.IsTopNew,
                                    IsTopVisited = productItem.IsTopVisited,
                                    SaleStatus = productItem.SaleStatus.Value,
                                    IsTopSale = productItem.IsTopSale,
                                    VisitedCount = productItem.VisitedCount,
                                    SeoDescription = productItem.SeoDescription,
                                    SeoTitle = productItem.SeoTitle,
                                    SaleCount = productItem.SaleCount,
                                    ProductName = productItem.ProductName,
                                    BrandName = productItem.Brand.Title,
                                    CoverFile = productItem.ProductCoverAttachments.Count > 0 ? productItem.ProductCoverAttachments.FirstOrDefault().FilePath : productItem.Category.CategoryAttachments.Count > 0 ? productItem.Category.CategoryAttachments.FirstOrDefault().FilePath : "",
                                    Price = GetUserProductPrice(productItem.GetInventoryFromApi, productItem.APIAmount, productItem.FinancialProducts),
                                    DiscountedPrice = productItem.FinancialProducts.Count > 0 ? productItem.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                                    Inventory = productItem.Inventory,
                                    EnTitle = productItem.EnName
                                };
                                items.Add(model);

                            }

                        }
                    }

                }

                result.Data = items.Skip(skip).Take(filterModel.Size).ToList();


            }


            result.IsSuccess = true;
            result.Total = items.Count;
            result.Size = filterModel.Size;
            result.Page = filterModel.Page;
            return result;

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


        public async Task<ShopActionResult<int>> GetSpecialOfferProductCount()
        {
            var result = new ShopActionResult<int>();


            var items = await context.Products.Include(q => q.FinancialProducts)
                .Where(w => w.IsSpecialOffer == true && w.IsActive == true && (w.GetInventoryFromApi == true ? w.APIQuantity > 0 : (w.Inventory != null && w.Inventory.Value > 0)) && w.SaleStatus == SaleStatus.InProgressSelling).CountAsync();


            result.IsSuccess = true;
            result.Data = items;
            return result;
        }


        public async Task<ShopActionResult<int>> AddFavoriteProduct(FavoriteProductModel model, Guid userId)
        {
            var result = new ShopActionResult<int>();

            if (model.IsSelected == true)
            {
                var obj = new FavoriteProduct()
                {
                    CreateDate = DateTime.Now,
                    ProductId = model.ProductId,
                    UserId = userId

                };

                await context.FavoriteProducts.AddAsync(obj);

                //result.Message = MessagesFA.AddFavoriteProduct;

            }
            else
            {
                var product = await context.FavoriteProducts.FirstOrDefaultAsync(f => f.ProductId == model.ProductId && f.UserId == userId);
                context.FavoriteProducts.Remove(product);

                //result.Message = MessagesFA.DeleteFavoriteProduct;

            }
            await context.SaveChangesAsync();
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<FavoriteProductDto>>> GetAllFavoriteProduct(Guid userId)
        {
            var result = new ShopActionResult<List<FavoriteProductDto>>();



            var data = await context.FavoriteProducts.Include(i => i.Product).Include(i => i.Product.Brand).Include(i => i.Product.ProductCoverAttachments).Include(i => i.Product.FinancialProducts).Include(q => q.Product.Category).ThenInclude(i => i.CategoryAttachments)
                .Where(w => w.Product.IsActive == true && w.Product.SaleStatus == SaleStatus.InProgressSelling && w.UserId == userId && (w.Product.GetInventoryFromApi == true ? w.Product.APIQuantity > 0 : (w.Product.Inventory != null && w.Product.Inventory.Value > 0))).ToListAsync();



            result.Data = data.Select(item => new FavoriteProductDto
            {
                ProductId = item.Product.Id,
                Remark = item.Product.Remark,
                EnTitle = item.Product.EnName,
                Id = item.Id,
                SubCategoryId = item.Product.Category.ParentId,
                SubCategoryName = item.Product.Category.ParentId != null ? context.ProductCategories.FirstOrDefault(f => f.Id == item.Product.Category.ParentId.Value).CategoryName : "",
                CategoryName = item.Product.Category.CategoryName,
                Code = item.Product.Code,
                BrandId = item.Product.BrandId,
                BrandName = item.Product.Brand.Title,
                ProductName = item.Product.ProductName,
                CategoryId = item.Product.CategoryId,
                IsActive = item.Product.IsActive,
                Inventory = item.Product.Inventory,
                GetInventoryFromApi = item.Product.GetInventoryFromApi,
                SeoDescription = item.Product.SeoDescription,
                SeoTitle = item.Product.SeoTitle,
                SaleStatus = item.Product.SaleStatus,
                ShortDescription = item.Product.ShortDescription,
                OrginalFileAttachments = item.Product.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Orginal).Select(u => u.FilePath).ToList()
                                              .Concat(item.Product.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList()).ToList(),
                CatalogueAndBrochureFileAttachments = item.Product.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.CatalogueAndBrochure).Select(u => u.FilePath).ToList(),
                OtherFileAttachments = item.Product.ProductAttachments.Where(w => w.ProductAttachmentType == ProductAttachmentType.Other).Select(u => u.FilePath).ToList(),
                CoverFile = item.Product.ProductCoverAttachments.Count > 0 ? item.Product.ProductCoverAttachments.FirstOrDefault().FilePath : "",
                Price = GetUserProductPrice(item.Product.GetInventoryFromApi, item.Product.APIAmount, item.Product.FinancialProducts),
                DiscountedPrice = item.Product.FinancialProducts.Count > 0 ? item.Product.FinancialProducts.OrderByDescending(o => o.CreateDate).FirstOrDefault().DiscountedPrice : 0,
                SaleStatusTitle = item.Product.SaleStatus.Value.GetNameAttribute(),




            }).ToList();

            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<bool>> GetByIdFavoriteProduct(Guid userId, int productId)
        {
            var result = new ShopActionResult<bool>();

            var favoriteProducts = await context.FavoriteProducts.FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == userId);

            if (favoriteProducts != null)
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }

            result.IsSuccess = true;
            return result;
        }



        #endregion





    }

}
