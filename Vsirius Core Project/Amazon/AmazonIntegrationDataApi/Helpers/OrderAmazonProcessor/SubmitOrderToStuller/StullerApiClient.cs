using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.OrderStuller;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller
{
    public interface IStullerApiClient
    {
        public Task<bool> SubmitOrder(SubmitOrderParam orders, bool? isResubmit);
        public Task<List<StullerOrderDTO>?> GetOrder(int days = 0);
    }
    public class StullerApiClient : IStullerApiClient
    {
        private readonly IConfiguration _config;
        private string? url = "";
        public StullerApiClient(IConfiguration config)
        {
            _config = config;

            string? ip = _config.GetSection("OrderStullerApi:Ip").Value;
            url = $"{ip}{_config.GetSection("OrderStullerApi:SubmitStuller").Value}";
        }

        public async Task<bool> SubmitOrder(SubmitOrderParam orders, bool? isResubmit)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromHours(2);
                    if (isResubmit == true)
                    {
                        url = url + "?isResubmit=true";
                    }
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(url);

                    string requestBody = JsonSerializer.Serialize(orders);
                    httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.SendAsync(httpRequestMessage);
                    if (response.IsSuccessStatusCode)
                    {

                        var responseContent = await response.Content.ReadFromJsonAsync<bool>();
                        return responseContent;
                    }
                    else
                    {
                        return false;
                    }
                };
            }
            catch (Exception e)
            {
                await Utilities2.WriteLogAsync("Could not SubmitOrder " + e.ToString());
                return false;
            }

        }
        public async Task<List<StullerOrderDTO>?> GetOrder(int days = 0)
        {
            try
            {
                List<StullerOrderDTO> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationOrderAsync(1, pageSize, days);
                var totalPages = 0;
                List<StullerOrderDTO> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData!);
                    await Utilities2.WriteLogAsync($"Total pages Get Qgold Order: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<StullerOrderDTO>?>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationOrderAsync(pageNumber, pageSize, days));

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
        private async Task<PaginationUtility<StullerOrderDTO>?> GetPaginationOrderAsync(int pageNumber, int pageSize, int days = 0)
        {
            string url = _config.GetValue<string>("OrderStullerApi:Ip")! + _config.GetValue<string>("OrderStullerApi:SubmitStuller")!;

            PaginationUtility<StullerOrderDTO>? result = null;

            var parameters = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() },
            };
            if (days > 0)
            {
                parameters.Add("FromDate", DateTime.Now.AddDays(-days).ToString("MM-dd-yyyy"));
                parameters.Add("ToDate", DateTime.Now.AddDays(1).ToString("MM-dd-yyyy"));
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
                    result = JsonConvert.DeserializeObject<PaginationUtility<StullerOrderDTO>>(json);
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
