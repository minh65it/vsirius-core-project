using MongoDB.Driver;
using Newtonsoft.Json;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class GetOrderWalmartService : IGetOrderWalmartService
    {
        private readonly IInteractWalmartSellerCommonService _interactWalmartSellerCommonService;
        private readonly IConfiguration _configuration;
        private readonly IOrderWalmartRepository _orderWalmartRepository;

        public GetOrderWalmartService(
            IInteractWalmartSellerCommonService interactWalmartSellerCommonService,
            IConfiguration configuration,
            IOrderWalmartRepository orderWalmartRepository)
        {
            _interactWalmartSellerCommonService = interactWalmartSellerCommonService;
            _configuration = configuration;
            _orderWalmartRepository = orderWalmartRepository;
        }
        public async Task GetOrderWalmart()
        {
            Util.WriteLog("----------------------------------Start Get Orders from Walmart---------------------------------------------");
            try
            {
                Util.WriteLog("Get Orders From Walmart Seller Start");

                List<Order> lstOrder = await GetOrdersFromWalmartSeller();

                Util.WriteLog($"Total Order {lstOrder.Count}");

                Util.WriteLog("Get Orders From Walmart Seller End");

                if (lstOrder != null && lstOrder.Count > 0)
                {
                    Util.WriteLog("Start Create Update Orders From Walmart Seller");

                    await CreateUpdateOrderToDb(lstOrder);

                    Util.WriteLog("End Create Update Orders From Walmart Seller");
                }
                else
                {
                    Util.WriteLog("Not have order. Process end!");
                }

                lstOrder = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Trigger GetOrderWalmart failed. Err:{ex.Message}");
            }
            Util.WriteLog("----------------------------------End Get Orders from Walmart---------------------------------------------");
        }

        private async Task CreateUpdateOrderToDb(List<Order> lstOrder)
        {
            List<Order> lstOrderLocal = await _orderWalmartRepository.FindAll<Order>(nameof(Order));
            List<string> lstPOLocal = lstOrderLocal.Select(x => x.purchaseOrderId).ToList();

            var lstUpdate = lstOrder.Where(x => lstPOLocal.Contains(x.purchaseOrderId)).ToList();
            foreach (var order in lstUpdate)
            {
                order.PullUpdateDate = DateTime.Now;
                var filter = Builders<Order>.Filter.Eq(x => x.purchaseOrderId, order.purchaseOrderId);
                await _orderWalmartRepository.ReplaceOne(nameof(Order), order, filter);
            }

            var lstInsert = lstOrder.Where(x => !lstPOLocal.Contains(x.purchaseOrderId)).ToList();
            lstInsert.ForEach(x => x.PullUpdateDate = DateTime.Now);
            await _orderWalmartRepository.InsertMany(lstInsert, nameof(Order));
        }

        private async Task<List<Order>> GetOrdersFromWalmartSeller()
        {
            List<Order> orders = new();
            try
            {
                var offset = 200;
                var limit = 200;
                string nextCursor = "*";
                TokenResult tokenResult = await _interactWalmartSellerCommonService.GetTokenFromWalmartSeller();
                OrderWalmartSellerDto firstData = await GetOrdersWithPaging(0, limit, nextCursor, tokenResult);
                orders.AddRange(firstData.list.elements.order);
                var totalItems = firstData.list.meta.totalCount;
                Util.WriteLog($"Total Order in Seller: {totalItems}");
                nextCursor = firstData.list.meta.nextCursor;

                while (offset < totalItems)
                {
                    offset += limit;
                    if (offset > totalItems)
                    {
                        limit = totalItems - (offset - 200);
                    }
                    var item = await GetOrdersWithPaging(0, limit, nextCursor, tokenResult);
                    totalItems = item.list.meta.totalCount;
                    nextCursor = item.list.meta.nextCursor;
                    orders.AddRange(item.list.elements.order);
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Err: {ex.Message}");
            }
            return orders;
        }

        private async Task<OrderWalmartSellerDto> GetOrdersWithPaging(
            int offset,
            int limit,
            string nextCursor,
            TokenResult tokenResult,
            int retry = 0)
        {
            var data = new OrderWalmartSellerDto();
            try
            {
                var baseUrl = _configuration.GetSection("Walmart:OrderURL").Value!;
                var url =
                    $"{baseUrl}?offset={offset}&limit={limit}&nextCursor={nextCursor}&createdStartDate={DateTime.Now.AddDays(-7):yyyy-MM-dd}";
                if (nextCursor != "*")
                {
                    url = $"{baseUrl}{nextCursor.Replace("limit=200", $"limit={limit}")}";
                }
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    data = JsonConvert.DeserializeObject<OrderWalmartSellerDto>(responseBody);
                    if (data == null)
                    {
                        if (retry < 5)
                        {
                            retry++;
                            await Task.Delay(5000);
                            Util.WriteLog($"Err: {responseBody}");
                            tokenResult = await _interactWalmartSellerCommonService.GetTokenFromWalmartSeller();
                            data = await GetOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
                        }
                        else
                        {
                            Util.WriteLog($"Process failed");
                        }
                    }
                }
                else
                {
                    if (retry < 5)
                    {
                        retry++;
                        await Task.Delay(5000);
                        Util.WriteLog($"Err: {responseBody}");
                        tokenResult = await _interactWalmartSellerCommonService.GetTokenFromWalmartSeller();
                        data = await GetOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
                    }
                    else
                    {
                        Util.WriteLog($"Process failed");
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"offset: {offset}; limit: {limit}. Err: {ex.Message}");
                throw;
            }
            return data;
        }
    }
}
