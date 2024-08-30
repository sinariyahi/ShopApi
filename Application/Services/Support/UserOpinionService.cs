using Application.Interfaces.Support;
using Application.Interfaces;
using Domain.Entities.Support;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Support
{
    public class UserOpinionService : IUserOpinionService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<UserOpinion> queryService;
        public UserOpinionService(BIContext context, IGenericQueryService<UserOpinion> queryService)
        {
            this.context = context;
            this.queryService = queryService;
        }

        public async Task<ShopActionResult<int>> Add(UserOpinionDto model)
        {
            var result = new ShopActionResult<int>();

            var positiveOpinions = new List<UserOpinionModel>();
            var negativeOpinions = new List<UserOpinionModel>();

            foreach (var item in model.PositiveOpinions)
            {
                var obj = new UserOpinionModel();
                obj.Text = item.Text;
                obj.UseOpinionStatus = UseOpinionStatus.PositiveOpinion;

                positiveOpinions.Add(obj);
            }

            foreach (var item in model.NegativeOpinions)
            {
                var obj = new UserOpinionModel();
                obj.Text = item.Text;
                obj.UseOpinionStatus = UseOpinionStatus.NegativeOpinion;

                negativeOpinions.Add(obj);
            }

            var tempUserOpinionDetails = positiveOpinions.Concat(negativeOpinions);


            var userOpinion = new UserOpinion
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Remark = model.Remark,
                ProductId = model.ProductId,
                ArticleId = model.ArticleId,
               // ShowStatus = ShowStatus.NotAllowVisit,
                SenderUserId = model.SenderUserId,
               // UserOpinionType = model.UserOpinionType
            };
            await context.UserOpinions.AddAsync(userOpinion);
            await context.SaveChangesAsync();


            foreach (var item in tempUserOpinionDetails)
            {
                var userOpinionDetailItem = new UserOpinionDetail
                {
                    Remark = item.Text,
                   // UseOpinionStatus = item.UseOpinionStatus,
                    Id = Guid.NewGuid(),
                    UserOpinionId = userOpinion.Id,
                };
                await context.UserOpinionDetails.AddAsync(userOpinionDetailItem);
            }
            await context.SaveChangesAsync();
            result.IsSuccess = true;
            return result;
        }

        public Task<ShopActionResult<int>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ShopActionResult<List<ListUserOpinionDto>>> GetAll(FilterUserOpinionDto model = null)
        {
            var result = new ShopActionResult<List<ListUserOpinionDto>>();
            var title = string.Empty;

            int skip = (model.Page - 1) * model.Size;

            foreach (var item in model.Filtered)
            {
                if (item.column == "title")
                {
                    title = item.value;
                }

                if (item.column == "userOpinionType")
                {
                    model.UseOpinionType = (UserOpinionType)Enum.Parse(typeof(UserOpinionType), item.value);
                }

                if (item.column == "showStatus")
                {
                    model.ShowStatus = (ShowStatus)Enum.Parse(typeof(ShowStatus), item.value);
                }
            }

            var data = await context.UserOpinions.Include(i => i.SenderUser)
                 .Include(i => i.Product).Include(i => i.Article).Where(w => (string.IsNullOrEmpty(title) || w.Product.ProductName.Contains(title) || w.Article.Title.Contains(title))
                 //&&
                 //((model.UseOpinionType == null || w.UserOpinionType == model.UseOpinionType)) &&
                 //((model.ShowStatus == null || w.ShowStatus == model.ShowStatus))
                 ).Select(s => new ListUserOpinionDto
                 {
                     Id = s.Id,
                     Remark = s.Remark,
                     ProductId = s.ProductId,
                     ProductTitle = s.Product.ProductName,
                   //  Status = s.ShowStatus.GetNameAttribute(),
                     SenderUser = s.SenderUser.FullName != null ? s.SenderUser.FullName : s.SenderUser.PhoneNumber != null ? s.SenderUser.PhoneNumber : s.SenderUser.Email,
                     CreateDate = DateUtility.FormatShamsiDateTime(s.CreateDate),
                   //  UserOpinionTypeTitle = s.UserOpinionType.GetNameAttribute(),
                     ArticleTitle = s.Article.Title,
                     Title = s.Article != null ? s.Article.Title : s.Product.ProductName,
                     //NegativeOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.NegativeOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                     //PositiveOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.PositiveOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                 }).ToListAsync();
            result.Total = data.Count;
            result.Data = data.Skip(skip).Take(model.Size).ToList();
            result.Size = model.Size;
            result.Page = model.Page;
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<UserOpinionDto>> GetById(Guid id)
        {
            var result = new ShopActionResult<UserOpinionDto>();

            var data = await context.UserOpinions.Include(i => i.UserOpinionDetails).Include(i => i.SenderUser).FirstOrDefaultAsync(w => w.Id == id);

            result.Data = new UserOpinionDto
            {
                Remark = data.Remark,
               // ShowStatus = data.ShowStatus,
                ProductId = data.ProductId,
              //  SenderUser = data.SenderUser.FullName != null ? data.SenderUser.FullName : "کاربر تامین پلاس",
                CreateDate = data.CreateDate,
                ShamsiCreateDate = DateUtility.FormatShamsiDateTime(data.CreateDate),
             //   NegativeOpinions = data.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.NegativeOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
             //   PositiveOpinions = data.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.PositiveOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
            };


            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<List<UserOpinionDto>>> GetByProductId(int ProductId)
        {
            var result = new ShopActionResult<List<UserOpinionDto>>();

            result.Data = await context.UserOpinions.Include(i => i.UserOpinionDetails).Include(i => i.SenderUser).Where(w => w.ProductId == ProductId
            //&& w.UserOpinionType == UserOpinionType.Products
            //&& w.ShowStatus == ShowStatus.AllowVisit
            )
                .Select(s => new UserOpinionDto
                {
                    Remark = s.Remark,
                    ProductId = s.ProductId,
                    SenderUser = s.SenderUser.FullName != null ? s.SenderUser.FullName : "کاربر تامین پلاس",
                    CreateDate = s.CreateDate,
                   // NegativeOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.NegativeOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                   // PositiveOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.PositiveOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                }).ToListAsync();

            result.IsSuccess = true;
            return result;
        }
        public async Task<ShopActionResult<List<UserOpinionDto>>> GetByArticleId(int articleId)
        {
            var result = new ShopActionResult<List<UserOpinionDto>>();

            result.Data = await context.UserOpinions.Include(i => i.UserOpinionDetails).Include(i => i.SenderUser).Where(w => w.ArticleId == articleId
           // && w.UserOpinionType == UserOpinionType.Articles
           // && w.ShowStatus == ShowStatus.AllowVisit
           )
                .Select(s => new UserOpinionDto
                {
                    Remark = s.Remark,
                    ArticleId = s.ArticleId,
                    SenderUser = s.SenderUser.FullName != null ? s.SenderUser.FullName : "کاربر تامین پلاس",
                    CreateDate = s.CreateDate,
                   // NegativeOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.NegativeOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                   // PositiveOpinions = s.UserOpinionDetails.Where(w => w.UseOpinionStatus == UseOpinionStatus.PositiveOpinion).Select(s => new UserOpinionModel { Text = s.Remark }).ToList(),
                }).ToListAsync();

            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(UserInputOpinionModel model)
        {
            var result = new ShopActionResult<int>();


            var userOpinion = await context.UserOpinions.FirstOrDefaultAsync(f => f.Id == model.Id);
          //  userOpinion.ShowStatus = model.ShowStatus;

            await context.SaveChangesAsync();
          //  result.Message = MessagesFA.UpdateSuccessful;

            result.IsSuccess = true;
            return result;
        }
    }
}
