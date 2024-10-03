using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Services.Interfaces;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class SearchProductService : ISearchProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SearchProductService(IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FilterDto>?> FilterDataAsync(string? feedProductType)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("FilterData");

                var httpResponseMessage = await httpClient.GetAsync($"?feed_product_type={feedProductType}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<FilterDto>>(json);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return null;
        }

        public async Task<SearchProductDto?> SearchProductDataAsync(SearchParam searchParam)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Search");

                var queryParameters = new Dictionary<string, dynamic>
                {
                    { nameof(searchParam.PageNumber) , $"{searchParam.PageNumber}" },
                    { nameof(searchParam.PageSize), $"{searchParam.PageSize}" },
                    { nameof(searchParam.keyWord), $"{searchParam.keyWord}" }
                };

                var query = string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"));

                var requestBody = searchParam.filters;

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PostAsync($"?{query}", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<SearchProductDto>(reponse);

                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }

            return new SearchProductDto();
        } 
        
        public async Task<SearchProductDto?> SearchProductDataAsyncMock(SearchParam searchParam)
        {
            return new SearchProductDto()
            {
                pagination = new PaginationDto()
                {
                    pageNumber = 1,
                    pageSize = 24,
                    skip = 0,
                    totalCount = 1,
                    totalPage = 1
                },
                result = new List<VsiriusProductDto>
                {
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    },
                    new()
                    {
                        item_sku = "S-121933:126:P",
                        main_image_url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/S-121933%3A126%3AP%5C1.jpg",
                        item_name = "14K White 2 Mm Band Size 6.5 121933:126:P",
                        standard_price = "11",
                        bullet_point1 = "ProductDescription"
                    }

                }
            };
        }
    }
}
