using Application.Interfaces.Catalog;
using Infrastructure.Common;
using Infrastructure.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Catalog.Product
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }


        /// <summary>
        ///  لیست  جستجوی کاربران برای محصولات 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListUserLogSearchForProduct")]
        public async Task<IActionResult> GetListUserLogSearchForProduct(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await productService.GetListUserLogSearchForProduct(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        /// <summary>
        ///  لیست کالا ها 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(GridQueryModel model = null)
        {
           // if (UserType == UserType)
           // {
                var result = await productService.GetList(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        ///  لیست کالا ها به صورت خروجی اکسل 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListForExcel")]
        public async Task<IActionResult> GetListForExcel()
        {
           // if (UserType == UserType)
           // {
                var fileName = ExcelUtility.GenerateExcelFileName("Product");
                var exportbytes = await productService.GetListForExcel(null, fileName);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
           // }
           // else { return BadRequest(); }

        }

        /// <summary>
        /// آپلود فایل قیمت محصولات
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("ImportPriceListWithExcel")]
        public async Task<IActionResult> ImportPriceListWithExcel(IFormFile file)
        {
            var result = await productService.ImportPriceListWithExcel(file, Guid.NewGuid());
            return Ok(result);
        }



        /// <summary>
        ///  لیست تاریخچه جستجو ... ها به صورت خروجی اکسل 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListUserLogSearchForExcel")]
        public async Task<IActionResult> GetListUserLogSearchForExcel()
        {
          //  if (UserType == UserType)
          //  {
                var fileName = ExcelUtility.GenerateExcelFileName("userLogSearch");
                var exportbytes = await productService.GetListUserLogSearchForExcel(null, fileName);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
          //  }
          //  else { return BadRequest(); }

        }




        /// <summary>
        ///  لیست کالاها براساس کد گروه
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListWithCategory/{categoryId}")]
        public async Task<IActionResult> GetListWithCategory(int categoryId)
        {
            var result = await productService.GetListWithCategory(categoryId);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await productService.GetById(id);
            return Ok(result);
        }


        /// <summary>
        ///   برگرداندن مقالات مشابه یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetArticleProductById/{id}")]
        public async Task<IActionResult> GetArticleProductById(int id)
        {
            var result = await productService.GetArticleProductById(id);
            return Ok(result);
        }


        /// <summary>
        ///     ثبت مقالات مشابه برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddArticleProduct")]
        public async Task<IActionResult> AddArticleProduct(ArticleProductInputDto model)
        {
            var result = await productService.AddArticleProduct(model);
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن ویدیوها مشابه یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVideoProductById/{id}")]
        public async Task<IActionResult> GetVideoProductById(int id)
        {
            var result = await productService.GetVideoProductById(id);
            return Ok(result);
        }


        /// <summary>
        ///     ثبت ویدیوها مشابه برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddVideoProduct")]
        public async Task<IActionResult> AddVideoProduct(VideoProductInputDto model)
        {
            var result = await productService.AddVideoProduct(model);
            return Ok(result);
        }




        /// <summary>
        ///   برگرداندن کالاهای مشابه یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSimilarProductById/{id}")]
        public async Task<IActionResult> GetSimilarProductById(int id)
        {
            var result = await productService.GetSimilarProductById(id);
            return Ok(result);
        }


        /// <summary>
        ///     ثبت کالاهای مشابه برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddSimilarProduct")]
        public async Task<IActionResult> AddSimilarProduct(SimilarProductInputDto model)
        {
            var result = await productService.AddSimilarProduct(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن وضعیت مالی یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFinancialProductList/{id}")]
        public async Task<IActionResult> GetFinancialProductList(int id)
        {

            var result = await productService.GetFinancialProductList(id);
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن وضعیت مالی یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFinancialProductById/{id}")]
        public async Task<IActionResult> GetFinancialProductById(int id)
        {
            var result = await productService.GetFinancialProductById(id);
            return Ok(result);
        }


        /// <summary>
        ///     ویرایش وضعیت مالی برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateFinancialProduct")]
        public async Task<IActionResult> UpdateFinancialProduct(FinancialProductDto model)
        {
            var result = await productService.UpdateFinancialProduct(model);
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن وضعیت ارسال    
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeliveryProductById/{id}")]
        public async Task<IActionResult> GetDeliveryProductById(int id)
        {
            var result = await productService.GetDeliveryProductById(id);
            return Ok(result);
        }



        /// <summary>
        ///     ویرایش وضعیت ارسال برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateDeliveryProduct")]
        public async Task<IActionResult> UpdateDeliveryProduct(DeliveryProductDto model)
        {
            var result = await productService.UpdateDeliveryProduct(model);
            return Ok(result);
        }



        /// <summary>
        ///     ویرایش وضعیت ارسال برای یک کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddDeliveryProduct")]
        public async Task<IActionResult> AddDeliveryProduct(DeliveryProductDto model)
        {
            var result = await productService.AddDeliveryProduct(model);
            return Ok(result);
        }

        /// <summary>
        ///   برگرداندن وضعیت ارسال یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeliveryProduct")]
        public async Task<IActionResult> GetDeliveryProduct(DeliveryProductFilterDto model = null)
        {
            var result = await productService.GetDeliveryProduct(model);
            return Ok(result);
        }


        /// <summary>
        ///   حذف وضعیت ارسال برای یک کالا 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("DeleteDeliveryProduct/{id}")]
        public async Task<IActionResult> DeleteDeliveryProduct(int id)
        {
            var result = await productService.DeleteDeliveryProduct(id);
            return Ok(result);
        }



        /// <summary>
        ///   برگرداندن فایل های یک کالا 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductAttachment/{id}")]
        public async Task<IActionResult> GetProductAttachment(int id)
        {
            var result = await productService.GetProductAttachment(id);
            return Ok(result);
        }



        /// <summary>
        ///     ثبت فایل برای  کالا  
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateProductAttachment")]
        public async Task<IActionResult> UpdateProductAttachment([FromForm, FromBody] ProductAttachmentInputDto model)
        {
            var result = await productService.UpdateProductAttachment(model);
            return Ok(result);
        }


        /// <summary>
        ///     ثبت کالا جدید 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm, FromBody] ProductDto model)
        {
          //  if (UserType == UserType)
          //  {
                model.UserId = UserId;
                var result = await productService.Add(model);
                return Ok(result);
          //  }
          //  else { return BadRequest(); }

        }



        /// <summary>
        ///   ویرایش کالا 
        /// </summary>
        /// <returns></returns>        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm, FromBody] ProductDto model)
        {

           // if (UserType == UserType)
           // {
                model.UserId = UserId;
                var result = await productService.Update(model);
                return Ok(result);
           // }
           // else { return BadRequest(); }

        }


        ///// <summary>
        /////   حذف کالا 
        ///// </summary>
        ///// <returns></returns> 
        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await productService.Delete(id);
        //    return Ok(result);
        //}

        /// <summary>
        ///   حذف تصویر کالا 
        /// </summary>
        /// <returns></returns> 
        [HttpDelete("DeleteAttachment/{id}")]
        public async Task<IActionResult> DeleteAttachment(Guid id)
        {
            var result = await productService.DeleteAttachment(id);
            return Ok(result);
        }

        /// <summary>
        ///     اضافه کردن به لیست علاقه مندی ها  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddFavoriteProduct")]
        public async Task<IActionResult> AddFavoriteProduct(FavoriteProductModel model)
        {
            var result = await productService.AddFavoriteProduct(model, UserId);
            return Ok(result);
        }


        /// <summary>
        ///      بر گرداندن لیست علاقه مندی ها  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllFavoriteProduct")]
        public async Task<IActionResult> GetAllFavoriteProduct()
        {
            var result = await productService.GetAllFavoriteProduct(UserId);
            return Ok(result);
        }


        /// <summary>
        ///      بر گرداندن لیست علاقه مندی ها  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByIdFavoriteProduct/{productId}")]
        public async Task<IActionResult> GetByIdFavoriteProduct(int productId)
        {
            var result = await productService.GetByIdFavoriteProduct(UserId, productId);
            return Ok(result);
        }

    }
}
