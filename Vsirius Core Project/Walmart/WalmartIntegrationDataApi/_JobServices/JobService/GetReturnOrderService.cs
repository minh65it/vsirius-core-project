using MongoDB.Driver;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi.CollectionMongo;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Return;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class GetReturnOrderService : IGetReturnOrderService
    {
        private readonly IInteractWalmartSellerCommonService _interactWalmart;
        private readonly IOrderWalmartRepository _walmartRep;
        private readonly IConfiguration _configuration;

        public GetReturnOrderService(
            IOrderWalmartRepository walmartRep,
            IConfiguration configuration,
            IInteractWalmartSellerCommonService interactWalmart)
        {
            _configuration = configuration;
            _walmartRep = walmartRep;
            _interactWalmart = interactWalmart;
        }
        public async Task GetReturnOrder()
        {
            Util.WriteLog("----------------------------------Start Get Return Orders from Walmart---------------------------------------------");

            try
            {
                List<ReturnOrder> orders = new();
                var offset = 10;
                var limit = 10;
                string nextCursor = "*";
                TokenResult tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();
                DataReturnDTO? firstData = await GetReturnOrdersWithPaging(0, limit, nextCursor, tokenResult);
                orders.AddRange(firstData!.returnOrders!);
                var totalItems = firstData!.meta!.totalCount!;
                nextCursor = firstData!.meta!.nextCursor!;

                while (offset < totalItems)
                {
                    offset += limit;
                    if (offset > totalItems)
                    {
                        limit = totalItems - (offset - 100);
                    }
                    var item = await GetReturnOrdersWithPaging(0, limit, nextCursor, tokenResult);
                    totalItems = item!.meta!.totalCount!;
                    nextCursor = item!.meta!.nextCursor!;
                    orders.AddRange(item!.returnOrders!);
                }

                if (orders.Any())
                {
                    await CreateUpdateReturnOrderToDb(orders);
                }

                orders = null;
                nextCursor = null;
                tokenResult = null;
                firstData = null;

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Get return order. Err: {ex.Message}");
            }

            Util.WriteLog("----------------------------------End Get Return Orders from Walmart---------------------------------------------");

        }

        private async Task<DataReturnDTO?> GetReturnOrdersWithPaging(
           int offset,
           int limit, string nextCursor,
           TokenResult tokenResult,
           int retry = 0)
        {
            var data = new DataReturnDTO();
            try
            {

                var baseUrl = _configuration.GetSection("Walmart:OrderReturnURL").Value!;
                var url =
                    $"{baseUrl}?offset={offset}&limit={limit}&nextCursor={nextCursor}&returnCreationStartDate={DateTime.Now.AddMonths(-1):yyyy-MM-dd}&returnCreationEndDate={DateTime.Now:yyyy-MM-dd}";
                if (nextCursor != "*")
                {
                    url = $"{baseUrl}{nextCursor.Replace("limit=10", $"limit={limit}")}";
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
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<DataReturnDTO>(responseBody);
                    if (data == null)
                    {
                        if (retry < 5)
                        {
                            retry++;
                            await Task.Delay(5000);
                            tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();
                            data = await GetReturnOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    if (retry < 5)
                    {
                        retry++;
                        await Task.Delay(5000);
                        tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();
                        data = await GetReturnOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch
            {
                return null;
            }
            return data;

        }

        private async Task CreateUpdateReturnOrderToDb(List<ReturnOrder> orders)
        {
            List<ReturnOrder> lstOrderLocal = await _walmartRep.FindAll<ReturnOrder>(CollectionName.ReturnOrderWalmart);
            List<string> lstIdLocal = lstOrderLocal.Select(x => x.returnOrderId).ToList();

            var lstUpdate = orders.Where(x => lstIdLocal.Contains(x.returnOrderId)).ToList();
            var updateFilter = Builders<ReturnOrder>.Filter.In(x => x.returnOrderId, lstUpdate.Select(x => x.returnOrderId));
            foreach (var order in lstUpdate)
            {
                var filter = Builders<ReturnOrder>.Filter.Eq(x => x.returnOrderId, order.returnOrderId);
                await _walmartRep.ReplaceOne(CollectionName.ReturnOrderWalmart, order, filter);
            }

            var lstInsert = orders.Where(x => !lstIdLocal.Contains(x.returnOrderId)).ToList();

            await _walmartRep.InsertMany(lstInsert, CollectionName.ReturnOrderWalmart);
        }

    }
}
