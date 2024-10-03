using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;

namespace PhillyLuxEcommerce.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductDetailService(IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<VsiriusProductDto> GetProduct(string productId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ProductDetailData");

                var httpResponseMessage = await httpClient.GetAsync($"?item_sku={productId}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<VsiriusProductDto>(json);
                    return data;
                }

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return null;
        }

        public async Task<List<VsiriusProductDto>> GetRelatedProducts(string filter)
        {

            try
            {
                var httpClient = _httpClientFactory.CreateClient("RelatedProductData");

                var httpResponseMessage = await httpClient.GetAsync($"?Filter={filter}&TakeProduct=20");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<VsiriusProductDto>>(json);
                    return data;
                }

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }

            return new List<VsiriusProductDto>();
        }
    }
}
