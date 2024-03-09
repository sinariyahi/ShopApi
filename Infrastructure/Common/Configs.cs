using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Configs
    {
        public string GoldiranAPI { get; set; }
        public string GoldiranAPIToken { get; set; }
        public string MediaServer { get; set; }
        public string EditorMediaFolder { get; set; }
        public string DBConnection { get; set; }
        public string AccessDBConnection { get; set; }
        public string CubeDBConnection { get; set; }
        public int FromReportYear { get; set; }
        public int ToReportYear { get; set; }
        public int DefaultMonth { get; set; }
        public string FilePath { get; set; }
        public int RefreshTokenTimeout { get; set; }
        public int TokenTimeout { get; set; }
        public string TokenKey { get; set; }
        public string EncryptionKey { get; set; }
        public int PageItemCount { get; set; }
        public string ConfirmationEmail { get; set; }
        public string ConfirmationEmailPassword { get; set; }
        public string MailServer { get; set; }
        public string WebSiteAddress { get; set; }
        public int Port { get; set; }
        public string SystemEmailSenderRole { get; set; }
        public string SMSUserName { get; set; }
        public string SMSPassword { get; set; }
        public string SMSDomain { get; set; }
        public string SMSNumber { get; set; }
    }
}
