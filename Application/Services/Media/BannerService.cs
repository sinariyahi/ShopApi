using Application.Interfaces.Media;
using Application.Interfaces;
using Domain.Entities.Media;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Media;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Media
{
    public class BannerService : IBannerService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Banner> bannerQueryService;
        private readonly IFileService _fileService;
        private readonly string _filePath;
        private readonly Configs _configs;
        public BannerService(BIContext context,
            IGenericQueryService<Banner> bannerQueryService,
                  IFileService fileService, IOptions<Configs> options
            )
        {
            this.context = context;
            this.bannerQueryService = bannerQueryService;
            _fileService = fileService;
            _filePath = options.Value.FilePath;
            _configs = options.Value;
        }

        public async Task<ShopActionResult<int>> Add(BannerDto model)
        {
            var result = new ShopActionResult<int>();
            var item = new Banner
            {
                Remark = model.Remark,
                CreateDate = DateTime.Now,
                Title = model.Title,
                IsActive = model.IsActive,
                SortOrder = model.SortOrder,
                Link = model.Link,
                SeoTitle = model.SeoTitle,
                PositionPlace = model.PositionPlace,
                FromDate = !String.IsNullOrEmpty(model.FromDate) ? DateUtility.ConvertToMiladi(model.FromDate).Value : DateTime.Now,
                ToDate = !String.IsNullOrEmpty(model.ToDate) ? DateUtility.ConvertToMiladi(model.ToDate).Value : DateTime.Now,
            };
            await context.AddAsync(item);
            await context.SaveChangesAsync();

            #region Attachments
            if (model.Files != null)
            {

                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "BannerAttachments");

                for (int i = 0; i < model.Files.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.Files[i], "Media", "BannerAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }
            }



            #endregion



            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new Banner { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<BannerDto>> GetById(int id)
        {
            var result = new ShopActionResult<BannerDto>();

            var item = await context.Banners
                            .Include(i => i.BannerAttachments)
                .SingleOrDefaultAsync(q => q.Id == id);

            var model = new BannerDto()
            {
                CreateDate = DateTime.Now,
                FileAttachment = item.BannerAttachments.Select(s => new FileItemDto { Entity = "BannerAttachments", FilePath = s.FilePath }).ToList(),
                FromDate = item.FromDate.ToShamsi().ToString(),
                ToDate = item.ToDate.ToShamsi().ToString(),
                IsActive = item.IsActive,
                Id = id,
                Link = item.Link,
                PositionPlace = item.PositionPlace,
                Remark = item.Remark,
                SeoTitle = item.SeoTitle,
                SortOrder = item.SortOrder,
                Title = item.Title
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<BannerDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<BannerDto>>();

            var queryResult = await bannerQueryService.QueryAsync(model, includes: new List<string> { "BannerAttachments" });

            result.Data = queryResult.Data.Select(item => new BannerDto
            {
                CreateDate = DateTime.Now,
                FileAttachment = item.BannerAttachments.Select(s => new FileItemDto { Entity = "BannerAttachments", FilePath = s.FilePath }).ToList(),
                FromDate = item.FromDate.ToShamsi().ToString(),
                ToDate = item.ToDate.ToShamsi().ToString(),
                IsActive = item.IsActive,
                IsActiveTitle = item.IsActive == true ? "فعال" : "غیر فعال",
                Id = item.Id,
                Link = item.Link,
                PositionPlace = item.PositionPlace,
                Remark = item.Remark,
                SeoTitle = item.SeoTitle,
                SortOrder = item.SortOrder,
                Title = item.Title,
                PositionPlaceTitle = EnumHelpers.GetNameAttribute<PositionPlace>(item.PositionPlace)

            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }





        public async Task<ShopActionResult<UserBannerDto>> GetUserList(PositionPlace positionPlace)
        {
            var result = new ShopActionResult<UserBannerDto>();
            result.Data = new UserBannerDto();


            switch (positionPlace)
            {
                case PositionPlace.PartOne:
                    result.Data.List = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).Where(w => w.PositionPlace == positionPlace)
                          .Select(s => new UserModelBannerDto
                          {
                              Id = s.Id,
                              PositionPlace = s.PositionPlace,
                              Link = s.Link,
                              Title = s.Title,
                              SeoTitle = s.SeoTitle,
                              FileAttachment = s.BannerAttachments.Select(s => s.FilePath).ToList(),
                          }).ToListAsync();
                    break;
                case PositionPlace.PartTwo:
                    var data = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (data != null)
                    {
                        var model = new UserModelBannerDto
                        {
                            Id = data.Id,
                            PositionPlace = data.PositionPlace,
                            Link = data.Link,
                            Title = data.Title,
                            SeoTitle = data.SeoTitle,
                            FileAttachment = data.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = model;
                    }

                    break;
                case PositionPlace.PartThree:
                    result.Data.List = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).Where(w => w.PositionPlace == positionPlace)
                          .Select(s => new UserModelBannerDto
                          {
                              Id = s.Id,
                              PositionPlace = s.PositionPlace,
                              Link = s.Link,
                              Title = s.Title,
                              SeoTitle = s.SeoTitle,
                              FileAttachment = s.BannerAttachments.Select(s => s.FilePath).ToList(),
                          }).ToListAsync();
                    break;
                case PositionPlace.PartFour:
                    result.Data.List = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).Where(w => w.PositionPlace == positionPlace)
                          .Select(s => new UserModelBannerDto
                          {
                              Id = s.Id,
                              PositionPlace = s.PositionPlace,
                              Link = s.Link,
                              Title = s.Title,
                              SeoTitle = s.SeoTitle,
                              FileAttachment = s.BannerAttachments.Select(s => s.FilePath).ToList(),
                          }).ToListAsync();
                    break;
                case PositionPlace.ImmediateOffer:
                    var dataModel = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);
                    if (dataModel != null)
                    {
                        var obj = new UserModelBannerDto
                        {
                            Id = dataModel.Id,
                            PositionPlace = dataModel.PositionPlace,
                            Link = dataModel.Link,
                            Title = dataModel.Title,
                            SeoTitle = dataModel.SeoTitle,
                            FileAttachment = dataModel.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = obj;
                    }

                    break;

                case PositionPlace.AboveTheMenu:
                    var dataAboveTheMenuModel = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataAboveTheMenuModel != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataAboveTheMenuModel.Id,
                            PositionPlace = dataAboveTheMenuModel.PositionPlace,
                            Link = dataAboveTheMenuModel.Link,
                            Title = dataAboveTheMenuModel.Title,
                            SeoTitle = dataAboveTheMenuModel.SeoTitle,
                            FileAttachment = dataAboveTheMenuModel.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }

                    break;


                case PositionPlace.OfferForProductCategory:
                    var dataOfferForProductCategory = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForProductCategory != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForProductCategory.Id,
                            PositionPlace = dataOfferForProductCategory.PositionPlace,
                            Link = dataOfferForProductCategory.Link,
                            Title = dataOfferForProductCategory.Title,
                            SeoTitle = dataOfferForProductCategory.SeoTitle,
                            FileAttachment = dataOfferForProductCategory.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }

                    break;

                case PositionPlace.OfferForPiece:
                    var dataOfferForPiece = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForPiece != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForPiece.Id,
                            PositionPlace = dataOfferForPiece.PositionPlace,
                            Link = dataOfferForPiece.Link,
                            Title = dataOfferForPiece.Title,
                            SeoTitle = dataOfferForPiece.SeoTitle,
                            FileAttachment = dataOfferForPiece.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }


                    break;

                case PositionPlace.OfferForPieceCategory:
                    var dataOfferForPieceCategory = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForPieceCategory != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForPieceCategory.Id,
                            PositionPlace = dataOfferForPieceCategory.PositionPlace,
                            Link = dataOfferForPieceCategory.Link,
                            Title = dataOfferForPieceCategory.Title,
                            SeoTitle = dataOfferForPieceCategory.SeoTitle,
                            FileAttachment = dataOfferForPieceCategory.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }


                    break;


                case PositionPlace.OfferForBrand:
                    var dataOfferForBrand = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForBrand != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForBrand.Id,
                            PositionPlace = dataOfferForBrand.PositionPlace,
                            Link = dataOfferForBrand.Link,
                            Title = dataOfferForBrand.Title,
                            SeoTitle = dataOfferForBrand.SeoTitle,
                            FileAttachment = dataOfferForBrand.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }

                    break;



                case PositionPlace.OfferForBrandPiece:
                    var dataOfferForBrandPiece = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForBrandPiece != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForBrandPiece.Id,
                            PositionPlace = dataOfferForBrandPiece.PositionPlace,
                            Link = dataOfferForBrandPiece.Link,
                            Title = dataOfferForBrandPiece.Title,
                            SeoTitle = dataOfferForBrandPiece.SeoTitle,
                            FileAttachment = dataOfferForBrandPiece.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }

                    break;

                case PositionPlace.OfferForBrandProductCategory:
                    var dataOfferForBrandProductCategory = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataOfferForBrandProductCategory != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataOfferForBrandProductCategory.Id,
                            PositionPlace = dataOfferForBrandProductCategory.PositionPlace,
                            Link = dataOfferForBrandProductCategory.Link,
                            Title = dataOfferForBrandProductCategory.Title,
                            SeoTitle = dataOfferForBrandProductCategory.SeoTitle,
                            FileAttachment = dataOfferForBrandProductCategory.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var immediateOffer = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == PositionPlace.ImmediateOffer);
                        if (immediateOffer != null)
                        {
                            var obj = new UserModelBannerDto
                            {
                                Id = immediateOffer.Id,
                                PositionPlace = immediateOffer.PositionPlace,
                                Link = immediateOffer.Link,
                                Title = immediateOffer.Title,
                                SeoTitle = immediateOffer.SeoTitle,
                                FileAttachment = immediateOffer.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = obj;
                        }

                    }


                    break;



                case PositionPlace.ProductDetails:
                    var dataProductDetails = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataProductDetails != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataProductDetails.Id,
                            PositionPlace = dataProductDetails.PositionPlace,
                            Link = dataProductDetails.Link,
                            Title = dataProductDetails.Title,
                            SeoTitle = dataProductDetails.SeoTitle,
                            FileAttachment = dataProductDetails.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }
                    else
                    {
                        var dataAbove = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                        if (dataAbove != null)
                        {
                            var dataValues = new UserModelBannerDto
                            {
                                Id = dataAbove.Id,
                                PositionPlace = dataAbove.PositionPlace,
                                Link = dataAbove.Link,
                                Title = dataAbove.Title,
                                SeoTitle = dataAbove.SeoTitle,
                                FileAttachment = dataAbove.BannerAttachments.Select(s => s.FilePath).ToList(),

                            };

                            result.Data.Model = dataValues;
                        }

                    }


                    break;

                case PositionPlace.BrandFeatures:
                    var dataBrandFeatures = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataBrandFeatures != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataBrandFeatures.Id,
                            PositionPlace = dataBrandFeatures.PositionPlace,
                            Link = dataBrandFeatures.Link,
                            Title = dataBrandFeatures.Title,
                            SeoTitle = dataBrandFeatures.SeoTitle,
                            FileAttachment = dataBrandFeatures.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }

                    break;

                case PositionPlace.ProductFeatures:
                    var dataProductFeatures = await context.Banners.Include(i => i.BannerAttachments).OrderByDescending(o => o.SortOrder).ThenByDescending(t => t.CreateDate).FirstOrDefaultAsync(w => w.PositionPlace == positionPlace);

                    if (dataProductFeatures != null)
                    {
                        var dataValues = new UserModelBannerDto
                        {
                            Id = dataProductFeatures.Id,
                            PositionPlace = dataProductFeatures.PositionPlace,
                            Link = dataProductFeatures.Link,
                            Title = dataProductFeatures.Title,
                            SeoTitle = dataProductFeatures.SeoTitle,
                            FileAttachment = dataProductFeatures.BannerAttachments.Select(s => s.FilePath).ToList(),

                        };

                        result.Data.Model = dataValues;
                    }

                    break;


            }


            result.IsSuccess = true;

            return result;
        }




        public async Task<ShopActionResult<int>> Update(BannerDto model)
        {
            var result = new ShopActionResult<int>();

            var item = await context.Banners
                            .Include(i => i.BannerAttachments)
                .SingleOrDefaultAsync(q => q.Id == model.Id);


            item.FromDate = !String.IsNullOrEmpty(model.FromDate) ? DateUtility.ConvertToMiladi(model.FromDate).Value : DateTime.Now;
            item.ToDate = !String.IsNullOrEmpty(model.ToDate) ? DateUtility.ConvertToMiladi(model.ToDate).Value : DateTime.Now;
            item.IsActive = model.IsActive;
            item.Id = model.Id;
            item.Link = model.Link;
            item.PositionPlace = model.PositionPlace;
            item.Remark = model.Remark;
            item.SeoTitle = model.SeoTitle;
            item.SortOrder = model.SortOrder;
            item.Title = model.Title;

            await context.SaveChangesAsync();


            if (model.Files != null)
            {
                //context.BannerAttachments.RemoveRange(item.BannerAttachments);
                await _fileService.CreateFolderNewItem(_filePath + "\\" + "Media", "BannerAttachments");

                for (int i = 0; i < model.Files.Count(); i++)
                {
                    await _fileService.SaveNewItemInFolder(model.Files[i], "Media", "BannerAttachments", null, null, item.Id, DateTime.Now, Guid.NewGuid(), false);

                }

            }
           // result.Message = MessagesFA.UpdateSuccessful;

            result.IsSuccess = true;
            return result;
        }
    }
}
