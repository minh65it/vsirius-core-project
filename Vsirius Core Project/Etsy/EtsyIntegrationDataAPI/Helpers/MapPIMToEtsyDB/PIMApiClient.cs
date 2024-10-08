using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Utilities;
using Newtonsoft.Json;
using System.Text;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB
{
    public interface IPIMApiClient
    {
        public Task<PaginationUtility<ProductPIMDto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore);
        public Task<PaginationUtility<ProductPIMDto>> GetProductTopPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore);
    }
    public class PIMApiClient : IPIMApiClient
    {
        private readonly IConfiguration _configuration;
        private ILoggerCustom _logService;
        private string url;
        private string urlTopProduct;

        public PIMApiClient(IConfiguration configuration, ILoggerCustom logService)
        {
            _configuration = configuration;
            url = _configuration.GetSection("PIM:UrlGetAllData").Value!;
            urlTopProduct = _configuration.GetSection("PIM:UrlGetTopProduct").Value!;
            _logService = logService;
        }

        public async Task<PaginationUtility<ProductPIMDto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<ProductPIMDto> result = new PaginationUtility<ProductPIMDto>(null, 1, 1, 1, 1);
            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}"; //All data
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    HttpContent content = new StringContent("{}", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PaginationUtility<ProductPIMDto>>(json)!;
                    }
                    else
                    {
                        await _logService.CreateLogError($"Call Api failed: {apiUrl} with Error internal error: {response.StatusCode}");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Call Api failed: {apiUrl} with exception {ex.Message}");
                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<PaginationUtility<ProductPIMDto>> GetProductTopPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<ProductPIMDto> result = new PaginationUtility<ProductPIMDto>(null, 1, 1, 1, 1);
            string apiUrl = $"{urlTopProduct}?isSortQuantity=true&isPagging=true&PageNumber={pageNumber}&PageSize={pageSize}"; //All data
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PaginationUtility<ProductPIMDto>>(json)!;
                    }
                    else
                    {
                        await _logService.CreateLogError($"Call Api failed: {apiUrl} with Error internal error: {response.StatusCode}");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Call Api failed: {apiUrl} with exception {ex.Message}");
                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
