using Application.Interfaces.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.EIED;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class ComboInfoService : IComboInfoService
    {
        private readonly BIContext context;
        private readonly ILogger logger;
        private readonly IApplicationConfigService applicationConfigService;
        private readonly Configs configs;

        public ComboInfoService(BIContext context,
            ILogger<ComboInfoService> logger,
            IApplicationConfigService applicationConfigService,
            IOptions<Configs> options)
        {
            this.context = context;
            this.logger = logger;
            this.applicationConfigService = applicationConfigService;
            this.configs = options.Value;
        }

        private List<EIEDEnums> EnumNamedValues<T>() where T : System.Enum
        {
            var result = new List<EIEDEnums>();
            var values = Enum.GetValues(typeof(T));

            foreach (int item in values)
            {
                var model = new EIEDEnums
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute((T)Enum.Parse(typeof(T), item.ToString())),
                };
                result.Add(model);
            }
            result = result.OrderBy(q => q.Text).ToList();
            return result;
        }

        #region GetComboInfo
        public async Task<List<ComboItemDto>> GetSmsType()
        {
            var list = new List<ComboItemDto>();

            var values = Enum.GetValues(typeof(SmsType)).Cast<SmsType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<SmsType>(item).ToString(),
                };
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        ///  های شاخص Dropdown 
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<ComboInfoDto>> GetComboInfo(Guid userId)
        {
            var result = new ShopActionResult<ComboInfoDto>();
            try
            {
                var discpilines = await GetDisciplines();
                var roles = await GetRoles();
                var organizationLevel = await GetOrganizationLevel();

                result.Data = new ComboInfoDto();
                result.Data.UnitTypes = EnumNamedValues<UnitType>();
                result.Data.ControlTypes = EnumNamedValues<ControlType>();
                result.Data.Discpilines = discpilines.Data;
                result.Data.OrganizationLevel = organizationLevel.Data;
                result.Data.PersonnelPositions = EnumNamedValues<PersonnelPosition>();
                result.Data.UserTypes = EnumNamedValues<UserType>();
                result.Data.EquipmentTypes = EnumNamedValues<EquipmentType>();
                result.Data.Roles = roles;
               // result.Data.Months = DateUtility.GetShamsiMonths();
                result.Data.ApplicationConfig = applicationConfigService.Get().Result.Data;
                var currentYear = DateTime.Now.ToShamsiYear();

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetOrganizationLevel()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(OrganizationLevel)).Cast<OrganizationLevel>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<OrganizationLevel>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetFeatureCategory()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            result.Data = await context.FeatureCategories
               .Select(q => new ComboItemDto
               {
                   Value = q.Id,
                   Text = q.Title
               }).AsNoTracking().ToListAsync();
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        ///     برگرداندن  GetProducts  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetProducts(int categoryId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Products.Where(q => q.IsActive == true && q.CategoryId == categoryId).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.ProductName,

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }




        /// <summary>
        ///     برگرداندن  GetCities  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetCities(Province provinceId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Cities.Where(q => q.IsActive == true && q.ProvinceId == provinceId).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,
                    Label = x.Title
                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }




        /// <summary>
        ///     برگرداندن  فرصت های شغلی  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetJobOpportunities()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.JobOpportunities.Where(q => q.IsActive == true).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,
                    Label = x.Title
                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }

        /// <summary>
        ///     برگرداندن  GetProductsByCategoryIdForUser  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetProductsByCategoryIdForUser(int categoryId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Products.Where(q => q.IsActive == true && q.CategoryId == categoryId).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.ProductName,

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }


        /// <summary>
        ///     برگرداندن  GetAllProductsForUser  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetAllProductsForUser()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Products.Where(q => q.IsActive == true && q.SaleStatus == SaleStatus.InProgressSelling).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.ProductName,
                    Label = x.ProductName

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
               // result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }







        /// <summary>
        ///     برگرداندن  Articles  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetArticles(int categoryId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Articles.Where(q => q.IsActive == true && q.ArticleCategoryId == categoryId).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }



        /// <summary>
        ///    get Disciplines in roles with parentId - بخش id  برگرداندن امور با  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetDisciplinesById(int id)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Roles.Where(q => (q.ParentId == id) && q.IsActive).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.RoleName

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }








        /// <summary>
        ///    بخش 
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetDisciplines(int? organizationId = null)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Roles.Where(q => (organizationId == null || q.ParentId == organizationId) && q.OrganizationLevel == OrganizationLevel.Discipline && q.IsActive).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.RoleName

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }





        /// <summary>
        ///     Brands
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetBrands()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Brands.Where(q => q.IsActive == true).OrderBy(o => o.SortOrder).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,
                    Label = x.Title,
                    EnTitle = x.EnTitle,
                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }


        /// <summary>
        ///     VideoCategory
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetVideoCategory()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.VideoCategories.Where(q => q.IsActive == true).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }


        public async Task<ShopActionResult<List<ComboItemDto>>> GetSaleStatus()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(SaleStatus)).Cast<SaleStatus>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<SaleStatus>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }


        public async Task<ShopActionResult<List<ComboItemDto>>> GetPagesLinkTypes()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(PagesLinkType)).Cast<PagesLinkType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<PagesLinkType>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }


        public async Task<ShopActionResult<List<ComboItemDto>>> GetOrderStatus()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<OrderStatus>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetShowStatus()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(ShowStatus)).Cast<ShowStatus>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<ShowStatus>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }



        public async Task<ShopActionResult<List<ComboItemDto>>> GetUserTypes()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<UserType>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }





        public async Task<ShopActionResult<List<ComboItemDto>>> GetPositionPlace()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(PositionPlace)).Cast<PositionPlace>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<PositionPlace>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetVideoSource()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(VideoSource)).Cast<VideoSource>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<VideoSource>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetProvince()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(Province)).Cast<Province>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<Province>(item).ToString(),
                    Label = EnumHelpers.GetNameAttribute<Province>(item).ToString()
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetDeliveryType()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<DeliveryType>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetUserOpinionType()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(UserOpinionType)).Cast<UserOpinionType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<UserOpinionType>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetCountType()
        {
            var list = new List<ComboItemDto>();
            var result = new ShopActionResult<List<ComboItemDto>>();

            var values = Enum.GetValues(typeof(CountType)).Cast<CountType>().ToList();

            foreach (var item in values)
            {
                var model = new ComboItemDto
                {
                    Value = (int)item,
                    Text = EnumHelpers.GetNameAttribute<CountType>(item).ToString(),
                };
                list.Add(model);
            }
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }




        /// <summary>
        ///   Origins
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetOrigins()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Origins.Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }


        /// <summary>
        ///   ArticleCategory
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<ComboItemDto>>> GetArticleCategory()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.ArticleCategories.Where(w => w.IsActive == true).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }




        private async Task<List<ComboItemDto>> GetRoles()
        {
            var result = await context.Roles.Select(q => new ComboItemDto
            {
                Value = q.Id,
                Text = q.RoleName
            }).AsNoTracking().ToListAsync();

            return result;
        }


        public async Task<List<ComboItemDto>> GetRolesCombo()
        {
            var result = await context.Roles.Select(q => new ComboItemDto
            {
                Value = q.Id,
                Text = q.RoleName
            }).AsNoTracking().ToListAsync();

            return result;
        }


        public async Task<ShopActionResult<List<ComboItemDto>>> GetSymbolWithOutParent()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Symbols.Where(w => w.ParentId == null && w.IsActive == true).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title,

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetVideos(int categoryId)
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.Videos.Where(q => q.IsActive == true && q.VideoCategoryId == categoryId).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.Title

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetProductCategory()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            try
            {

                var data = await context.ProductCategories.Where(q => q.IsActive == true && q.ParentId == null).Select(x => new ComboItemDto
                {
                    Value = x.Id,
                    Text = x.CategoryName,
                    Label = x.CategoryName

                }).ToListAsync();
                result.Data = data;

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.ErrorForGetData;
                return result;
            }
        }

        #endregion
    }
}
