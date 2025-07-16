using AutoMapper;
using Azure;
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
        public Task<PaginationUtility<ProductPIMDto>> GetPaginationResult_RFT(int pageNumber, int pageSize, SemaphoreSlim semaphore);
        public Task<PaginationUtility<ProductPIMDto>> GetProductTopPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore);
    }
    public class PIMApiClient : IPIMApiClient
    {
        private readonly IConfiguration _configuration;
        private ILoggerCustom _logService;
        private string url;
        private string urlTopProduct;
        private readonly PimApi._Services.Interfaces.IProductService _servicePim;
        private readonly IMapper _mapper;

        public PIMApiClient(IConfiguration configuration, ILoggerCustom logService, PimApi._Services.Interfaces.IProductService servicePim, IMapper mapper)
        {
            _configuration = configuration;
            url = _configuration.GetSection("PIM:UrlGetAllData").Value!;
            urlTopProduct = _configuration.GetSection("PIM:UrlGetTopProduct").Value!;
            _logService = logService;
            _servicePim = servicePim;
            _mapper = mapper;
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

        public async Task<PaginationUtility<ProductPIMDto>> GetPaginationResult_RFT(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<ProductPIMDto> result = new PaginationUtility<ProductPIMDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new PimApi.Helpers.Utilities.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;

                var search = new PimApi.Dtos.SearchParams();
                var data = await _servicePim.GetData(pagination, search, true);

                result.Result = new List<ProductPIMDto>();
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<ProductPIMDto>(item);
                    result.Result.Add(dto);
                }

                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.Skip = data.Pagination.Skip;

                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Get producr from Pim failed with exception {ex.Message}");
                return result;
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
