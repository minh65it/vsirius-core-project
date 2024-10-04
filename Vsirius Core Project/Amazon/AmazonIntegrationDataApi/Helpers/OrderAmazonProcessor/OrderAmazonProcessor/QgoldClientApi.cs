using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using Newtonsoft.Json;
using System.Text;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor
{
    public interface IQgoldClientApi
    {
        public Task<List<OrderMarketplaceDto>?> GetOrderMarketplace(OrderMarketplaceParams? paramSearch);
        public Task<OrderMarketplaceDto> GetDetailOrderBySellerMarketplace(string orderSellerId);
        public Task<OrderMarketplaceDto> GetDetailOrderByIdMarketplace(string orderSellerId);
        Task<bool> AddReSubmitOrder(OrderMarketplaceDto returnOrder);
    }
    public class QgoldClientApi : IQgoldClientApi
    {
        private readonly IConfiguration _configuration;

        public QgoldClientApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddReSubmitOrder(OrderMarketplaceDto returnOrder)
        {
            try
            {
                string url = $"{_configuration.GetSection("OrderProcessApi:Ip").Value}{_configuration.GetSection("OrderProcessApi:ReSubmitOrder").Value}";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromHours(2);
                    //httpClient.DefaultRequestHeaders.Add("clientId", OrderProcessApiClientId);
                    //httpClient.DefaultRequestHeaders.Add("clientSecret", OrderProcessApiClientSecret);
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(url);

                    string requestBody = System.Text.Json.JsonSerializer.Serialize(returnOrder);
                    httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    //httpClient.DefaultRequestHeaders.ConnectionClose = true ;
                    var response = await httpClient.SendAsync(httpRequestMessage);
                    if (response.IsSuccessStatusCode)
                    {

                        var responseContent = await response.Content.ReadFromJsonAsync<MongoOperationResult>();
                        return responseContent!.Success;
                    }
                    else
                    {
                        return false;
                    }
                };
            }
            catch (Exception e)
            {
                await Utilities2.WriteLogAsync("Could not ReSubmitOrder " + e.ToString());
                return false;
            }
        }

        public async Task<OrderMarketplaceDto> GetDetailOrderByIdMarketplace(string orderSellerId)
        {
            string url = _configuration.GetValue<string>("OrderProcessApi:Ip")! + _configuration.GetValue<string>("OrderProcessApi:DetailOrderByIdMarketplace")!;

            OrderMarketplaceDto? result = null;

            string apiUrl = $"{url}??orderId={orderSellerId}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrderMarketplaceDto>(json);
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

        public async Task<OrderMarketplaceDto> GetDetailOrderBySellerMarketplace(string orderSellerId)
        {
            string url = _configuration.GetValue<string>("OrderProcessApi:Ip")! + _configuration.GetValue<string>("OrderProcessApi:DetailOrderBySellerMarketplace")!;

            OrderMarketplaceDto? result = null;

            string apiUrl = $"{url}??orderSellerId={orderSellerId}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrderMarketplaceDto>(json);
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

        public async Task<List<OrderMarketplaceDto>?> GetOrderMarketplace(OrderMarketplaceParams? paramSearch)
        {
            try
            {
                List<OrderMarketplaceDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationOrderMarketplaceAsync(1, pageSize, paramSearch);
                var totalPages = 0;
                List<OrderMarketplaceDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Data;
                    totalPages = dataFirstCall.PageCount;
                    allResults.AddRange(firstData!);
                    await Utilities2.WriteLogAsync($"Total pages GetOrderMarketplace: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationResult<OrderMarketplaceDto>?>> tasks = new();
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
        private async Task<PaginationResult<OrderMarketplaceDto>?> GetPaginationOrderMarketplaceAsync(int pageNumber, int pageSize, OrderMarketplaceParams? paramSearch)
        {
            string url = _configuration.GetValue<string>("OrderProcessApi:Ip")! + _configuration.GetValue<string>("OrderProcessApi:QgoldOrderMarketplace")!;

            PaginationResult<OrderMarketplaceDto>? result = null;

            var parameters = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() },
            };
            if(paramSearch != null)
            {
                if (!string.IsNullOrWhiteSpace(paramSearch.OrderId))
                {
                    parameters.Add("OrderId", paramSearch.OrderId);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.ShipToName))
                {
                    parameters.Add("ShipToName", paramSearch.ShipToName);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.ShipToAddress1))
                {
                    parameters.Add("ShipToAddress1", paramSearch.ShipToAddress1);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.ShipToAddress2))
                {
                    parameters.Add("ShipToAddress2", paramSearch.ShipToAddress2);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.ShipToAddress3))
                {
                    parameters.Add("ShipToAddress3", paramSearch.ShipToAddress3);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.Seller_Order_ID))
                {
                    parameters.Add("Seller_Order_ID", paramSearch.Seller_Order_ID);
                }
                if (!string.IsNullOrWhiteSpace(paramSearch.Marketplace))
                {
                    parameters.Add("Marketplace", paramSearch.Marketplace);
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
                    result = JsonConvert.DeserializeObject<PaginationResult<OrderMarketplaceDto>>(json);
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
