using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.Dtos;

namespace PhillyLuxEcommerce.Services
{
    public class TopProductService : ITopProductService
    {
        private readonly ILogService _logService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TopProductService(IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SearchProductDto?> SearchProductDataAsync(SearchTopProductParam searchParam)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("SearchTopProduct");

                var queryParameters = new Dictionary<string, dynamic>
                {
                    { nameof(searchParam.feed_product_type), $"{searchParam.feed_product_type}" },
                    { nameof(searchParam.PageNumber) , $"{searchParam.PageNumber}" },
                    { nameof(searchParam.PageSize), $"{searchParam.PageSize}" },
                    { nameof(searchParam.Keyword), $"{searchParam.Keyword}" },
                    { nameof(searchParam.Filter), $"{searchParam.Filter}" },
                };

                var query = string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"));

                var httpResponseMessage = await httpClient.GetAsync($"?{query}");

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
            }

            return new SearchProductDto();
        }
    }
}
