using Application.Interfaces.Security;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Security
{
    public class UserGroupService : IUserGroupService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<UserGroup> service;
        public UserGroupService(BIContext context,
            IGenericQueryService<UserGroup> service
            )
        {
            this.context = context;
            this.service = service;
        }

        public async Task<ShopActionResult<Guid>> Add(UserGroupDto model)
        {
            var result = new ShopActionResult<Guid>();
            var data = new UserGroup
            {
                IsActive = model.IsActive,
                CreateDate = DateTime.Now,
                Title = model.Title,

            };

            await context.AddAsync(data);
            await context.SaveChangesAsync();

            foreach (var item in model.Actions)
            {
                var obj = new UserGroupAction
                {
                    UserGroupId = data.Id,
                    CompanyRequestWorkFlowStatus = item
                };
                await context.UserGroupActions.AddAsync(obj);
            }

            foreach (var item in model.Users)
            {
                var obj = new UserGroupMember
                {
                    UserGroupId = data.Id,
                    UserId = item
                };
                await context.UserGroupMembers.AddAsync(obj);
            }

            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<Guid>> Delete(Guid id)
        {
            var result = new ShopActionResult<Guid>();

            var item = new UserGroup { Id = id };
            context.Remove(item);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<UserGroupDto>> GetById(Guid id)
        {
            var result = new ShopActionResult<UserGroupDto>();

            var data = await context.UserGroups.FindAsync(id);
            var model = new UserGroupDto
            {
                Id = id,
                IsActive = data.IsActive,
                Title = data.Title,
                Users = await context.UserGroupMembers.Where(w => w.UserGroupId == id).Select(s => s.UserId).ToListAsync(),
                Actions = await context.UserGroupActions.Where(w => w.UserGroupId == id).Select(s => s.CompanyRequestWorkFlowStatus).ToListAsync(),

            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<UserGroupDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<UserGroupDto>>();

            var queryResult = await service.QueryAsync(model);

            result.Data = queryResult.Data.Select(q => new UserGroupDto
            {
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                IsActiveTitle = q.IsActive == true ? "Active" : "InActive"

            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<UserAccessForUserGroupDto>> GetUserActionsAccessForUserGroup(Guid userId)
        {
            var result = new ShopActionResult<UserAccessForUserGroupDto>();

            var queryResult = await context.UserGroupMembers.Include(q => q.UserGroup).Where(w => w.UserId == userId).ToListAsync();

            var data = new UserAccessForUserGroupDto();

            data.UserId = userId;
            //data.UserGroupId = queryResult.FirstOrDefault().UserGroupId;
            data.Actions = new List<CompanyRequestWorkFlowStatus>();

            foreach (var item in queryResult)
            {
                var actionsList = context.UserGroupActions.Where(w => w.UserGroupId == item.UserGroupId).Select(s => s.CompanyRequestWorkFlowStatus).ToList();
                foreach (var actionItem in actionsList)
                {
                    data.Actions.Add(actionItem);

                }
            }
            data.Actions.Distinct();
            data.DisciplineIds = context.UserRoles.Where(w => w.UserId == userId).Select(s => s.RoleId).ToList();
            data.DisciplineNames = context.UserRoles.Include(i => i.Role).Where(w => w.UserId == userId).Select(s => s.Role.RoleName).ToList();

            //if (queryResult.FirstOrDefault() !=null)
            //{
            //    //data.Actions = context.UserGroupActions.Where(w => w.UserGroupId == data.UserGroupId).Select(s => s.CompanyRequestWorkFlowStatus).ToList();
            //    data.DisciplineIds = context.UserRoles.Where(w => w.UserId == userId).Select(s => s.RoleId).ToList();
            //}

            result.Data = data;
            result.IsSuccess = true;
            return result;

        }

        public async Task<ShopActionResult<Guid>> Update(UserGroupDto model)
        {
            var result = new ShopActionResult<Guid>();

            var data = await context.UserGroups.FindAsync(model.Id);

            #region UserGroupMembers

            foreach (var item in await context.UserGroupMembers.Where(w => w.UserGroupId == model.Id).ToListAsync())
            {
                context.Remove(item);
            }
            await context.SaveChangesAsync();

            foreach (var item in model.Users)
            {
                var obj = new UserGroupMember
                {
                    UserGroupId = model.Id,
                    UserId = item
                };
                await context.UserGroupMembers.AddAsync(obj);
            }

            #endregion


            #region UserGroupActions

            foreach (var item in await context.UserGroupActions.Where(w => w.UserGroupId == model.Id).ToListAsync())
            {
                context.Remove(item);
            }
            await context.SaveChangesAsync();

            foreach (var item in model.Actions)
            {
                var obj = new UserGroupAction
                {
                    UserGroupId = data.Id,
                    CompanyRequestWorkFlowStatus = item
                };
                await context.UserGroupActions.AddAsync(obj);
            }

            #endregion


            data.Title = model.Title;
            data.IsActive = model.IsActive;

            await context.SaveChangesAsync();

            result.IsSuccess = true;
          //  result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
