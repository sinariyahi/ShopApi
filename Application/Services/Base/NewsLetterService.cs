using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain.Entities.Support;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class NewsLetterService : INewsLetterService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<NewsLetter> _queryService;
        private readonly IGenericQueryService<UserNewsLetter> _userNewsqueryService;

        public NewsLetterService(BIContext context, IGenericQueryService<NewsLetter> queryService,
               IGenericQueryService<UserNewsLetter> userNewsqueryService)
        {
            this.context = context;
            _queryService = queryService;
            _userNewsqueryService = userNewsqueryService;

        }

        public async Task<ShopActionResult<int>> Add(NewsLetterDto model)
        {
            var result = new ShopActionResult<int>();
            var data = new NewsLetter
            {
                IsActive = model.IsActive,
                Title = model.Title,
                Remark = model.Remark,
                Template = model.Template,
                CreateDate = DateTime.Now
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }


        public async Task<ShopActionResult<int>> Register(UserNewsLetterDto model, Guid? userId)
        {
            var result = new ShopActionResult<int>();
            var data = new UserNewsLetter
            {
                Email = model.Email,
                UserId = userId,
                CreateDate = DateTime.Now
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.SuccessTheOperation;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var item = new NewsLetter { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<NewsLetterDto>> GetById(int id)
        {
            var result = new ShopActionResult<NewsLetterDto>();

            var data = await context.NewsLetters.FindAsync(id);
            var model = new NewsLetterDto
            {
                Id = data.Id,
                IsActive = data.IsActive,
                Title = data.Title,
                Remark = data.Remark,
                Template = data.Template,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<NewsLetterDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<NewsLetterDto>>();

            var queryResult = await _queryService.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new NewsLetterDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "فعال" : "غیرفعال"
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }



        public async Task<ShopActionResult<List<UserRegisterNewsLetterDto>>> GetUserRegisterNewsLetter(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserRegisterNewsLetterDto>>();

            var queryResult = await _userNewsqueryService.QueryAsync(model, includes: new string[] { "User" });

            result.Data = queryResult.Data.Select(q => new UserRegisterNewsLetterDto
            {
                Id = q.Id,
                CreateDate = DateUtility.CovertToShamsi(q.CreateDate),
                Email = q.Email,
                FullName = q.User != null ? q.User.FirstName + " " + q.User.LastName : ""

            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }


        public async Task<ShopActionResult<int>> Update(NewsLetterDto model)
        {
            var result = new ShopActionResult<int>();

            var data = await context.NewsLetters.FindAsync(model.Id);
            data.Title = model.Title;
            data.IsActive = model.IsActive;
            data.Remark = model.Remark;
            data.Template = model.Template;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
