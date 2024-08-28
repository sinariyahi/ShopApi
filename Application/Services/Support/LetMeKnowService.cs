using Application.Interfaces.Support;
using Application.Interfaces;
using Domain.Entities.Support;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Support
{
    public class LetMeKnowService : ILetMeKnowService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<UserLetMeKnow> _queryService;

        public LetMeKnowService(BIContext context, IGenericQueryService<UserLetMeKnow> queryService)
        {
            this.context = context;
            _queryService = queryService;
        }

        public async Task<byte[]> GetListForExcel(GridQueryModel model = null, string fileName = null)
        {

            var queryResult = await _queryService.QueryAsync(model, includes: new string[] { "User", "Product.Category" }, exportToExcel: true);

            var data = queryResult.Data.Select(q => new UserRegisterLetMeKnowDtoDto
            {
                Id = q.Id,
                CreateDate = DateUtility.CovertToShamsi(q.CreateDate),
                Mobile = q.Mobile,
                ProductName = q.Product.ProductName,
                FullName = q.User != null ? q.User.FirstName + " " + q.User.LastName : "",
                CategoryName = q.Product.Category.CategoryName

            }).ToList();

            var exportData = ExcelUtility.ExportToExcel<UserRegisterLetMeKnowDtoDto>(data, fileName);
            return exportData;
        }

        public async Task<ShopActionResult<int>> Register(UserLetMeKnowDto model, Guid? userId)
        {
            var result = new ShopActionResult<int>();
            var mobileValue = "";
            if (userId != null)
            {
                mobileValue = context.Users.FirstOrDefault(f => f.Id == userId)?.PhoneNumber;
            }

            if (userId != null && await context.UserLetMeKnows.AnyAsync(a => a.UserId == userId && a.ProductId == model.ProductId))
            {
                result.IsSuccess = false;
                result.Message = "قبلا شما برای این محصول درخواست اطلاع رسانی کرده اید ";
                return result;

            }
            if (userId == null && await context.UserLetMeKnows.AnyAsync(a => a.Mobile == model.Mobile && a.ProductId == model.ProductId))
            {
                result.IsSuccess = false;
                result.Message = "قبلا شما برای این محصول درخواست اطلاع رسانی کرده اید ";
                return result;

            }

            var data = new UserLetMeKnow
            {
                Mobile = userId == null ? model.Mobile : mobileValue,
                UserId = userId,
                CreateDate = DateTime.Now,
                ProductId = model.ProductId,
            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.SuccessTheOperation;
            return result;
        }






        public async Task<ShopActionResult<List<UserRegisterLetMeKnowDtoDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserRegisterLetMeKnowDtoDto>>();

            var queryResult = await _queryService.QueryAsync(model, includes: new string[] { "User", "Product.Category" });

            result.Data = queryResult.Data.Select(q => new UserRegisterLetMeKnowDtoDto
            {
                Id = q.Id,
                CreateDate = DateUtility.CovertToShamsi(q.CreateDate),
                Mobile = q.Mobile,
                ProductName = q.Product.ProductName,
                FullName = q.User != null ? q.User.FirstName + " " + q.User.LastName : "",
                CategoryName = q.Product.Category.CategoryName

            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }



    }
}
