using Application.Interfaces.Shop;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers.Shop
{
    public class ShopController : BaseController
    {
        private readonly IShopService shopService;
        public ShopController(IShopService shopService)
        {
            this.shopService = shopService;
        }

        /// <summary>
        ///  لیست استان - شهر - منطقه
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBasicData/{dataType}/{parentId}")]
        public async Task<IActionResult> GetBasicData(int dataType, int parentId)
        {
            var result = await shopService.GetBasicData(dataType, parentId);
            return Ok(result);
        }

        /// <summary>
        ///  اطلاعات قیمت و موجودی کالا
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPartInfo/{partNo}")]
        public async Task<IActionResult> GetPartInfo(string partNo)
        {
            var result = await shopService.GetPartBalanceInfo(partNo);
            return Ok(result);
        }

        /// <summary>
        ///  محله
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetParishList/{cityId}/{regionId}/{term?}")]
        public async Task<IActionResult> GetPartInfo(int cityId, int regionId, string? term)
        {
            var result = await shopService.GetParishList(cityId, regionId, term);
            return Ok(result);
        }
    }
}
