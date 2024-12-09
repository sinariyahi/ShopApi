using Application.Interfaces.Support;
using Infrastructure.Models.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Support
{
    public class UserOpinionController : BaseController
    {
        private readonly IUserOpinionService userOpinionService;

        public UserOpinionController(IUserOpinionService userOpinionService)
        {
            this.userOpinionService = userOpinionService;
        }

        /// <summary>
        ///   لیست  دیدگاه کاربران
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(FilterUserOpinionDto model = null)
        {
            var result = await userOpinionService.GetAll(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن  دیدگاه کاربران 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await userOpinionService.GetById(Guid.Parse(id));
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن  دیدگاه کاربران برای یک محصول 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetByProductId/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await userOpinionService.GetByProductId(productId);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن  دیدگاه کاربران برای یک مقاله 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetByArticleId/{articleId}")]
        public async Task<IActionResult> GetByArticleId(int articleId)
        {
            var result = await userOpinionService.GetByArticleId(articleId);
            return Ok(result);
        }


        /// <summary>
        ///     ثبت دیدگاه کاربران
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(UserOpinionDto model)
        {
            model.SenderUserId = UserId;
            var result = await userOpinionService.Add(model);
            return Ok(result);
        }



        /// <summary>
        ///   ویرایش دیدگاه کاربران 
        /// </summary>
        /// <returns></returns>        
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserInputOpinionModel model)
        {
            var result = await userOpinionService.Update(model);
            return Ok(result);
        }

    }
}
