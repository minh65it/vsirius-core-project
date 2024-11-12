using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AutoMapper;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold
{
    public interface IQgoldApiClient
    {
        public Task<bool> SubmitOrder(List<QgoldFtpOrderObject> orders, bool? isResubmit);
        public Task<bool> SubmitOrder_RFT(List<QgoldFtpOrderObject> orders, bool? isResubmit);
        public Task<List<QgoldOrderDto>?> GetOrder(int days = 0);
    }
    public class QgoldApiClient : IQgoldApiClient
    {
        private readonly IConfiguration _config;
        private string? url = "";
        private string? OrderProcessApiClientId = "";
        private string? OrderProcessApiClientSecret = "";
        private readonly OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService _qgoldProcessService;
        private readonly OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService _submitOrderQgoldService;
        private readonly IMapper _mapper;
        public QgoldApiClient(IConfiguration config, 
            OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService qgoldProcessService, 
            IMapper mapper,
            OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService submitOrderQgoldService)
        {
            _config = config;

            string? ip = _config.GetSection("OrderProcessApi:Ip").Value;
            url = $"{ip}{_config.GetSection("OrderProcessApi:SubmitQgold").Value}";
            OrderProcessApiClientId = _config.GetSection("OrderProcessApi:ClientId").Value;
            OrderProcessApiClientSecret = _config.GetSection("OrderProcessApi:ClientSecret").Value;
            _qgoldProcessService = qgoldProcessService;
            _mapper = mapper;
            _submitOrderQgoldService = submitOrderQgoldService;
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

        public async Task<bool> SubmitOrder_RFT(List<QgoldFtpOrderObject> orders, bool? isResubmit)
        {
            try
            {
                // Map Amazon order to Qgold order
                List<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject> list = new List<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>();
                foreach (var item in orders)
                {
                    OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject dto = _mapper.Map<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>(item);
                    list.Add(dto);
                }
                var data = await _submitOrderQgoldService.SubmitQgold(list, true);
                return data;
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
                var dataFirstCall = await GetPaginationOrderAsync_RFT(1, pageSize, days);
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

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationOrderAsync_RFT(pageNumber, pageSize, days);

                    if (getData.Data!= null && getData.Data.Count > 0)
                    {
                        allResults.AddRange(getData.Data);
                    }
                    else
                    {
                        await Utilities2.WriteLogAsync($"Failed when add record");
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
            if (days > 0)
            {
                parameters.Add("FromDate", DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd"));
                parameters.Add("ToDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
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

        private async Task<PaginationResult<QgoldOrderDto>?> GetPaginationOrderAsync_RFT(int pageNumber, int pageSize, int days = 0)
        {
            PaginationResult<QgoldOrderDto> result = new PaginationResult<QgoldOrderDto>();
            try
            {
                OrderProcessApi.DTOs.PaginationParam pagination = new OrderProcessApi.DTOs.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;

                OrderProcessApi.DTOs.SearchQgoldProcessParam search = new OrderProcessApi.DTOs.SearchQgoldProcessParam();
                search.FromDate = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
                search.ToDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                var data = await _qgoldProcessService.GetOrders(pagination, search);
                result.Data = new List<QgoldOrderDto>();
                result.PageNumber = data.PageNumber;
                result.PageSize = data.PageSize;
                result.TotalCount = data.TotalCount;
                result.PageCount = data.PageCount;
                foreach (var item in data.Data)
                {
                    QgoldOrderDto dto = _mapper.Map<QgoldOrderDto>(item);
                    result.Data.Add(dto);
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call service GetOrder failed with exception {ex.Message} at page {pageNumber}");
            }
            return result;
        }
    }
}
