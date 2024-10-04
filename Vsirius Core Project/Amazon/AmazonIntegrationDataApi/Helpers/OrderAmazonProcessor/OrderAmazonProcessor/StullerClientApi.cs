using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using Newtonsoft.Json;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor
{
    public interface IStullerClientApi
    {
        public Task<List<StullerOrderDto>?> GetOrderMarketplace(AmazonProcessSearchParam? paramSearch);
        public Task<StullerOrderDto> GetDetailOrderByIdMarketplace(string orderSellerId);
    }
    public class StullerClientApi : IStullerClientApi
    {
        private readonly IConfiguration _configuration;

        public StullerClientApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<StullerOrderDto> GetDetailOrderByIdMarketplace(string orderSellerId)
        {
            string url = _configuration.GetValue<string>("OrderStullerApi:Ip")! + _configuration.GetValue<string>("OrderStullerApi:DetailOrderByIdMarketplace")!;

            StullerOrderDto? result = null;

            string apiUrl = $"{url}{orderSellerId}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<PaginationUtility<StullerOrderDto>>(json);
                    if(jsonData != null)
                    {
                        return jsonData.Result.Count > 0 ? jsonData.Result[0] : null;
                    }
                    return null;
                }
                else
                {
                    await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with exception {ex.Message}");
            }

            return result;
        }

        public async Task<List<StullerOrderDto>?> GetOrderMarketplace(AmazonProcessSearchParam? paramSearch)
        {
            try
            {
                List<StullerOrderDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationOrderMarketplaceAsync(1, pageSize, paramSearch);
                var totalPages = 0;
                List<StullerOrderDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData!);
                    await Utilities2.WriteLogAsync($"Total pages GetOrderMarketplace: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<StullerOrderDto>?>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationOrderMarketplaceAsync(pageNumber, pageSize, paramSearch));

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
                await Utilities2.WriteLogAsync($"GetOrderMarketplace Failed with exception: {ex.Message}");
                return null;
            }
        }
        private async Task<PaginationUtility<StullerOrderDto>?> GetPaginationOrderMarketplaceAsync(int pageNumber, int pageSize, AmazonProcessSearchParam? paramSearch)
        {
            string url = _configuration.GetValue<string>("OrderStullerApi:Ip")! + _configuration.GetValue<string>("OrderStullerApi:StullerOrderMarketplace")!;

            PaginationUtility<StullerOrderDto>? result = null;

            var parameters = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() },
            };
            if(paramSearch != null)
            {
                if (!string.IsNullOrWhiteSpace(paramSearch.FromDate))
                {
                    parameters.Add("FromDate", paramSearch.FromDate);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.ToDate))
                {
                    parameters.Add("ToDate", paramSearch.ToDate);
                }
            }

            var queryString = string.Join("&", parameters.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));

            string apiUrl = $"{url}?{queryString}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<StullerOrderDto>>(json);
                }
                else
                {
                    await Utilities2.WriteLogAsync($"Call Api Stuller failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call Api Stuller failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }
    }
}
