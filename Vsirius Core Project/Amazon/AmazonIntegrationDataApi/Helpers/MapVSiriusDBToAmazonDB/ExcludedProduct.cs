using AmazonIntegrationDataApi.Helpers.Ultilities;
using FikaAmazonAPI.AmazonSpApiSDK.Services;
using Newtonsoft.Json;
using System.Text;

namespace AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB
{
    public interface IExcludedProduct
    {
        public Task<List<ExcludedProductDto>?> GetExcludedProducts();
        public Task<bool> UpdateExcludedProducts(List<ExcludedProductDto> listUpdate);
    }
    public class ExcludedProduct : IExcludedProduct
    {
        private readonly IConfiguration _configuration;

        public ExcludedProduct(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ExcludedProductDto>?> GetExcludedProducts()
        {
            try
            {
                List<ExcludedProductDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationExcludedProductAsync(1, pageSize);
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

                //List rest data
                List<Task<PaginationUtility<ExcludedProductDto>?>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationExcludedProductAsync(pageNumber, pageSize));

                    if (amountRunTask < 3 && i != totalPages)
                    {
                        amountRunTask++;
                    }
                    else
                    {
                        amountRunTask = 0;
                        var results = await Task.WhenAll(tasks);
                        foreach (var result in results)
                        {
                            if (result != null)
                            {
                                allResults.AddRange(result.Result);
                            }
                            else
                            {
                                await Utilities2.WriteLogAsync($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
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
                if(result != null)
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
    }
}
