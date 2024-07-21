using Application.Interfaces.Base;
using Application.Interfaces;
using Domain.Entities.Base;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models.EIED;
using Infrastructure.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Base
{
    public class ProjectService : IProjectService
    {
        private readonly BIContext context;
        private readonly IGenericQueryService<Project> projectQueryService;
        public ProjectService(BIContext context,
            IGenericQueryService<Project> projectQueryService
            )
        {
            this.context = context;
            this.projectQueryService = projectQueryService;
        }

        public async Task<ShopActionResult<int>> Add(ProjectDto model)
        {
            var result = new ShopActionResult<int>();
            var project = new Project
            {
                CreateDate = DateTime.Now,
                UserId = model.UserId,
                Description = model.Description,
                IsActive = model.IsActive,
                Title = model.Title,
                Code = model.Code,
                ProjectTitle = model.ProjectTitle,
            };

            await context.AddAsync(project);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.SaveSuccessful;
            return result;
        }

        public async Task<ShopActionResult<int>> Delete(int id)
        {
            var result = new ShopActionResult<int>();

            var project = new Project { Id = id };
            context.Remove(project);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.DeleteSuccessful;
            return result;
        }

        public async Task<ShopActionResult<ProjectDto>> GetById(int id)
        {
            var result = new ShopActionResult<ProjectDto>();

            var project = await context.Projects.Include(q => q.OrganizationUnit).SingleOrDefaultAsync(q => q.Id == id);
            var model = new ProjectDto
            {
                Description = project.Description,
                Id = project.Id,
                IsActive = project.IsActive,
                Title = project.Title,
                UserId = project.UserId.Value,
                Code = project.Code,
                ProjectTitle = project.ProjectTitle,
            };

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<ShopActionResult<List<ProjectDto>>> GetList(GridQueryModel model = null)
        {
            var result = new ShopActionResult<List<ProjectDto>>();

            var queryResult = await projectQueryService.QueryAsync(model, includes: new string[] { "OrganizationUnit" });

            result.Data = queryResult.Data.Select(q => new ProjectDto
            {
                Description = q.Description,
                Id = q.Id,
                IsActive = q.IsActive,
                Title = q.Title,
                UserId = q.UserId.Value,
                Code = q.Code,
                ProjectTitle = q.ProjectTitle,
            }).ToList();
            result.IsSuccess = true;
            result.Total = queryResult.Total;
            result.Size = queryResult.Size;
            result.Page = queryResult.Page;
            return result;
        }

        public async Task<ShopActionResult<List<ComboItemDto>>> GetForCombo()
        {
            var result = new ShopActionResult<List<ComboItemDto>>();
            result.Data = await context.Projects.Where(q => q.IsActive)
                .Select(q => new ComboItemDto
                {
                    Value = q.Id,
                    Text = q.Title,
                })
                .ToListAsync();
            result.IsSuccess = true;
            return result;
        }

        public async Task<ShopActionResult<int>> Update(ProjectDto model)
        {
            var result = new ShopActionResult<int>();

            var project = await context.Projects.FindAsync(model.Id);
            project.Title = model.Title;
            project.IsActive = model.IsActive;
            project.Description = model.Description;
            project.Code = model.Code;
            project.ProjectTitle = model.ProjectTitle;
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            //result.Message = MessagesFA.UpdateSuccessful;
            return result;
        }
    }
}
