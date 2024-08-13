using Application.Interfaces.Security;
using Domain.Entities.Security;
using Domain;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Common;

namespace Application.Services.Security
{
    public class MenuService : IMenuService
    {
        private readonly ILogger logger;
        private readonly BIContext context;

        public MenuService(BIContext context, ILogger<MenuService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        private List<MenuDto> GetMenuChildren(MenuDto model, List<MenuDto> items)
        {
            var childrenMenu = new List<MenuDto>();

            //if (menu.Count() == 0) return childrenMenu;

            foreach (var item in items)
            {
                if (model.Id == item.ParentId)
                {
                    var obj = new MenuDto();
                    obj.Name = item.Name;
                    obj.Id = item.Id;
                    obj.ParentId = item.ParentId;
                    obj._tag = item._tag;
                    obj.Icon = item.Icon;
                    obj.To = item.To;
                    childrenMenu.Add(obj);
                    //  GetMenuChildren(item, items);
                }

            }
            return childrenMenu;

        }

        public async Task<ShopActionResult<List<MenuDto>>> GetMenu()
        {

            var result = new ShopActionResult<List<MenuDto>>();
            try
            {
                var menulist = new List<MenuDto>();
                var Fullmenu = new List<MenuDto>();

                var menu = await context.Menus.OrderBy(q => q.SortOrder).Where(q => q.IsActive == true).ToListAsync();
                #region SubMenu
                foreach (var data in menu)
                {
                    var subMenu = new MenuDto
                    {
                        Id = data.Id,
                        Icon = data.Icon,
                        Name = data.Title,
                        ParentId = data.ParentId,
                        To = data.Path,
                        _tag = data.Tag,

                    };

                    if (data.ParentId == null)
                    {
                        menulist.Add(subMenu);
                    }

                }
                #endregion

                #region MenuAll
                foreach (var data in menu)
                {
                    var prop = new MenuDto
                    {
                        Id = data.Id,
                        Icon = data.Icon,
                        Name = data.Title,
                        ParentId = data.ParentId,
                        To = data.Path,
                        _tag = data.Tag,

                    };

                    Fullmenu.Add(prop);
                }
                #endregion

                #region MenuWithChildren
                foreach (var item in menulist)
                {
                    var model = new MenuDto();

                    model.Id = item.Id;
                    model.ParentId = item.ParentId;
                    model.To = item.To;
                    model._tag = item._tag;
                    model.Icon = item.Icon;
                    model.Name = item.Name;

                    List<MenuDto> child = GetMenuChildren(item, Fullmenu);
                    if (child.Count() > 0)
                    {
                        item._children = child;

                    }
                }
                result.Data = menulist.ToList();
                #endregion

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.CommunicationError;
                return result;
            }
        }

        public async Task<ShopActionResult<List<MenuDto>>> GetMenuAccessForUser(Guid UserId)
        {
            var result = new ShopActionResult<List<MenuDto>>();
            try
            {
                var menulist = new List<MenuDto>();

                #region MenuAccess
                var userRoles = await context.UserRoles.Where(u => u.UserId == UserId).AsNoTracking().Select(q => q.RoleId).ToListAsync();
                var menus = await context.MenuRoles.Where(q => userRoles.Contains(q.RoleId)).AsNoTracking().Select(q => q.Menu).OrderBy(q => q.SortOrder).ToListAsync();
                var topMenus = menus.Where(q => q.ParentId == null);

                foreach (var item in topMenus)
                {
                    var model = new MenuDto();
                    model.Id = item.Id;
                    model.ParentId = item.ParentId;
                    model.To = item.Path;
                    model._tag = item.Tag;
                    model.Icon = item.Icon;
                    model.Name = item.Title;
                    model._children = GenerateChildsRecursive(menus, model.Id);

                    menulist.Add(model);
                }

                result.Data = menulist;
                #endregion

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result.IsSuccess = false;
                //result.Message = MessagesFA.CommunicationError;
                return result;
            }

        }

        private List<MenuDto>? GenerateChildsRecursive(List<Menu> menus, int parentId)
        {
            if (!menus.Any(q => q.ParentId == parentId)) return null;
            var childs = new List<MenuDto>();
            var subItems = menus.Where(q => q.ParentId == parentId);
            foreach (var item in subItems)
            {
                var model = new MenuDto();
                model.Id = item.Id;
                model.ParentId = item.ParentId;
                model.To = item.Path;
                model._tag = item.Tag;
                model.Icon = item.Icon;
                model.Name = item.Title;
                childs.Add(model);
            }

            return childs;
        }
    }
}
