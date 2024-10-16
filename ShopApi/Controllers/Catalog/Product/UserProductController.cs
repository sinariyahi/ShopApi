using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Product
{
        [Route("api/[controller]")]
        [ApiController]
        public class UserProductController : ControllerBase
        {
            private readonly IProductService productService;
            public UserProductController(IProductService productService)
            {
                this.productService = productService;
            }


            /// <summary>
            ///   برگرداندن جدیدترین محصولات 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetTopNewProductList/{count}")]
            public async Task<IActionResult> GetTopNewProductList(int count)
            {
                var result = await productService.GetTopNewProductList(count);
                return Ok(result);
            }

            /// <summary>
            ///   برگرداندن محصولات پیشنهاد ویژه 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetSpecialOfferProductList/{count}")]
            public async Task<IActionResult> GetSpecialOfferProductList(int count)
            {
                var result = await productService.GetSpecialOfferProductList(count);
                return Ok(result);
            }


            /// <summary>
            ///   برگرداندن پرفروش ترین محصولات 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetTopSaleProductList/{count}")]
            public async Task<IActionResult> GetTopSaleProductList(int count)
            {
                var result = await productService.GetTopSaleProductList(count);
                return Ok(result);
            }


            /// <summary>
            ///   برگرداندن پر بازدید ترین محصولات 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetTopVisitedProductList/{count}")]
            public async Task<IActionResult> GetTopVisitedProductList(int count)
            {
                var result = await productService.GetTopVisitedProductList(count);
                return Ok(result);
            }



            /// <summary>
            ///   برگرداندن  محصولات مشابه 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetUserSimilarProductByTitle/{title}")]
            public async Task<IActionResult> GetUserSimilarProductByTitle(string title)
            {
                var result = await productService.GetUserSimilarProductByTitle(title);
                return Ok(result);
            }

            /// <summary>
            ///   برگرداندن  اطلاعات  مشابه 
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetSimilarData/{id}")]
            public async Task<IActionResult> GetSimilarData(int id)
            {
                var result = await productService.GetSimilarData(id);
                return Ok(result);
            }


            /// <summary>
            ///   برگرداندن  تعداد محصولات  فروش ویژه
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetSpecialOfferProductCount")]
            public async Task<IActionResult> GetSpecialOfferProductCount()
            {
                var result = await productService.GetSpecialOfferProductCount();
                return Ok(result);
            }



            /// <summary>
            ///   برگرداندن  محصولات برای مگا منو  
            /// </summary>
            /// <returns></returns>
            [HttpPost("GetProductsForParentItemsMegaMenu")]
            public async Task<IActionResult> GetProductsForParentItemsMegaMenu(GridQueryModel model)
            {
                var result = await productService.GetProductsForParentItemsMegaMenu(model);
                return Ok(result);
            }



            /// <summary>
            ///   برگرداندن  محصولات  
            /// </summary>
            /// <returns></returns>
            [HttpPost("GetUserList")]
            public async Task<IActionResult> GetUserList(GridQueryModel model)
            {
                var result = await productService.GetUserList(model);
                return Ok(result);
            }

            /// <summary>
            ///   برگرداندن جزئیات یک محصول  
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetProductDetail/{id}")]
            public async Task<IActionResult> GetProductDetail(int id)
            {
                var result = await productService.GetProductDetail(id);
                return Ok(result);
            }


            /// <summary>
            ///   برگرداندن جزئیات یک محصول  
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetProductDetailByTitle/{title}")]
            public async Task<IActionResult> GetProductDetail(string title)
            {
                var result = await productService.GetByTitle(title);
                return Ok(result);
            }




            /// <summary>
            ///   برگرداندن id محصولات  
            /// </summary>
            /// <returns></returns>
            [HttpGet("GetAllTitle")]
            public async Task<IActionResult> GetAllTitle()
            {
                var result = await productService.GetAllTitle();
                return Ok(result);
            }





        }
}
