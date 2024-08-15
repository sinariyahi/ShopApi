using Application.Interfaces.Security;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Security
{
    public class RoleService : IRoleService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Role> roleQueryService;
        public RoleService(BIContext context, IGenericQueryService<Role> roleQueryService)
        {
            this.context = context;
            this.roleQueryService = roleQueryService;
        }

        public async Task<ShopActionResult<int>> Add(RoleDto model)
        {
            var result = new ShopActionResult<int>();

            var role = new Role
            {
                RoleName = model.RoleName,
                ParentId = model.ParentId,
                Code = model.Code,
                IsActive = model.IsActive,
                OrganizationLevel = model.OrganizationLevel,
                Title = model.Title,
            };
            if (!await context.Roles.AnyAsync(a => a.Code == role.Code))
            {
                await context.AddAsync(role);
                await context.SaveChangesAsync();

                result.IsSuccess = true;
               // result.Message = MessagesFA.SaveSuccessful;
                return result;
            }
            else
            {
                result.IsSuccess = false;
               // result.Message = MessagesFA.CodeExists;
                return result;
            }

        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            if (await context.Roles.AnyAsync(q => q.ParentId == id))
            {
                result.IsSuccess = false;
             //   result.Message = MessagesFA.RoleHasChildren;
                return result;
            }

            var role = new Role { Id = id };
            context.Remove(role);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
           // result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<RoleDto>> GetById(int id)
        {
            var result = new ShopActionResult<RoleDto>();

            var role = await context.Roles.FindAsync(id);
            var model = new RoleDto
            {
                Id = role.Id,
                IsActive = role.IsActive,
                RoleName = role.RoleName,
                Code = role.Code,
                ParentId = role.ParentId,
                OrganizationLevel = role.OrganizationLevel,
                Title = role.Title,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        /// <summary>
        /// لیست نقش ها  
        /// </summary>
        /// <returns></returns>
        public async Task<ShopActionResult<List<RoleDto>>> GetRoles(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<RoleDto>>();
            var queryResult = await roleQueryService.QueryAsync(model, includes: new[] { "Parent" });

            var list = new List<RoleDto>();
            foreach (var item in queryResult.Data)
            {
                var role = new RoleDto
                {
                    Id = item.Id,
                    RoleName = item.RoleName,
                    IsActive = item.IsActive,
                    Code = item.Code,
                    ParentId = item.ParentId,
                    ParentTitle = item.ParentId != null ? item.Parent.RoleName : string.Empty,
                    OrganizationLevel = item.OrganizationLevel,
                    Title = item.Title,
                };

                list.Add(role);
            }

            result.Data = list;
            result.IsSuccess = true;
            return result;

        }

        public async Task<ShopActionResult<int>> Update(RoleDto model)
        {
            var result = new ShopActionResult<int>();

            var role = await context.Roles.FindAsync(model.Id);

            if (role.Code == model.Code)
            {
                role.RoleName = model.RoleName;
                role.IsActive = model.IsActive;
                role.Code = model.Code;
                role.ParentId = model.ParentId;
                role.OrganizationLevel = model.OrganizationLevel;
                role.Title = model.Title;

                await context.SaveChangesAsync();

                result.IsSuccess = true;
                //result.Message = MessagesFA.UpdateSuccessful;
                return result;
            }
            else
            {
                if (!await context.Roles.AnyAsync(a => a.Code == model.Code))
                {

                    role.RoleName = model.RoleName;
                    role.IsActive = model.IsActive;
                    role.Code = model.Code;
                    role.ParentId = model.ParentId;
                    role.OrganizationLevel = model.OrganizationLevel;
                    role.Title = model.Title;

                    await context.SaveChangesAsync();

                    result.IsSuccess = true;
                    //result.Message = MessagesFA.UpdateSuccessful;
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    //result.Message = MessagesFA.CodeExists;
                    return result;
                }
            }


        }

        public async Task<ShopActionResult<List<RoleGroupPermissionDto>>> GetRolePermissions(int roleId)
        {
            var result = new ShopActionResult<List<RoleGroupPermissionDto>>();
            var finalData = new List<RoleGroupPermissionDto>();

            var permissionGroups = await context.PermissionGroups.Where(q => q.IsActive == true).ToListAsync();
            var rolePermissions = await context.RolePermissions.Where(q => q.RoleId == roleId).ToListAsync();
            var allPermissions = await context.Permissions.Where(q => q.IsActive == true).ToListAsync();

            foreach (var group in permissionGroups)
            {
                var groupModel = new RoleGroupPermissionDto
                {
                    Value = group.Id,
                    Label = group.Title,
                    EnTitle = group.EnTitle,
                };
                groupModel.Children = allPermissions.Where(q => q.PermissionGroupId == group.Id)
                    .Select(q => new RolePermissionDto
                    {
                        Value = q.Id,
                        Label = q.Title,
                        EnTitle = q.EnTitle,
                        Checked = rolePermissions.Any(p => p.PermissionId == q.Id),
                    }).ToList();

                finalData.Add(groupModel);
            }

            finalData = finalData.OrderByDescending(q => q.Children.Count).ToList();

            finalData.ForEach(q => q.Checked = q.Children.Any() && q.Children.Count == q.Children.Where(c => c.Checked).Count());

            finalData = finalData.Where(q => q.Children != null && q.Children.Count > 0).ToList();

            result.IsSuccess = true;
            result.Data = finalData;
            return result;
        }
        public async Task<ShopActionResult<int>> UpdateRolePermissions(int roleId, List<RolePermissionDto> permissions)
        {
            var result = new ShopActionResult<int>();

            var rolePermissions = await context.RolePermissions.Where(q => q.RoleId == roleId).ToListAsync();

            //remove all specifig role permissions
            context.RolePermissions.RemoveRange(rolePermissions);

            var newRolePermissions = new List<RolePermission>();
            foreach (var permission in permissions)
            {
                var newRolePermission = new RolePermission
                {
                    PermissionId = permission.Value,
                    RoleId = roleId,
                };
                newRolePermissions.Add(newRolePermission);
            }

            await context.RolePermissions.AddRangeAsync(newRolePermissions);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }

        public async Task<ShopActionResult<List<TreeDto>>> GetRoleTree(int? parentId)
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var roles = await context.Roles.Where(q => parentId == null || (q.ParentId == parentId || q.Id == parentId)).ToListAsync();
            var rootElement = roles.FirstOrDefault(q => q.ParentId == null);

            var model = new TreeDto
            {
                Title = rootElement.RoleName,
                Key = rootElement.Id,
                Text = rootElement.RoleName,
                Value = rootElement.Id,
                Children = GenerateRecuresive(rootElement, roles),
            };

            result.IsSuccess = true;
            result.Data = new List<TreeDto>() { model };
            return result;
        }

        private List<TreeDto> GenerateRecuresive(Role parentRole, List<Role> roles, int level = 2)
        {
            var childRoles = roles.Where(q => q.ParentId == parentRole.Id);
            var children = new List<TreeDto>();
            foreach (var item in childRoles)
            {
                var child = new TreeDto
                {
                    Key = item.Id,
                    Title = item.RoleName,
                    Value = item.Id,
                    Text = item.RoleName,
                    Children = GenerateRecuresive(item, roles, 2),
                    Rate = level == 1 ? 30 : 20,
                };

                children.Add(child);
            }

            return children;
        }

        public async Task<ShopActionResult<List<TreeDto>>> GetOragnizationChartData(int? parentId = null)
        {
            var result = new ShopActionResult<List<TreeDto>>();
            var roles = await context.Roles.Where(q => parentId == null || (q.ParentId == parentId || q.Id == parentId)).ToListAsync();
            var rootElement = roles.FirstOrDefault(q => q.ParentId == null);

            var model = new TreeDto
            {
                Title = rootElement.RoleName,
                Key = rootElement.Id,
                Text = rootElement.RoleName,
                Value = rootElement.Id,
                Children = GenerateRecuresive(rootElement, roles, 1),
                Rate = 50,
            };


            var projects = await context.Projects.Where(q => q.IsActive).ToListAsync();
            foreach (var item in model.Children)
            {
                var projectChilds = projects.Where(q => q.OrganizationUnitId == item.Key).
                    Select(q => new TreeDto
                    {
                        Key = q.Id,
                        Title = $"پروژه {q.Title}",
                        Value = q.Id,
                        Text = $"پروژه {q.Title}",
                        Rate = 20,
                    }).ToList();
                item.Children.AddRange(projectChilds);
            }

            result.IsSuccess = true;
            result.Data = new List<TreeDto> { model };
            return result;
        }
    }
}
