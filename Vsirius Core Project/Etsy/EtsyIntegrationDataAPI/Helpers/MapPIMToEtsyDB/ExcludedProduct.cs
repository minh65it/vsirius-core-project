using AutoMapper;
using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Utilities;
using Newtonsoft.Json;
using System.Text;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB
{
    public interface IExcludedProduct
    {
        public Task<List<ExcludedProductDto>?> GetExcludedProducts();
        public Task<bool> UpdateExcludedProducts(List<ExcludedProductDto> listUpdate);
        public Task<bool> UpdateExcludedProducts_RFT(List<ExcludedProductDto> listUpdate);
    }
    public class ExcludedProduct : IExcludedProduct
    {
        private readonly IConfiguration _configuration;
        private ILoggerCustom _logService;
        private readonly ExcludedProductAPI._Services.Interfaces.IExcludedProductService _serviceExcluded;
        private readonly IMapper _mapper;

        public ExcludedProduct(IConfiguration configuration,
            ILoggerCustom logService,
            IMapper mapper,
            ExcludedProductAPI._Services.Interfaces.IExcludedProductService serviceExcluded)
        {
            _configuration = configuration;
            _logService = logService;
            _serviceExcluded = serviceExcluded;
            _mapper = mapper;
        }

        public async Task<List<ExcludedProductDto>?> GetExcludedProducts()
        {
            try
            {
                List<ExcludedProductDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationExcludedProductAsync_RFT(1, pageSize);
                var totalPages = 0;
                List<ExcludedProductDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData!);
                    await _logService.CreateLogInformation($"Total pages GetExcludedProducts: {totalPages}");
                }
                else
                {
                    return null;
                }

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationExcludedProductAsync_RFT(pageNumber, pageSize);

                    foreach (var result in getData.Result)
                    {
                        if (result != null)
                        {
                            allResults.AddRange(getData.Result);
                        }
                        else
                        {
                            await _logService.CreateLogError($"Failed when add record");
                        }
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"GetExcludedProducts Failed with exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateExcludedProducts(List<ExcludedProductDto> listUpdate)
        {
            try
            {
                var json = JsonConvert.SerializeObject(listUpdate);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _configuration.GetValue<string>("APIExcludedProduct:CreateUpdate")!;
                using var client = new HttpClient();

                var response = await client.PostAsync(url, data);

                string resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResponseData>(resultString);
                if (result != null)
                {
                    return result.Success;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Call Api CreateUpdate failed: with exception {ex.Message}");
                return false;
            }

        }

        public async Task<bool> UpdateExcludedProducts_RFT(List<ExcludedProductDto> listUpdate)
        {
            try
            {
                var list = new List<ExcludedProductAPI.DTOs.ExcludedProductDTO>();
                foreach (var item in listUpdate)
                {
                    var dto = _mapper.Map<ExcludedProductAPI.DTOs.ExcludedProductDTO>(item);
                }
                var data = await _serviceExcluded.BulkCreateUpdate(list);
                return data.Success;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Get excluded product failed: with exception {ex.Message}");
                return false;
            }
        }
        public class ResponseData
        {
            public string Caption { get; set; }
            public string Message { get; set; }
            public bool Success { get; set; }
            public object Data { get; set; }
            public object ValidateData { get; set; }
        }

        private async Task<PaginationUtility<ExcludedProductDto>?> GetPaginationExcludedProductAsync(int pageNumber, int pageSize)
        {
            string url = _configuration.GetValue<string>("APIExcludedProduct:PullData")!;

            PaginationUtility<ExcludedProductDto>? result = null;

            string apiUrl = $"{url}?Marketplace=Esty&PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<ExcludedProductDto>>(json);
                }
                else
                {
                    await _logService.CreateLogError($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<PaginationUtility<ExcludedProductDto>?> GetPaginationExcludedProductAsync_RFT(int pageNumber, int pageSize)
        {
            var result = new PaginationUtility<ExcludedProductDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new ExcludedProductAPI.Helpers.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;

                var search = new ExcludedProductAPI.DTOs.ExcludedProductParam();
                search.Marketplace = "Etsy";

                var data = await _serviceExcluded.GetAll(pagination, search);

                result.Result = new List<ExcludedProductDto>();
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<ExcludedProductDto>(item);
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
                await _logService.CreateLogError($"Get product excluded failed with exception {ex.Message} at page {pageNumber}");
                return result;
            }
        }
    }
}
