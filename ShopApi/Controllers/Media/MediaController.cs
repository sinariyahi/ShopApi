using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Media
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly IFileService fileService;

        public MediaController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var fileSaveUrl = await fileService.SaveFileInDisk(file);
            return Ok(fileSaveUrl);
        }
    }
}
