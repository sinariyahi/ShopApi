using Application.Interfaces.Shop;
using Infrastructure.Common;
using Infrastructure.Models.Shop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services.Shop
{
    public class ShopService : IShopService
    {
        private readonly Configs _configs;
        private readonly ILogger logger;

        public ShopService(IOptions<Configs> options, ILogger<ShopService> logger)
        {
            _configs = options.Value;
            this.logger = logger;
        }

        private HttpClient GenerateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{_configs.GoldiranAPI}/");

            httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _configs.GoldiranAPIToken);

            return httpClient;
        }

        public async Task<ShopActionResult<List<KeyValueDto>>> GetBasicData(int datatype, int parentId)
        {
            logger.LogInformation($"call GetBasicData with dataType:{datatype}, parentId:{parentId}");
            var result = new ShopActionResult<List<KeyValueDto>>();
            using (var httpClient = GenerateHttpClient())
            {
                using (var response = await httpClient.GetAsync($"getBasicData?dataType={datatype}&parentId={parentId}"))
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ShopActionResult<List<KeyValueDto>>>();
                    foreach (var kv in apiResponse.Data)
                    {
                        kv.Value = kv.Id;
                        kv.Label = kv.Title;
                        kv.Title = kv.Title;
                        kv.Text = kv.Title;

                    }
                    logger.LogInformation($"call GetBasicData Result:{apiResponse}");
                    return apiResponse;
                }
            }
        }

        public async Task<ShopActionResult<List<ParishItemDto>>> GetParishList(int cityId, int regionId, string term)
        {
            var result = new ShopActionResult<List<ParishItemDto>>();
            using (var httpClient = GenerateHttpClient())
            {
                using (var response = await httpClient.GetAsync($"getParishList?cityId={cityId}&regionId={regionId}&term={term}"))
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ShopActionResult<List<ParishItemDto>>>();
                    foreach (var kv in apiResponse.Data)
                    {
                        kv.Value = kv.Id;
                        kv.Label = kv.ParishName;
                        kv.Text = kv.ParishName;
                        kv.Title = kv.ParishName;

                    }
                    return apiResponse;
                }
            }
        }

        public async Task<ShopActionResult<PartBalanceInfoDto>> GetPartBalanceInfo(string partNo)
        {
            var result = new ShopActionResult<PartBalanceInfoDto>();
            using (var httpClient = GenerateHttpClient())
            {
                using (var response = await httpClient.GetAsync($"getPartBalanceInfo?partNo={partNo}"))
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ShopActionResult<PartBalanceInfoDto>>();
                    return apiResponse;
                }
            }
        }

        public async Task<ShopActionResult<string>> RegisterOnlineSale(RegisterOnlineSaleDto data)
        {
            var result = new ShopActionResult<string>();
            var jsonData = JsonSerializer.Serialize(data);
            var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var httpClient = GenerateHttpClient())
            {
                using (var response = await httpClient.PostAsync("registerOnlineSale", requestContent))
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ShopActionResult<string>>();
                    response.EnsureSuccessStatusCode();
                    return apiResponse;
                }
            }
        }
    }
}
