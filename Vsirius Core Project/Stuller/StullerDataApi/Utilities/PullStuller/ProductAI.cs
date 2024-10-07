using Newtonsoft.Json;
using StullerApiClient.Models;

namespace StullerDataApi.Utilities.PullStuller
{
    public interface IProductAI
    {
        public Task PushToProductAI(List<Product> dataStuller);
    }
    public class ProductAI : IProductAI
    {
        private readonly IConfiguration _config;

        public ProductAI(IConfiguration config)
        {
            _config = config;
        }

        public async Task PushToProductAI(List<Product> dataStuller)
        {
            var lstAllSkuProductAI = await GetAllSkuProductAI();
            var i = 1;
            foreach (var item in dataStuller)
            {
                if (lstAllSkuProductAI.Any(x => x.Equals(item.SKU)))
                {
                    continue;
                }
                else
                {
                    if (i < 20)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                    bool IsSuccessInsert = await CreateProductAI(item);
                    if (!IsSuccessInsert) break;
                }
            }

        }
        private async Task<bool> CreateProductAI(Product item)
        {
            var param = new CreateUpdateProductAiParam()
            {
                Brand = "VSirius",
                Description = item.Description,
                SKU = item.SKU
            };

            try
            {
                var url = _config.GetValue<string>("ProductAiApi:Create");
                var httpClient = new HttpClient();
                var jsonContent = JsonContent.Create(param);
                HttpResponseMessage response = await httpClient.PostAsync(url, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OperationResult1<ProductAiDto>>(json);
                    return result.Success;
                }
                else
                {
                    Common.WriteLog($"Call Api failed: {url} with error internal error: {response.StatusCode}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Common.WriteLog("CreateProductAI failed. Exception: " + ex.Message);
                return false;
            }
        }
        private async Task<List<string>> GetAllSkuProductAI()
        {
            var lstAllSkuProductAI = new List<string>();

            try
            {
                var pageSize = 3000;
                PaginationUtility<string> dataFirstCall = await GetPaginationResultAsync(1, pageSize);
                var totalPages = 0;
                List<string> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstAllSkuProductAI.AddRange(firstData!);
                    Common.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<string>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResultAsync(pageNumber, pageSize));

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
                                lstAllSkuProductAI.AddRange(result.Result);
                            }
                            else
                            {
                                Common.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteLog("GetAllSkuProductAI failed. Err:" + ex.Message);
            }

            return lstAllSkuProductAI;
        }
        private async Task<PaginationUtility<string>> GetPaginationResultAsync(int pageNumber, int pageSize)
        {
            string url = _config.GetSection("ProductAiApi:GetAllSku").Value!;

            PaginationUtility<string> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<string>>(json);
                    Common.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Common.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Common.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }
    }
}
