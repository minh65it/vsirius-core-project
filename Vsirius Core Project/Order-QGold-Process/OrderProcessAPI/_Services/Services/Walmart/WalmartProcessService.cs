using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Services.IServices.Walmart;
using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using OrderProcessDomain.Walmart;
using System.Net.Http.Headers;
using System.Text;

namespace OrderProcessApi._Services.Services.Walmart
{
    public class WalmartProcessService : IWalmartProcessService
    {
        IQgoldRepository _qgoldDB;
        IAmazonRepository _amazonDB;
        IWalmartRepository _walmartDB;
        IConfiguration _configuration;
        ISubmitOrderQgoldService _submitOrderQgoldService;
        IMapper _mapper;

        public WalmartProcessService(
            IQgoldRepository qgoldDB,
            IAmazonRepository amazonDB,
            IWalmartRepository walmartDB,
            IConfiguration configuration,
            ISubmitOrderQgoldService submitOrderQgoldService,
            IMapper mapper)
        {
            _qgoldDB = qgoldDB;
            _amazonDB = amazonDB;
            _walmartDB = walmartDB;
            _configuration = configuration;
            _submitOrderQgoldService = submitOrderQgoldService;
            _mapper = mapper;
        }

        public async Task<Result> DeleteOrders(List<string> ds)
        {
            var pointDateCanDelete = DateTime.Now.AddDays(-8);

            FilterDefinition<WalmartOrder>? filterWalmart = null;
            FilterDefinition<SubmittedWalmartOrder>? filterSubmit = null;
            FilterDefinition<OrderMarketplace>? filterMarket = null;

            foreach (var item in ds)
            {
                Common.AddOrFilter(ref filterWalmart, Builders<WalmartOrder>.Filter.Eq(x => x.purchaseOrderId, item));
                Common.AddOrFilter(ref filterMarket, Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, item));
                Common.AddOrFilter(ref filterSubmit, Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, item));
            }

            var lstMarketplace = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket)).Data;
            var lstSubmit = (await _walmartDB.Find(CollectionName.WalmartOrderSubmit, filterSubmit)).Data;
            var lstWalmart = (await _walmartDB.Find(CollectionName.WalmartOrder, filterWalmart)).Data;

            var marketError = new StringBuilder();
            foreach (var item in lstMarketplace)
            {
                if (item.TimeStamp > pointDateCanDelete)
                {
                    marketError.Append($"{item.Seller_Order_ID}, ");
                }
                else
                {
                    await _qgoldDB.DeleteMany(CollectionName.QgoldMarketplace,
                        Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, item.Seller_Order_ID));
                }
            }

            var submitErr = new StringBuilder();
            foreach (var item in lstSubmit)
            {
                if (item.SubmittedDate > pointDateCanDelete)
                {
                    submitErr.Append($"{item.WalmartOrderId}, ");
                }
                else
                {
                    await _walmartDB.DeleteMany(CollectionName.WalmartOrderSubmit,
                        Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, item.WalmartOrderId));
                }
            }

            var walmartErr = new StringBuilder();
            foreach (var item in lstWalmart)
            {
                if (item.orderDate > Common.ConvertDateTimeToEpoch(pointDateCanDelete))
                {
                    walmartErr.Append($"{item.purchaseOrderId}, ");
                }
                else
                {
                    await _amazonDB.DeleteMany(CollectionName.WalmartOrder,
                        Builders<WalmartOrder>.Filter.Eq(x => x.purchaseOrderId, item.purchaseOrderId));
                }
            }

            var msg = $"{(marketError.Length > 0 ? marketError + "in OrderMarketplace Qgold delete failed.\n" : "")}" +
                $"{(walmartErr.Length > 0 ? walmartErr + "in Order Walmart delete failed.\n" : "")}" +
                $"{(submitErr.Length > 0 ? submitErr + "in SubmittedOrder Walmart delete failed.\n" : "")}"
                ;
            if (msg.Trim().Length > 0)
            {
                return new Result() { IsSuccess = false, Message = msg };
            }
            return new Result() { IsSuccess = true };
        }

        public async Task<WalmartOrderDetailDTO?> GetDetailOrder(string id)
        {
            WalmartOrderDetailDTO? result = null;

            var walmart = await _walmartDB.FisrtOrDefault(CollectionName.WalmartOrder,
                 Builders<WalmartOrder>.Filter.Eq(x => x.purchaseOrderId, id));
            if (walmart != null)
            {
                var submit = await _walmartDB.FisrtOrDefault(CollectionName.WalmartOrderSubmit,
                    Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, id));
                var market = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace,
                    Builders<OrderMarketplace>.Filter.Eq("Seller_Order_ID", id));
                result = new()
                {
                    customerOrderId = walmart.customerOrderId,
                    purchaseOrderId = walmart.purchaseOrderId,
                    BuyerName = walmart.shippingInfo.postalAddress.name,
                    Address1 = walmart.shippingInfo.postalAddress.address1,
                    Address2 = walmart.shippingInfo.postalAddress.address2,
                    City = walmart.shippingInfo.postalAddress.city,
                    Country = walmart.shippingInfo.postalAddress.country,
                    customerEmailId = walmart.customerEmailId,
                    Phone = walmart.shippingInfo.phone,
                    estimatedDeliveryDate = Common.ConvertEpochToDateTime(walmart.shippingInfo.estimatedDeliveryDate),
                    estimatedShipDate = Common.ConvertEpochToDateTime(walmart.shippingInfo.estimatedShipDate),
                    IsSubmitted = !string.IsNullOrEmpty(submit?.WalmartOrderId),
                    methodShipping = walmart.shippingInfo.methodCode,
                    orderDate = Common.ConvertEpochToDateTime(walmart.orderDate),
                    PostalCode = walmart.shippingInfo.postalAddress.postalCode,
                    QgoldOrderId = market?.OrderId,
                    State = walmart.shippingInfo.postalAddress.state,
                    CarrierName = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                    TrackingNumber = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                    Items = CustomMapping.MapItemWalmart(walmart.orderLines!.orderLine)
                };

            }

            return result;
        }

        public async Task<PaginationResult<WalmartOrderDetailDTO>> GetOrders(
            PaginationParam paginationParam,
            WalmartProcessSearchParam searchParam)
        {
            var result = new PaginationResult<WalmartOrderDetailDTO>();

            FilterDefinition<WalmartOrder>? filterWalmart = null;
            FilterDefinition<OrderMarketplace>? filterQgold = null;


            if (searchParam != null)
            {
                if (searchParam.OrderStatus is not null)
                {
                    switch (searchParam.OrderStatus)
                    {
                        case WalmartOrderType.Created:
                            filterWalmart = Builders<WalmartOrder>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", WalmartOrderType.Created.ToString());
                            break;
                        case WalmartOrderType.Acknowledged:
                            filterWalmart = Builders<WalmartOrder>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", WalmartOrderType.Acknowledged.ToString());
                            break;
                        case WalmartOrderType.Delivered:
                            filterWalmart = Builders<WalmartOrder>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", WalmartOrderType.Delivered.ToString());
                            break;
                        case WalmartOrderType.Shipped:
                            filterWalmart = Builders<WalmartOrder>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", WalmartOrderType.Shipped.ToString());
                            break;
                        case WalmartOrderType.Cancelled:
                            filterWalmart = Builders<WalmartOrder>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", WalmartOrderType.Cancelled.ToString());
                            break;
                    }
                };
                if (!string.IsNullOrWhiteSpace(searchParam.TrackingNumber))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                        Regex("orderLines.orderLine.orderLineStatuses.orderLineStatus.trackingInfo.trackingNumber",
                        new BsonRegularExpression(searchParam.TrackingNumber, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.Address1))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                            Regex("shippingInfo.postalAddress.address1",
                            new BsonRegularExpression(searchParam.Address1, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.BuyerName))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                               Regex("shippingInfo.postalAddress.name",
                               new BsonRegularExpression(searchParam.BuyerName, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.purchaseOrderId))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                               Regex("purchaseOrderId",
                               new BsonRegularExpression(searchParam.purchaseOrderId, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.customerOrderId))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                                   Regex("customerOrderId",
                                   new BsonRegularExpression(searchParam.customerOrderId, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.SKU))
                {
                    var filter = Builders<WalmartOrder>.Filter.
                                       Regex("orderLines.orderLine.item.sku",
                                       new BsonRegularExpression(searchParam.SKU, "i"));
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.FromDate) && !string.IsNullOrWhiteSpace(searchParam.ToDate))
                {
                    var fromDate = Common.ConvertDateTimeToEpoch(DateTime.Parse(searchParam.FromDate));
                    var toDate = Common.ConvertDateTimeToEpoch(DateTime.Parse(searchParam.ToDate).AddDays(1).AddMilliseconds(-1));

                    var filter = Builders<WalmartOrder>.Filter.And(
                            Builders<WalmartOrder>.Filter.Gte("orderDate", fromDate),
                            Builders<WalmartOrder>.Filter.Lte("orderDate", toDate)
                        );
                    Common.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.QgoldOrderId))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                                           Regex("Seller_Order_ID",
                                           new BsonRegularExpression(searchParam.SKU, "i"));
                    Common.AddAndFilter(ref filterQgold, filter);
                };
            }

            var lstQgold = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterQgold)).Data;
            if (filterQgold != null)
            {
                foreach (var item in lstQgold)
                {
                    Common.AddOrFilter(ref filterWalmart, Builders<WalmartOrder>.Filter.Eq(x => x.purchaseOrderId, item.Seller_Order_ID));
                }
            }

            if (searchParam?.IsSubmitted is not null)
            {
                var lstWalmart = (await _walmartDB.Find(CollectionName.WalmartOrder, filterWalmart)).Data;
                var lstSubmit = (await _walmartDB.Find<SubmittedWalmartOrder>(CollectionName.WalmartOrderSubmit)).Data;

                List<WalmartOrderDetailDTO> lstJoin = (from walmart in lstWalmart
                                                       join submit in lstSubmit on walmart.purchaseOrderId equals submit.WalmartOrderId into joinTable
                                                       from lastResult in joinTable.DefaultIfEmpty()
                                                       select new WalmartOrderDetailDTO
                                                       {
                                                           customerOrderId = walmart.customerOrderId,
                                                           purchaseOrderId = walmart.purchaseOrderId,
                                                           BuyerName = walmart.shippingInfo.postalAddress.name,
                                                           Address1 = walmart.shippingInfo.postalAddress.address1,
                                                           Address2 = walmart.shippingInfo.postalAddress.address2,
                                                           City = walmart.shippingInfo.postalAddress.city,
                                                           Country = walmart.shippingInfo.postalAddress.country,
                                                           customerEmailId = walmart.customerEmailId,
                                                           Phone = walmart.shippingInfo.phone,
                                                           estimatedDeliveryDate = Common.ConvertEpochToDateTime(walmart.shippingInfo.estimatedDeliveryDate),
                                                           estimatedShipDate = Common.ConvertEpochToDateTime(walmart.shippingInfo.estimatedShipDate),
                                                           IsSubmitted = !string.IsNullOrEmpty(lastResult?.WalmartOrderId),
                                                           methodShipping = walmart.shippingInfo.methodCode,
                                                           orderDate = Common.ConvertEpochToDateTime(walmart.orderDate),
                                                           PostalCode = walmart.shippingInfo.postalAddress.postalCode,
                                                           QgoldOrderId = null,
                                                           State = walmart.shippingInfo.postalAddress.state,
                                                           CarrierName = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                                                           TrackingNumber = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                                                           Items = CustomMapping.MapItemWalmart(walmart.orderLines!.orderLine)
                                                       }).Where(x => x.IsSubmitted == searchParam.IsSubmitted).OrderByDescending(x => x.orderDate).ToList();
                result = Common.CreatePagination(lstJoin, pageNumber: paginationParam.PageNumber, pageSize: paginationParam.PageSize);

                FilterDefinition<OrderMarketplace>? filterMarket = null;
                foreach (var item in result.Data)
                {
                    Common.AddOrFilter(ref filterMarket, Builders<OrderMarketplace>.Filter.Eq("Seller_Order_ID", item.purchaseOrderId));
                }
                var lstMarket = _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket).Result.Data;
                foreach (var item in result.Data)
                {
                    item.QgoldOrderId = lstMarket.FirstOrDefault(x => x.Seller_Order_ID == item.purchaseOrderId)?.OrderId;
                }
            }
            else
            {
                var sort = Builders<WalmartOrder>.Sort.Combine(
                Builders<WalmartOrder>.Sort.Descending("orderDate")
                );

                var resultTemp = await _walmartDB.Find(
                    CollectionName.WalmartOrder,
                    filterWalmart,
                    page: paginationParam.PageNumber,
                    PageSize: paginationParam.PageSize,
                    sort: sort)
                    ;

                result.PageSize = resultTemp.PageSize;
                result.PageCount = resultTemp.PageCount;
                result.PageNumber = resultTemp.PageNumber;
                result.TotalCount = resultTemp.TotalCount;
                result.Data = CustomMapping.ConvertToWalmartOrderDTO(resultTemp.Data, lstQgold);
                FilterDefinition<SubmittedWalmartOrder>? filterSubmit = null;
                foreach (var item in result.Data)
                {
                    Common.AddOrFilter(ref filterSubmit, Builders<SubmittedWalmartOrder>.Filter.Eq("WalmartOrderId", item.purchaseOrderId));
                }
                var lstSubmit = (await _walmartDB.Find(CollectionName.WalmartOrderSubmit, filterSubmit)).Data;

                foreach (var item in result.Data)
                {
                    item.IsSubmitted = lstSubmit.Any(x => x.WalmartOrderId == item.purchaseOrderId);
                }
            }
            return result;
        }

        public async Task<bool> LastestSellerOrder()
        {
            try
            {
                List<WalmartOrder> orders = new();
                var offset = 100;
                var limit = 100;
                string nextCursor = "*";
                TokenResult tokenResult = await GetTokenFromWalmartSeller();
                OrderWalmartSellerDto? firstData = await GetOrdersWithPaging(0, limit, nextCursor, tokenResult);
                orders.AddRange(firstData!.list!.elements!.order!);
                var totalItems = firstData!.list!.meta!.totalCount!;
                nextCursor = firstData!.list!.meta!.nextCursor!;

                while (offset < totalItems)
                {
                    offset += limit;
                    if (offset > totalItems)
                    {
                        limit = totalItems - (offset - 100);
                    }
                    var item = await GetOrdersWithPaging(0, limit, nextCursor, tokenResult);
                    totalItems = item!.list!.meta!.totalCount!;
                    nextCursor = item!.list!.meta!.nextCursor!;
                    orders.AddRange(item!.list!.elements!.order!);
                }

                if (orders.Any())
                {
                    await CreateUpdateOrderToDb(orders);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<OperationResult> ReSubmit(QgoldFtpOrderObject order)
        {
            try
            {
                order.Marketplace = "Walmart";
                //await _amazonDB.DeleteMany(CollectionName.AmazonOrderSubmit, Builders<OrderAmazonSeller>.Filter.Eq(x => x.AmazonOrderId, order.Amz_Order_ID));
                List<QgoldFtpOrderObject> listAdd = new List<QgoldFtpOrderObject>() { order };
                var checkSubmit = await _submitOrderQgoldService.SubmitQgold(listAdd, true);
                if (checkSubmit)
                {
                    var dataAdd = _mapper.Map<OrderMarketplace>(order);
                    await _qgoldDB.InsertOne(dataAdd, CollectionName.QgoldReOrder);
                    await _walmartDB.InsertOne(new SubmittedWalmartOrder()
                    {
                        _id = ObjectId.GenerateNewId(),
                        SubmittedDate = DateTime.Now,
                        WalmartOrderId  = order.Amz_Order_ID,
                    }, CollectionName.WalmartOrderSubmit);
                    return new OperationResult() { Success = true, Message = "Resubmit done" };
                }
                return new OperationResult() { Success = true, Message = "Re-Submit failed when call await _submitOrderQgoldService.SubmitQgold(model)" };
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"ReSubmit error: {ex.Message}", $"ReSubmit_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult() { Success = false, Message = "System err" };
            }
        }

        private async Task CreateUpdateOrderToDb(List<WalmartOrder> orders)
        {
            List<WalmartOrder> lstOrderLocal = (await _walmartDB.Find<WalmartOrder>(CollectionName.WalmartOrder)).Data;
            List<string> lstPOLocal = lstOrderLocal.Select(x => x.purchaseOrderId).ToList();

            var lstUpdate = orders.Where(x => lstPOLocal.Contains(x.purchaseOrderId)).ToList();
            var updateFilter = Builders<WalmartOrder>.Filter.In(x => x.purchaseOrderId, lstUpdate.Select(x => x.purchaseOrderId));
            foreach (var order in lstUpdate)
            {
                order.PullUpdateDate = DateTime.Now;
                var filter = Builders<WalmartOrder>.Filter.Eq(x => x.purchaseOrderId, order.purchaseOrderId);
                await _walmartDB.ReplaceOne(CollectionName.WalmartOrder, order, filter);
            }

            var lstInsert = orders.Where(x => !lstPOLocal.Contains(x.purchaseOrderId)).ToList();
            lstInsert.ForEach(x => x.PullUpdateDate = DateTime.Now);
            await _walmartDB.InsertMany(lstInsert, CollectionName.WalmartOrder);
        }

        private async Task<OrderWalmartSellerDto?> GetOrdersWithPaging(int offset, int limit, string nextCursor, TokenResult tokenResult, int retry = 0)
        {
            var data = new OrderWalmartSellerDto();
            try
            {

                var baseUrl = _configuration.GetSection("Walmart:OrderURL").Value!;
                var url =
                    $"{baseUrl}?offset={offset}&limit={limit}&nextCursor={nextCursor}&createdStartDate={DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")}";
                if (nextCursor != "*")
                {
                    url = $"{baseUrl}{nextCursor.Replace("limit=100", $"limit={limit}")}";
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
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderWalmartSellerDto>(responseBody);
                    if (data == null)
                    {
                        if (retry < 5)
                        {
                            retry++;
                            await Task.Delay(5000);
                            tokenResult = await GetTokenFromWalmartSeller();
                            data = await GetOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
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
                        tokenResult = await GetTokenFromWalmartSeller();
                        data = await GetOrdersWithPaging(offset, limit, nextCursor, tokenResult, retry);
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

        private async Task<TokenResult> GetTokenFromWalmartSeller()
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
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultAuth>(responseBody);
                    tokenResult.AccessToken = result!.access_token;
                }
            }
            catch
            {
                return null;
            }
            return tokenResult;
        }
    }
}
