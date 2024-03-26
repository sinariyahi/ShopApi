using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class SMSUtility
    {
        private readonly Configs configs;
        public SMSUtility(IOptions<Configs> options)
        {
            this.configs = options.Value;
        }
        public async Task<string> Send(string mobile, string message)
        {
            // Credentials
            string username = configs.SMSUserName;
            string password = configs.SMSPassword;
            string domain = configs.SMSDomain;
            string smsNumber = configs.SMSNumber;

            // Client
            using var client = new HttpClient();

            // Call
            var resultCode = await client.GetStringAsync("https://sms.magfa.com/api/http/sms/v1?service=enqueue&username=" +
                username + "&password=" + password + "&domain=" + domain + $"&from={smsNumber}&to={mobile}&text={message}");
            return resultCode;
        }
    }
}
