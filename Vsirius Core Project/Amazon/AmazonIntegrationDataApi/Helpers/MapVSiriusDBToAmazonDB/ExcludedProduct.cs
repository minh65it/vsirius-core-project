using AmazonIntegrationDataApi.Helpers.Ultilities;
using AutoMapper;
using FikaAmazonAPI.AmazonSpApiSDK.Services;
using Newtonsoft.Json;
using System.Text;

namespace AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB
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
        private readonly ExcludedProductAPI._Services.Interfaces.IExcludedProductService _service;
        private readonly IMapper _mapper;

        public ExcludedProduct(IConfiguration configuration, ExcludedProductAPI._Services.Interfaces.IExcludedProductService service, IMapper mapper)
        {
            _configuration = configuration;
            _service = service;
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
                    await Utilities2.WriteLogAsync($"Total pages GetExcludedProducts: {totalPages}");
                }
                else
                {
                    return null;
                }

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationExcludedProductAsync(pageNumber, pageSize);

                    foreach (var result in getData.Result)
                    {
                        if (getData.Result != null)
                        {
                            allResults.AddRange(getData.Result);
                        }
                        else
                        {
                            await Utilities2.WriteLogAsync($"Failed when add record");
                        }
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"GetExcludedProducts Failed with exception: {ex.Message}");
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
                await Utilities2.WriteLogAsync($"Call Api CreateUpdate failed: with exception {ex.Message}");
                return false;
            }

        }

        public async Task<bool> UpdateExcludedProducts_RFT(List<ExcludedProductDto> listUpdate)
        {
            var list = new List<ExcludedProductAPI.DTOs.ExcludedProductDTO>();
            foreach (var item in listUpdate)
            {
                var dto = _mapper.Map<ExcludedProductAPI.DTOs.ExcludedProductDTO>(item);
                list.Add(dto);
            }
            var result = await _service.BulkCreateUpdate(list);
            return result.Success;
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

            string apiUrl = $"{url}?Marketplace=Amazon&PageNumber={pageNumber}&PageSize={pageSize}";

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
                    await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<PaginationUtility<ExcludedProductDto>?> GetPaginationExcludedProductAsync_RFT(int pageNumber, int pageSize)
        {
            PaginationUtility<ExcludedProductDto> result = new PaginationUtility<ExcludedProductDto>(null, 0, 1, 1, 1);
            try
            {
                ExcludedProductAPI.Helpers.PaginationParam pagination = new ExcludedProductAPI.Helpers.PaginationParam();
                pagination.PageSize = pageSize;
                pagination.PageNumber = pageNumber;

                ExcludedProductAPI.DTOs.ExcludedProductParam param = new ExcludedProductAPI.DTOs.ExcludedProductParam();
                param.Marketplace = "Amazon";
                var data = await _service.GetAll(pagination, param);
                result.Result = new List<ExcludedProductDto>();

                foreach (var item in data.Result)
                {
                    ExcludedProductDto dto = _mapper.Map<ExcludedProductDto>(item);
                    result.Result.Add(dto);
                }

                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.Skip = data.Pagination.Skip;
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call service failed with exception {ex.Message} at page {pageNumber}");
                return result;
            }

        }
    }
}
