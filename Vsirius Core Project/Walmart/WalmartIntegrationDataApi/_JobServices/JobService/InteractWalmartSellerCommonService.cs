using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Dtos.SellerItem;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class InteractWalmartSellerCommonService : IInteractWalmartSellerCommonService
    {
        private readonly IConfiguration _configuration;
        public InteractWalmartSellerCommonService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Item>> GetItemsFromSeller(bool isPublish = false, bool isActive = false)
        {
            var lstData = new List<Item>();
            try
            {
                var offset = 5000;
                var limit = 5000;
                string nextCursor = "*";
                TokenResult tokenResult = await GetTokenFromWalmartSeller();

                ItemSellerInfo? firstData =
                    await GetListSkuSellerWithPaging(
                        0, limit, nextCursor, tokenResult, isPublish: isPublish, isActive: isActive);

                lstData.AddRange(firstData!.ItemResponse!);
                var totalItems = firstData.totalItems;
                Util.WriteLog($"Total Item in Seller: {totalItems}");
                nextCursor = firstData.nextCursor;

                while (offset < totalItems)
                {
                    offset += limit;
                    if (offset > totalItems)
                    {
                        limit = totalItems - (offset - 5000);
                    }
                    var item = await GetListSkuSellerWithPaging(
                        0, limit, nextCursor, tokenResult, isPublish: isPublish, isActive: isActive);
                    totalItems = item.totalItems;
                    nextCursor = item.nextCursor;
                    lstData.AddRange(item.ItemResponse!);
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Err when get items in seller. Exception: {ex.Message}");
            }
            return lstData;
        }

        private async Task<ItemSellerInfo?> GetListSkuSellerWithPaging(
            int offset,
            int limit,
            string nextCursor,
            TokenResult tokenResult,
            bool isPublish,
            bool isActive,
            int retry = 0)
        {
            ItemSellerInfo? data = new();
            try
            {
                var url = $"{_configuration.GetValue<string>("Walmart:GetItemSku")}?offset={offset}&limit={limit}&nextCursor={nextCursor}";
                if (isPublish)
                {
                    url += "&publishedStatus=PUBLISHED";
                }
                if (isActive)
                {
                    url += "&lifecycleStatus=ACTIVE";
                }
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    data = JsonConvert.DeserializeObject<ItemSellerInfo>(responseBody);
                    if (data!.ItemResponse == null)
                    {
                        retry++;
                        if (retry < 5)
                        {
                            await Task.Delay(5000);
                            Util.WriteLog($"Call Data Seller Failed Err: {responseBody}");
                            tokenResult = await GetTokenFromWalmartSeller();
                            data = await GetListSkuSellerWithPaging(offset, limit, nextCursor, tokenResult, isPublish, isActive, retry);
                        }
                        else
                        {
                            Util.WriteLog("Call API Walmart failed");
                            throw new Exception("Call API Walmart failed");
                        }
                    }
                }
                else
                {
                    retry++;
                    if (retry < 5)
                    {
                        await Task.Delay(5000);
                        Util.WriteLog($"Call Data Seller Failed Err: {responseBody}");
                        tokenResult = await GetTokenFromWalmartSeller();
                        data = await GetListSkuSellerWithPaging(offset, limit, nextCursor, tokenResult, isPublish, isActive, retry);
                    }
                    else
                    {
                        Util.WriteLog("Call API Walmart failed");
                        throw new Exception("Call API Walmart failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Call Data Seller Failed offset: {offset}; limit: {limit}. Err: {ex.Message}");
                throw;
            }
            return data;
        }

        public async Task<TokenResult> GetTokenFromWalmartSeller()
        {
            TokenResult tokenResult = new()
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            var url = _configuration.GetSection("Walmart:AccessTokenURL").Value;
            try
            {
                using HttpClient client = new();

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
                request.Headers.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                string credentials =
                Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                            $"{_configuration.GetSection("Walmart:ClientId").Value}:{_configuration.GetSection("Walmart:ClientSecret").Value}"));
                request.Headers.Add("Authorization", "Basic " + credentials);
                var requestData = new Dictionary<string, string>
                    {
                        { "grant_type", "client_credentials" },
                    };
                request.Content = new FormUrlEncodedContent(requestData);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResultAuth>(responseBody);
                    tokenResult.AccessToken = result!.access_token;
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Get Token Seller failed: {ex.Message}");
            }
            return tokenResult;
        }
    }
}
