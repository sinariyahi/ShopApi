using Domain.Entities.Base;
using Domain.Entities.Blog;
using Domain.Entities.Catalog;
using Domain.Entities.Media;
using Domain.Entities.Payment;
using Domain.Entities.Security;
using Domain.Entities.Support;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BIContext : DbContext
    {
        public BIContext(DbContextOptions options) : base(options)
        {
        }

        #region Security
        public DbSet<UserSms> UserSmss { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserResetPassword> UserResetPasswords { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserRefreshTokenHistory> UserRefreshTokenHistorys { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupMember> UserGroupMembers { get; set; }
        public DbSet<UserGroupAction> UserGroupActions { get; set; }
        public DbSet<Cartable> Cartables { get; set; }
        public DbSet<UserCartable> UserCartables { get; set; }

        #endregion

        #region Base
        public DbSet<UserOrganizationUnit> UserOrganizationUnits { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandAttachment> BrandAttachments { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<SocialMediaAttachment> SocialMediaAttachments { get; set; }
        public DbSet<JobOpportunity> JobOpportunities { get; set; }

        public DbSet<ApplicationConfig> ApplicationConfigs { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }

       // public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DeliveryLocation> DeliveryLocations { get; set; }
        public DbSet<Page> Pages { get; set; }

        #endregion

        #region Catalog

        public DbSet<Category> ProductCategories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<CategoryFeature> ProductCategoryFeatures { get; set; }
        public DbSet<CategoryAttachment> CategoryAttachments { get; set; }
        public DbSet<CategoryMainFeature> CategoryMainFeatures { get; set; }
        public DbSet<MainFeatureAttachment> MainFeatureAttachments { get; set; }
        public DbSet<UserLogSearchForProduct> UserLogSearchForProducts { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<ProductUsage> ProductUsages { get; set; }
        public DbSet<ProductFeatureValue> ProductFeatureValues { get; set; }
        public DbSet<ProductAttachment> ProductAttachments { get; set; }
        public DbSet<ProductCoverAttachment> ProductCoverAttachments { get; set; }

        public DbSet<FeatureCategory> FeatureCategories { get; set; }
        public DbSet<ProductUserComment> ProductUserComments { get; set; }

        public DbSet<VideoProductAttachment> VideoProductAttachments { get; set; }
        public DbSet<VideoFileProductAttachment> VideoFileProductAttachments { get; set; }
        public DbSet<SimilarProduct> SimilarProducts { get; set; }
        public DbSet<FinancialProduct> FinancialProducts { get; set; }
        public DbSet<DeliveryProduct> DeliveryProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Kopon> Kopons { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }


        #endregion


        #region Blog
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAttachment> ArticleAttachments { get; set; }
        public DbSet<ArticleProduct> ArticleProducts { get; set; }

        #endregion

        #region Meida
        public DbSet<VideoCategory> VideoCategories { get; set; }
        public DbSet<VideoAttachment> VideoAttachments { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoProduct> VideoProducts { get; set; }
        public DbSet<VideoCoverAttachment> VideoCoverAttachments { get; set; }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<BannerAttachment> BannerAttachments { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderAttachment> SliderAttachments { get; set; }


        #endregion

        #region Support
        public DbSet<UserOpinion> UserOpinions { get; set; }
        public DbSet<UserOpinionDetail> UserOpinionDetails { get; set; }
        public DbSet<UserNewsLetter> UserNewsLetters { get; set; }

        public DbSet<CompanyQuestion> CompanyQuestions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<InternalMessage> InternalMessages { get; set; }
        public DbSet<CartableRecord> CartableRecords { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<ContactFormAttachment> ContactFormAttachments { get; set; }
        public DbSet<CooperationForm> CooperationForms { get; set; }
        public DbSet<CooperationFormAttachment> CooperationFormAttachments { get; set; }
        public DbSet<UserLetMeKnow> UserLetMeKnows { get; set; }

        #endregion

        #region Payment
        public DbSet<UserPayment> UserPayments { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductFeatureValue>()
                .HasOne(q => q.Product)
                .WithMany(q => q.FeatureValues)
                .HasForeignKey(q => q.ProductId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductFeatureValue>()
               .HasOne(q => q.ProductCategoryFeature)
               .WithMany()
               .HasForeignKey(q => q.ProductCategoryFeatureId).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SimilarProduct>()
                .HasOne(q => q.Similar)
                .WithMany()
                .HasForeignKey(q => q.SimilarId).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<UserAnswer>()
               .HasOne(q => q.SenderUser)
               .WithMany(q => q.UserAnswers)
               .HasForeignKey(q => q.SenderUserId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserCartable>()
           .HasOne(q => q.RegisterUser)
           .WithMany(q => q.UserCartables)
           .HasForeignKey(q => q.RegisterUserId).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<UserPayment>()
                .HasOne(q => q.User)
                .WithMany(q => q.UserPayments)
                .HasForeignKey(q => q.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
