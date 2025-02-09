using Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IConnectionUtility, ConnectionUtility>();
            services.AddScoped<ICustomEncryption, CustomEncryption>();
            services.AddSingleton<MailUtility>();
            services.AddSingleton<SMSUtility>();

        //    var sp = services.BuildServiceProvider();
        //    Configs configs = sp.GetService<IOptions<Configs>>().Value;

        //    services.AddMailKit(optionBuilder =>
        //    {
        //        optionBuilder.UseMailKit(new MailKitOptions()
        //        {
        //            //get options from sercets.json
        //            Server = configs.MailServer,
        //            Port = configs.Port,
        //            SenderName = configs.ConfirmationEmail,
        //            SenderEmail = configs.ConfirmationEmail,
        //            // can be optional with no authentication 
        //            Account = configs.ConfirmationEmail,
        //            //Password = configs.ConfirmationEmailPassword,
        //            // enable ssl or tls
        //            Security = false,
        //        });
        //    });

            return services;
        }
    }
}
