using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.Support
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        readonly IMemoryCache cache;
        public CaptchaController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [HttpGet("{captchaKey}")]
        public async Task<FileResult> GetCaptchaImage(string captchaKey)
        {
            var text = await CustomCaptcha(captchaKey);
            if (text == null) return null;


            RandomImage ci = new RandomImage(text, 100, 40);
            var ms = new MemoryStream();
            ci.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            ci.Image.Dispose();
            return File(ms.ToArray(), "image/jpeg");
        }
        async Task<string> CustomCaptcha(string captchaName)
        {
            var generateRandomCode = GenerateRandomCode();

            cache.Remove(captchaName);
            var code = await cache.GetOrCreateAsync(captchaName, entry => { entry.SetAbsoluteExpiration(DateTime.Now.AddMinutes(30)); return generateRandomCode; });
            return code;
        }
        Task<string> GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 4; j++)
            {
                int ch = r.Next(0, 9);
                s = s + ch.ToString();
                r.NextDouble();
                r.Next(100, 1999);
            }
            return Task.FromResult(s);
        }
    }
}
