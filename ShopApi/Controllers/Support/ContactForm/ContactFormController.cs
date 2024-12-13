using Application.Interfaces.Support;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Support.ContactForm
{
        public class ContactFormController : BaseController
        {
            private readonly IContactFormService service;
            public ContactFormController(IContactFormService service)
            {
                this.service = service;
            }
            /// <summary>
            ///  لیست  همکاری با ما ها 
            /// </summary>
            /// <returns></returns>
            [HttpPost("GetList")]
            public async Task<IActionResult> GetList(GridQueryModel model = null)
            {
         //       if (UserType == UserType)
        //        {
                    var result = await service.GetList(model);
                    return Ok(result);
        //        }
        //        else { return BadRequest(); }

            }


            /// <summary>
            ///   برگرداندن جزییات همکاری با ما 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetById/{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await service.GetById(id);
                return Ok(result);
            }



        }
    }
}
