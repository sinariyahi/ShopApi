using Infrastructure.Models.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class MailUtility
    {
        private readonly Configs configs;
        private readonly ILogger logger;
        public MailUtility(IOptions<Configs> options, ILogger<MailUtility> logger)
        {
            this.configs = options.Value;
            this.logger = logger;
        }

        public async Task Send(MailMessageDto mail)
        {
            var message = new MailMessage();
            message.From = new MailAddress(mail.From);
            message.To.Add(new MailAddress(mail.To));
            message.Subject = mail.Subject;
            message.Body = mail.Body;
            message.IsBodyHtml = true;

            foreach (var cc in mail.CC)
            {
                message.CC.Add(cc);
            }

            try
            {
                using (var smtp = new SmtpClient(configs.MailServer, configs.Port))
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = false;
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Send Email with data :" + mail.ToString());
                logger.LogError(ex.ToString());
            }

        }
    }
}
