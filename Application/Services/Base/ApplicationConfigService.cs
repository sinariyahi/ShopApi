using Application.Interfaces.Base;
using Domain;
using Domain.Entities.Base;
using Infrastructure.Common;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public class ApplicationConfigService 
        //: IApplicationConfigService
    {
        private readonly BIContext context;
        public ApplicationConfigService(BIContext context)
        {
            this.context = context;
        }

        //public async Task<ShopActionResult<ApplicationConfigDto>> Get()
        //{
        //    var result = new ShopActionResult<ApplicationConfigDto>();

        //    var item = await context.ApplicationConfigs.FirstOrDefaultAsync();
        //    var model = new ApplicationConfigDto
        //    {
        //        ApplicationTitle = item.ApplicationTitle,
        //        ApplicationTitleEn = item.ApplicationTitleEn,
        //        Id = item.Id,
        //        FooterEn = item.FooterEn,
        //        Footer = item.Footer,
        //        MainLogoBase64 = FileUtility.ConvertToBase64(item.MainLogo),
        //        MainLogoBase64En = FileUtility.ConvertToBase64(item.MainLogoEn),
        //        ShowFooterEn = item.ShowFooterEn,
        //        ShowFooter = item.ShowFooter,
        //        SidebarLogoBase64 = FileUtility.ConvertToBase64(item.SidebarLogo),
        //        SidebarLogoBase64En = FileUtility.ConvertToBase64(item.SidebarLogoEn),
        //        CurrentYear = DateTime.Now.ToShamsiYear(),
        //    };

        //    result.IsSuccess = true;
        //    result.Data = model;
        //    return result;
        //}

        //public async Task<ShopActionResult<int>> Update(ApplicationConfigDto model)
        //{
        //    var result = new ShopActionResult<int>();
        //    if (await context.ApplicationConfigs.AnyAsync())
        //    {
        //        //Update
        //        var item = await context.ApplicationConfigs.FirstOrDefaultAsync();
        //        item.ApplicationTitle = model.ApplicationTitle.Replace("\"", "");
        //        item.ApplicationTitleEn = model.ApplicationTitleEn != null ? model.ApplicationTitleEn.Replace("\"", "") : null;
        //        item.Footer = model.Footer.Replace("\"", "");
        //        item.FooterEn = model.FooterEn != null ? model.FooterEn.Replace("\"", "") : null;
        //        item.ShowFooter = model.ShowFooter;
        //        item.ShowFooterEn = model.ShowFooterEn;

        //        if (model.MainLogo != null && model.MainLogo.Length > 0)
        //            item.MainLogo = FileUtility.ConvertToByteArray(model.MainLogo);

        //        if (model.MainLogoEn != null && model.MainLogoEn.Length > 0)
        //            item.MainLogoEn = FileUtility.ConvertToByteArray(model.MainLogoEn);

        //        if (model.SidebarLogo != null && model.SidebarLogo.Length > 0)
        //            item.SidebarLogo = FileUtility.ConvertToByteArray(model.SidebarLogo);

        //        if (model.SidebarLogoEn != null && model.SidebarLogoEn.Length > 0)
        //            item.SidebarLogoEn = FileUtility.ConvertToByteArray(model.SidebarLogoEn);

        //        await context.SaveChangesAsync();

        //    }
        //    else
        //    {
        //        //Insert
        //        var item = new ApplicationConfig
        //        {
        //            ApplicationTitle = model.ApplicationTitle,
        //            ApplicationTitleEn = model.ApplicationTitleEn,
        //            Footer = model.Footer,
        //            FooterEn = model.FooterEn,
        //            MainLogo = FileUtility.ConvertToByteArray(model.MainLogo),
        //            MainLogoEn = FileUtility.ConvertToByteArray(model.MainLogoEn),
        //            ShowFooter = model.ShowFooter,
        //            ShowFooterEn = model.ShowFooterEn,
        //            SidebarLogo = FileUtility.ConvertToByteArray(model.SidebarLogo),
        //            SidebarLogoEn = FileUtility.ConvertToByteArray(model.SidebarLogoEn),
        //        };
        //        await context.AddAsync(item);
        //        await context.SaveChangesAsync();
        //    }

           // result.IsSuccess = true;
           // result.Message = MessagesFA.UpdateSuccessful;
           // return result;
        //}
    }
}
