using Application.Interfaces.Base;
using Application.Interfaces.Blog;
using Application.Interfaces.Catalog;
using Application.Interfaces.Dashboard;
using Application.Interfaces.Media;
using Application.Interfaces.Order;
using Application.Interfaces.Payment;
using Application.Interfaces.PaymentGateway;
using Application.Interfaces.Security;
using Application.Interfaces.Sms;
using Application.Interfaces.Support;
using Application.Interfaces;
using Application.Services.Base;
using Application.Services.Blog;
using Application.Services.Catalog;
using Application.Services.Dashboard;
using Application.Services.Media;
using Application.Services.Payment;
using Application.Services.PaymentGateway;
using Application.Services.Security;
using Application.Services.Sms;
using Application.Services.Support;
using Application.Services;
using Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Shop;
using Application.Services.Shop;

namespace Application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicaiton(this IServiceCollection services)
        {

            #region Dashboard

            services.AddScoped<IDashboardService, DashboardService>();

            #endregion

            #region Security
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserGroupMemberService, UserGroupMemberService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<ICustomerService, CustomerService>();

            #endregion

            #region Sms
            services.AddScoped<ISmsService, SmsService>();
            services.AddSingleton<SMSUtility>();

            #endregion

            #region Base
            services.AddScoped<IComboInfoService, ComboInfoService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IApplicationConfigService, ApplicationConfigService>();
            services.AddScoped<IOriginService, OriginService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<ISocialMediaService, SocialMediaService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<INewsLetterService, NewsLetterService>();
            services.AddScoped<IJobOpportunityService, JobOpportunityService>();

            #endregion

            #region Support
            services.AddScoped<IUserOpinionService, UserOpinionService>();
            services.AddScoped<ILetMeKnowService, LetMeKnowService>();

            #endregion

            #region Catalog
            services.AddScoped<ISymbolService, SymbolService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IFeatureCategoryService, FeatureCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IKoponService, KoponService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICooperationFormService, CooperationFormService>();
            services.AddScoped<IContactFormService, ContactFormService>();

            #endregion

            #region Blog
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
            services.AddScoped<IArticleService, ArticleService>();
            #endregion

            #region Media
            services.AddScoped<IVideoCategoryService, VideoCategoryService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ISliderService, SliderService>();
            #endregion

            #region Goldiran
            services.AddScoped<IShopService, ShopService>();
            #endregion

            #region PaymentGateway

            services.AddScoped<IMellatPaymentService, MellatPaymentService>();
            #endregion

            #region Payment

            services.AddScoped<IUserPaymentService, UserPaymentService>();
            #endregion



            services.AddScoped(typeof(IGenericQueryService<>), typeof(GenericQueryService<>));
            return services;
        }
    }
}
