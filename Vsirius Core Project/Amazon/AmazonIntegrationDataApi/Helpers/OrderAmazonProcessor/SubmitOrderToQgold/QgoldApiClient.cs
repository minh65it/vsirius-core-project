using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold
{
    public interface IQgoldApiClient
    {
        public Task<bool> SubmitOrder(List<QgoldFtpOrderObject> orders, bool? isResubmit);
        public Task<List<QgoldOrderDto>?> GetOrder(int days = 0);
    }
    public class QgoldApiClient : IQgoldApiClient
    {
        private readonly IConfiguration _config;
        private string? url = "";
        private string? OrderProcessApiClientId = "";
        private string? OrderProcessApiClientSecret = "";
        public QgoldApiClient(IConfiguration config)
        {
            _config = config;

            string? ip = _config.GetSection("OrderProcessApi:Ip").Value;
            url = $"{ip}{_config.GetSection("OrderProcessApi:SubmitQgold").Value}";
            OrderProcessApiClientId = _config.GetSection("OrderProcessApi:ClientId").Value;
            OrderProcessApiClientSecret = _config.GetSection("OrderProcessApi:ClientSecret").Value;
        }

        public async Task<bool> SubmitOrder(List<QgoldFtpOrderObject> orders, bool? isResubmit)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromHours(2);
                    httpClient.DefaultRequestHeaders.Add("clientId", OrderProcessApiClientId);
                    httpClient.DefaultRequestHeaders.Add("clientSecret", OrderProcessApiClientSecret);
                    if (isResubmit == true)
                    {
                        url = url + "?isResubmit=true";
                    }
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(url);

                    string requestBody = JsonSerializer.Serialize(orders);
                    httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    //httpClient.DefaultRequestHeaders.ConnectionClose = true ;
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
        public async Task<List<QgoldOrderDto>?> GetOrder(int days = 0)
        {
            try
            {
                List<QgoldOrderDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationOrderAsync(1, pageSize, days);
                var totalPages = 0;
                List<QgoldOrderDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Data;
                    totalPages = dataFirstCall.PageCount;
                    allResults.AddRange(firstData!);
                    await Utilities2.WriteLogAsync($"Total pages Get Qgold Order: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationResult<QgoldOrderDto>?>> tasks = new();
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
                                allResults.AddRange(result.Data);
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
        private async Task<PaginationResult<QgoldOrderDto>?> GetPaginationOrderAsync(int pageNumber, int pageSize, int days = 0)
        {
            string url = _config.GetValue<string>("OrderProcessApi:Ip")! + _config.GetValue<string>("OrderProcessApi:QgoldOrder")!;

            PaginationResult<QgoldOrderDto>? result = null;

            var parameters = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() },
            };
            if(days> 0) 
            {
                parameters.Add("FromDate", DateTime.Now.AddDays (-days).ToString("yyyy-MM-dd"));
                parameters.Add("ToDate", DateTime.Now.AddDays (1).ToString("yyyy-MM-dd"));
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
                    result = JsonConvert.DeserializeObject<PaginationResult<QgoldOrderDto>>(json);
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
