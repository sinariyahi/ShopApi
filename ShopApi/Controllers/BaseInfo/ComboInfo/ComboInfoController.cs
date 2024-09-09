using Application.Interfaces.Base;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ShopApi.Controllers.BaseInfo.ComboInfo
{
    public class ComboInfoController : BaseController
    {
        private readonly IComboInfoService _comboService;
        private IMemoryCache _cache;
        public ComboInfoController(IComboInfoService comboService, IMemoryCache cache)
        {
            _comboService = comboService;
            _cache = cache;
        }
        private string _cacheKey = "IndexTemplateCombo";

        /// <summary>
        ///   نوع sms ارسال شده
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSmsType")]
        public async Task<IActionResult> GetSmsType()
        {
            var result = await _comboService.GetSmsType();
            return Ok(result);
        }


        /// <summary>
        /// ویدیو ها  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVideos/{categoryId}")]
        public async Task<IActionResult> GetVideos(int categoryId)
        {
            var result = await _comboService.GetVideos(categoryId);
            return Ok(result);
        }

        /// <summary>
        ///  های شاخص Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetComboInfo()
        {
            var result = await _comboService.GetComboInfo(UserId);
            return Ok(result);
        }


        /// <summary>
        ///  PagesLinkType Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPagesLinkTypes")]
        public async Task<IActionResult> GetPagesLinkTypes()
        {
            var result = await _comboService.GetPagesLinkTypes();
            return Ok(result);
        }


        /// <summary>
        /// محصولات   Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProducts/{categoryId}")]
        public async Task<IActionResult> GetProducts(int categoryId)
        {
            var result = await _comboService.GetProducts(categoryId);
            return Ok(result);
        }


        /// <summary>
        /// GetCities   Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCities/{provinceId}")]
        public async Task<IActionResult> GetCities(Province provinceId)
        {
            var result = await _comboService.GetCities(provinceId);
            return Ok(result);
        }




        /// <summary>
        /// مقالات  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetArticles/{categoryId}")]
        public async Task<IActionResult> GetArticles(int categoryId)
        {
            var result = await _comboService.GetArticles(categoryId);
            return Ok(result);
        }



        /// <summary>
        ///   وضعیت فروش Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSaleStatus")]
        public async Task<IActionResult> GetSaleStatus()
        {
            var result = await _comboService.GetSaleStatus();
            return Ok(result);
        }



        /// <summary>
        ///   وضعیت سفارش Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrderStatus")]
        public async Task<IActionResult> GetOrderStatus()
        {
            var result = await _comboService.GetOrderStatus();
            return Ok(result);
        }



        /// <summary>
        ///   وضعیت نمایش Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShowStatus")]
        public async Task<IActionResult> GetShowStatus()
        {
            var result = await _comboService.GetShowStatus();
            return Ok(result);
        }



        /// <summary>
        ///   PositionPlace Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPositionPlace")]
        public async Task<IActionResult> GetPositionPlace()
        {
            var result = await _comboService.GetPositionPlace();
            return Ok(result);
        }



        /// <summary>
        ///   GetUserOpinionType Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserOpinionType")]
        public async Task<IActionResult> GetUserOpinionType()
        {
            var result = await _comboService.GetUserOpinionType();
            return Ok(result);
        }



        /// <summary>
        ///   GetUserTypes Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            var result = await _comboService.GetUserTypes();
            return Ok(result);
        }




        /// <summary>
        ///  روش ارسال Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeliveryType")]
        public async Task<IActionResult> GetDeliveryType()
        {
            var result = await _comboService.GetDeliveryType();
            return Ok(result);
        }


        /// <summary>
        ///   استان Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProvince")]
        public async Task<IActionResult> GetProvince()
        {
            var result = await _comboService.GetProvince();
            return Ok(result);
        }


        /// <summary>
        ///   نوع تعداد Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountType")]
        public async Task<IActionResult> GetCountType()
        {
            var result = await _comboService.GetCountType();
            return Ok(result);
        }



        /// <summary>
        /// بخش ها Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDisciplines")]
        public async Task<IActionResult> GetDisciplines()
        {
            var result = await _comboService.GetDisciplines();
            return Ok(result);
        }



        /// <summary>
        ///ها Symbol  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSymbols")]
        public async Task<IActionResult> GetSymbols()
        {
            var result = await _comboService.GetSymbolWithOutParent();
            return Ok(result);
        }



        /// <summary>
        /// VideoCategory  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVideoCategory")]
        public async Task<IActionResult> GetVideoCategory()
        {
            var result = await _comboService.GetVideoCategory();
            return Ok(result);
        }




        /// <summary>
        /// GetVideoSource  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVideoSource")]
        public async Task<IActionResult> GetVideoSource()
        {
            var result = await _comboService.GetVideoSource();
            return Ok(result);
        }




        /// <summary>
        ///ها Brand  Dropdown 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _comboService.GetBrands();
            return Ok(result);
        }




        /// <summary>
        ///  GetFeatureCategory
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFeatureCategory")]
        public async Task<IActionResult> GetFeatureCategory()
        {
            var result = await _comboService.GetFeatureCategory();
            return Ok(result);
        }



        /// <summary>
        ///  GetArticleCategory
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetArticleCategory")]
        public async Task<IActionResult> GetArticleCategory()
        {
            var result = await _comboService.GetArticleCategory();
            return Ok(result);
        }




        /// <summary>
        ///    get Disciplines in roles with parentId - بخش id  برگرداندن امور با  
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDisciplinesById/{id}")]
        public async Task<IActionResult> GetDisciplinesById(int id)
        {
            var result = await _comboService.GetDisciplinesById(id);
            return Ok(result);
        }







    }
}
