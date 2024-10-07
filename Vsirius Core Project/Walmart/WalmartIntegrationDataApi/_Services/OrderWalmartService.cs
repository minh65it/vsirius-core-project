using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.CollectionMongo;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Common.Mappers;
using WalmartIntegrationDataApi.Dtos.Order;
using WalmartIntegrationDataApi.Dtos.Return;
using WalmartIntegrationDataApi.Dtos.Stuller;
using WalmartIntegrationDataApi.Models;
using WalmartIntegrationDataApi.Models.Temp;

namespace WalmartIntegrationDataApi._Services
{
    public class OrderWalmartService : IOrderWalmartService
    {
        private readonly IInteractWalmartSellerCommonService _interactWalmart;
        //private readonly IOrderQgoldRepository _qgoldRepo;
        readonly IOrderWalmartRepository _walmartRep;
        readonly IConfiguration _configuration;
        readonly IMapper _mapper;

        public OrderWalmartService(
            IOrderWalmartRepository walmartRep,
            IConfiguration configuration,
            IMapper mapper,
            //IOrderQgoldRepository qgoldRepo,
            IInteractWalmartSellerCommonService interactWalmart)
        {
            _configuration = configuration;
            _walmartRep = walmartRep;
            _mapper = mapper;
            _interactWalmart = interactWalmart;
            //_qgoldRepo = qgoldRepo;
        }

        public async Task<PaginationUtility<ReturnOrderDTO>?> GetReturnOrders(PaginationParam paginationParam, SearchReturnParam search)
        {
            FilterDefinition<ReturnOrder>? filterReturn = null;

            if (search != null)
            {
                if (search.StatusReturn != null)
                {
                    var filter = Builders<ReturnOrder>.Filter.Eq("returnOrderLines.status", search.StatusReturn.ToString());
                    Util.AddAndFilter(ref filterReturn, filter);
                }
                if (!string.IsNullOrWhiteSpace(search.CustomerName))
                {
                    var filter = Builders<ReturnOrder>.Filter.Or(
                               Builders<ReturnOrder>.Filter.Where(x =>
                               Regex.IsMatch(x.customerName.lastName, search.CustomerName, RegexOptions.IgnoreCase)),
                               Builders<ReturnOrder>.Filter.Where(x =>
                               Regex.IsMatch(x.customerName.firstName, search.CustomerName, RegexOptions.IgnoreCase))
                           );
                    Util.AddAndFilter(ref filterReturn, filter);
                }
                if (!string.IsNullOrWhiteSpace(search.ReturnOrderID))
                {
                    Util.AddAndFilter(ref filterReturn, Builders<ReturnOrder>.Filter.Where(x => x.returnOrderId.Equals(search.ReturnOrderID)));
                }
                if (!string.IsNullOrWhiteSpace(search.PurchaseOrderID))
                {
                    Util.AddAndFilter(ref filterReturn, Builders<ReturnOrder>.Filter.Eq("returnOrderLines.purchaseOrderId", search.PurchaseOrderID));
                }
                if (!string.IsNullOrWhiteSpace(search.SKU))
                {
                    Util.AddAndFilter(ref filterReturn, Builders<ReturnOrder>.Filter.Eq("returnOrderLines.item.sku", search.SKU));
                }
                if (!string.IsNullOrWhiteSpace(search.FromDate) && !string.IsNullOrWhiteSpace(search.ToDate))
                {
                    var fromDate = DateTime.Parse(search.FromDate);
                    var toDate = DateTime.Parse(search.ToDate).AddDays(1).AddSeconds(-1);
                    var filter = Builders<ReturnOrder>.Filter.And(
                            Builders<ReturnOrder>.Filter.Lte(x => x.returnOrderDate, toDate),
                            Builders<ReturnOrder>.Filter.Gte(x => x.returnOrderDate, fromDate)
                        );
                    Util.AddAndFilter(ref filterReturn, filter);
                }
            }
            var sort = Builders<ReturnOrder>.Sort.Descending(x => x.returnOrderDate);
            var rsLocal = await _walmartRep.Find(CollectionName.ReturnOrderWalmart,
                filterReturn, paginationParam.PageNumber, paginationParam.PageSize, sort);


            PaginationUtility<ReturnOrderDTO>? rs = new(
                Result: _mapper.Map<List<ReturnOrderDTO>>(rsLocal.Result),
                Pagination: new PaginationUtility<ReturnOrderDTO>.PaginationResult()
                {
                    PageNumber = paginationParam.PageNumber,
                    PageSize = paginationParam.PageSize,
                    Skip = rsLocal.Pagination.Skip,
                    TotalCount = rsLocal.Pagination.TotalCount,
                    TotalPage = rsLocal.Pagination.TotalPage
                });

            var lstRMA = rs.Result.Select(x => x.returnOrderId).ToList();

            List<ClaimDTO> claimDTOs = await GetAllClaimByRMA(lstRMA);

            foreach (var item in rs.Result)
            {
                item.claim = claimDTOs.FirstOrDefault(x => x.RMA == item.returnOrderId);
            }

            return rs;
        }

        private async Task<List<ClaimDTO>> GetAllClaimByRMA(List<string> lstRMA)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("rma", lstRMA);
                var url = _configuration.GetValue<string>("PackageAPI:GetAllClaim");
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ClaimDTO>>(content)!;
                }
                else
                {
                    return new List<ClaimDTO>();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog("GetAllClaimByRMA failed exc: " + ex.Message);
                return new List<ClaimDTO>();
            }
        }

        public async Task<ReturnOrderDTO?> GetDetailReturnOrder(string id)
        {
            var detail = _mapper.Map<ReturnOrderDTO>(await _walmartRep.FisrtOrDefault(CollectionName.ReturnOrderWalmart,
                Builders<ReturnOrder>.Filter.Where(x => x.returnOrderId.Equals(id))));
            if (detail != null)
            {
                var claim = await GetDetailClaim(detail.returnOrderId);
                detail.claim = claim;
            }
            return detail;
        }

        //Exten function
        private async Task<ClaimDTO> GetDetailClaim(string id)
        {
            ClaimDTO? result = new ClaimDTO() { };
            string token = await LoginIdentity();
            if (token == null)
                return result;

            // call api package
            //string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcwNjk1Mjk4MiwiaXNzIjoiaHR0cHM6Ly92c2lyaXVzLmNvbS8iLCJhdWQiOiJodHRwczovL3ZzaXJpdXMuY29tLyJ9.oHIjDQHMzPjY4hqkctjCwY_TIsxA_OIeMPdjXrqQi-o";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            httpClient.Timeout = TimeSpan.FromHours(1);
            var url = $"{_configuration.GetValue<string>("PackageAPI:URL")}?id={id}";
            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent != null)
                result = JsonConvert.DeserializeObject<ClaimDTO>(responseContent);
            return result;
        }

        private async Task<string> LoginIdentity()
        {
            string urlLogin = _configuration.GetValue<string>("IdentityApi:login")!;
            string email = _configuration.GetValue<string>("IdentityApi:user")!;
            string password = _configuration.GetValue<string>("IdentityApi:PassWord")!;

            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "email", email },
                    { "password", password }
                };

                // Content post
                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send request to urlLogin
                var response = await httpClient.PostAsync(urlLogin, content);

                // Get token from urlLogin
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic jsonRs = JsonConvert.DeserializeObject(result);
                    return jsonRs.token;
                }
                return null;
            }
        }

        public async Task<PaginationUtility<OrderDetailDTO>> GetOrders(PaginationParam paginationParam, WalmartOrderSearchParam searchParam)
        {
            FilterDefinition<Order>? filterWalmart = null;

            if (searchParam != null)
            {
                if (searchParam.OrderStatus is not null)
                {
                    filterWalmart = Builders<Order>.Filter
                                .Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", searchParam.OrderStatus.ToString());
                };
                if (!string.IsNullOrWhiteSpace(searchParam.Supplier))
                {
                    if (searchParam.Supplier.Equals("Stuller", StringComparison.OrdinalIgnoreCase))
                    {
                        Util.AddAndFilter(ref filterWalmart,
                            Builders<Order>.Filter.Regex("orderLines.orderLine.item.sku", new BsonRegularExpression("^S", "i"))
                            );
                    }
                    if (searchParam.Supplier.Equals("Qgold", StringComparison.OrdinalIgnoreCase))
                    {
                        Util.AddAndFilter(ref filterWalmart,
                            Builders<Order>.Filter.Regex("orderLines.orderLine.item.sku", new BsonRegularExpression("^Q", "i"))
                            );
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchParam.TrackingNumber))
                {
                    var filter = Builders<Order>.Filter.
                        Regex("orderLines.orderLine.orderLineStatuses.orderLineStatus.trackingInfo.trackingNumber",
                        new BsonRegularExpression(searchParam.TrackingNumber, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.Address1))
                {
                    var filter = Builders<Order>.Filter.
                            Regex("shippingInfo.postalAddress.address1",
                            new BsonRegularExpression(searchParam.Address1, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.BuyerName))
                {
                    var filter = Builders<Order>.Filter.
                               Regex("shippingInfo.postalAddress.name",
                               new BsonRegularExpression(searchParam.BuyerName, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.purchaseOrderId))
                {
                    var filter = Builders<Order>.Filter.
                               Regex("purchaseOrderId",
                               new BsonRegularExpression(searchParam.purchaseOrderId, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.customerOrderId))
                {
                    var filter = Builders<Order>.Filter.
                                   Regex("customerOrderId",
                                   new BsonRegularExpression(searchParam.customerOrderId, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.SKU))
                {
                    var filter = Builders<Order>.Filter.
                                       Regex("orderLines.orderLine.item.sku",
                                       new BsonRegularExpression(searchParam.SKU, "i"));
                    Util.AddAndFilter(ref filterWalmart, filter);
                };
                if (!string.IsNullOrWhiteSpace(searchParam.FromDate) && !string.IsNullOrWhiteSpace(searchParam.ToDate))
                {
                    var fromDate = Util.ConvertDateTimeToEpochUTC(DateTime.Parse(searchParam.FromDate));
                    var toDate = Util.ConvertDateTimeToEpochUTC(DateTime.Parse(searchParam.ToDate).AddDays(1).AddMilliseconds(-1));

                    var filter = Builders<Order>.Filter.And(
                            Builders<Order>.Filter.Gte("orderDate", fromDate),
                            Builders<Order>.Filter.Lte("orderDate", toDate)
                        );
                    Util.AddAndFilter(ref filterWalmart, filter);
                };

            }
            var lstQgold = await GetAllQgoldMarketplace();
            var lstStuller = await GetAllStullerOrder(searchParam?.FromDate, searchParam?.ToDate, searchParam?.SupplierOrderId);

            if (!string.IsNullOrWhiteSpace(searchParam?.SupplierOrderId))
            {
                var itemQgold = lstQgold.Where(x => x.OrderId != null && x.OrderId.Contains(searchParam.SupplierOrderId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (itemQgold != null)
                    Util.AddAndFilter(ref filterWalmart, Builders<Order>.Filter.Eq(x => x.purchaseOrderId, itemQgold.Seller_Order_ID));

                var itemStuller = lstStuller.Where(x => x.orderID != null && x.orderID.Contains(searchParam.SupplierOrderId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (itemStuller != null)
                    Util.AddAndFilter(ref filterWalmart, Builders<Order>.Filter.Eq(x => x.purchaseOrderId, itemStuller.marketplaceOrderId));
            };

            if (searchParam?.IsSubmitted is not null)
            {
                var lstWalmart = await _walmartRep.FindAll(CollectionName.OrderWalmart, filterWalmart);
                var lstSubmit = await _walmartRep.FindAll<SubmittedWalmartOrder>(CollectionName.SubmittedOrderWalmart);

                List<OrderDetailDTO> lstJoin = (from walmart in lstWalmart
                                                join submit in lstSubmit on walmart.purchaseOrderId equals submit.WalmartOrderId into joinTable
                                                from lastResult in joinTable.DefaultIfEmpty()
                                                let providerOrderId = lstQgold.FirstOrDefault(x => x.Seller_Order_ID == walmart.purchaseOrderId)?.OrderId
                                                ?? lstStuller.FirstOrDefault(x => x.marketplaceOrderId == walmart.purchaseOrderId)?.orderID
                                                let providerName = Util.GetProviderName(walmart.orderLines.orderLine[0].item.sku)
                                                select new OrderDetailDTO
                                                {
                                                    customerOrderId = walmart.customerOrderId,
                                                    purchaseOrderId = walmart.purchaseOrderId,
                                                    SupplierOrderId = providerOrderId,
                                                    Supplier = providerName,
                                                    BuyerName = walmart.shippingInfo.postalAddress.name,
                                                    Address1 = walmart.shippingInfo.postalAddress.address1,
                                                    Address2 = walmart.shippingInfo.postalAddress.address2,
                                                    City = walmart.shippingInfo.postalAddress.city,
                                                    Country = walmart.shippingInfo.postalAddress.country,
                                                    customerEmailId = walmart.customerEmailId,
                                                    Phone = walmart.shippingInfo.phone,
                                                    estimatedDeliveryDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedDeliveryDate),
                                                    estimatedShipDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedShipDate),
                                                    IsSubmitted = !string.IsNullOrEmpty(lastResult?.WalmartOrderId)
                                                        && !string.IsNullOrEmpty(walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber)
                                                        && !string.IsNullOrEmpty(providerOrderId),
                                                    methodShipping = walmart.shippingInfo.methodCode,
                                                    orderDate = Util.ConvertEpochUTCToDateTime(walmart.orderDate),
                                                    PostalCode = walmart.shippingInfo.postalAddress.postalCode,
                                                    State = walmart.shippingInfo.postalAddress.state,
                                                    CarrierName = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                                                    TrackingNumber = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                                                    Items = CustomMapping.MapItemWalmart(walmart.orderLines!.orderLine)
                                                }).Where(x => x.IsSubmitted == searchParam.IsSubmitted).OrderByDescending(x => x.orderDate).ToList();
                var result = PaginationUtility<OrderDetailDTO>.Create(lstJoin, pageNumber: paginationParam.PageNumber, pageSize: paginationParam.PageSize);

                return result;
            }
            else
            {
                var sort = Builders<Order>.Sort.Combine(
                Builders<Order>.Sort.Descending("orderDate")
                );

                var resultTemp = await _walmartRep.Find(
                    CollectionName.OrderWalmart,
                    filterWalmart,
                    page: paginationParam.PageNumber,
                    PageSize: paginationParam.PageSize,
                    sort: sort)
                    ;

                var result = new PaginationUtility<OrderDetailDTO>(
                    Pagination: new PaginationUtility<OrderDetailDTO>.PaginationResult()
                    {
                        PageNumber = resultTemp.Pagination.PageNumber,
                        PageSize = resultTemp.Pagination.PageSize,
                        TotalCount = resultTemp.Pagination.TotalCount,
                        Skip = resultTemp.Pagination.Skip,
                        TotalPage = resultTemp.Pagination.TotalPage
                    },
                    Result: CustomMapping.ConvertToWalmartOrderDTO(resultTemp.Result, lstQgold, lstStuller)
                    );

                var lstWalmartOrderID = result.Result.Select(x => x.purchaseOrderId).ToList();
                var filterSubmit = Builders<SubmittedWalmartOrder>.Filter.Where(x => lstWalmartOrderID.Contains(x.WalmartOrderId));
                var lstSubmit = await _walmartRep.FindAll(CollectionName.SubmittedOrderWalmart, filterSubmit);

                foreach (var item in result.Result)
                {
                    item.IsSubmitted = lstSubmit.Any(x => x.WalmartOrderId == item.purchaseOrderId);
                }
                return result;
            }
        }

        private async Task<List<StullerOrderDto>> GetAllStullerOrder(string? fromDate, string? toDate, string? marketplaceOrderId)
        {
            var result = new List<StullerOrderDto>();
            int pageSize = 3000;
            int pageNumber = 1;
            var firstData = await GetPaginationStullerFromAPI(fromDate, toDate, marketplaceOrderId, pageSize, pageNumber);
            result.AddRange(firstData.Result);
            for (int i = 2; i <= firstData.Pagination.TotalPage; i++)
            {
                var paginationRs = await GetPaginationStullerFromAPI(fromDate, toDate, marketplaceOrderId, pageSize, i);
                result.AddRange(paginationRs.Result);
            }
            return result;
        }

        private async Task<PaginationUtility<StullerOrderDto>> GetPaginationStullerFromAPI(string? fromDate, string? toDate, string? marketplaceOrderId, int pageSize, int pageNumber)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration.GetValue<string>("StullerAPI:GetOrders")}?PageSize={pageSize}&PageNumber={pageNumber}&FromDate={fromDate}&ToDate={toDate}&marketplaceOrderId={marketplaceOrderId}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PaginationUtility<StullerOrderDto>>(content)!;
                }
                else
                {
                    return new PaginationUtility<StullerOrderDto>();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog("GetPaginationStullerFromAPI failed exc: " + ex.Message);
                return new PaginationUtility<StullerOrderDto>();
            }
        }

        private async Task<List<OrderMarketplace>> GetAllQgoldMarketplace()
        {
            var result = new List<OrderMarketplace>();
            int pageSize = 3000;
            int pageNumber = 1;
            var firstData = await GetPaginationQgoldMarketplace(pageSize, pageNumber);
            result.AddRange(firstData.Data);
            for (int i = 2; i <= firstData.PageCount; i++)
            {
                var paginationRs = await GetPaginationQgoldMarketplace(pageSize, i);
                result.AddRange(paginationRs.Data);
            }
            return result;
        }

        private async Task<PaginationUtil1<OrderMarketplace>> GetPaginationQgoldMarketplace(int pageSize, int pageNumber)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["OrderProcessAPI:GetMarketplaceOrder"]}?PageSize={pageSize}&PageNumber={pageNumber}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PaginationUtil1<OrderMarketplace>>(content)!;
                }
                else
                {
                    return new PaginationUtil1<OrderMarketplace>();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog("GetAllClaimByRMA failed exc: " + ex.Message);
                return new PaginationUtil1<OrderMarketplace>();
            }
        }

        private async Task<OrderMarketplace> GetDetailOrderMarketplaceBySellerOrderId(string id)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration.GetValue<string>("OrderProcessAPI:DetailOrderBySellerId")}?orderSellerId={id}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<OrderMarketplace>(content)!;
                }
                else
                {
                    return new OrderMarketplace();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog("GetAllClaimByRMA failed exc: " + ex.Message);
                return new OrderMarketplace();
            }
        }

        public async Task<OrderDetailDTO?> GetDetailOrder(string id)
        {
            OrderDetailDTO? result = null;

            var walmart = await _walmartRep.FisrtOrDefault(CollectionName.OrderWalmart,
                 Builders<Order>.Filter.Eq(x => x.purchaseOrderId, id));
            if (walmart != null)
            {
                var submit = await _walmartRep.FisrtOrDefault(CollectionName.SubmittedOrderWalmart,
                    Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, id));

                var market = await GetDetailOrderMarketplaceBySellerOrderId(id);
                result = new()
                {
                    customerOrderId = walmart.customerOrderId,
                    purchaseOrderId = walmart.purchaseOrderId,
                    SupplierOrderId = market?.OrderId,
                    Supplier = Util.GetProviderName(walmart.orderLines.orderLine[0].item.sku),
                    BuyerName = walmart.shippingInfo.postalAddress.name,
                    Address1 = walmart.shippingInfo.postalAddress.address1,
                    Address2 = walmart.shippingInfo.postalAddress.address2,
                    City = walmart.shippingInfo.postalAddress.city,
                    Country = walmart.shippingInfo.postalAddress.country,
                    customerEmailId = walmart.customerEmailId,
                    Phone = walmart.shippingInfo.phone,
                    estimatedDeliveryDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedDeliveryDate),
                    estimatedShipDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedShipDate),
                    IsSubmitted = !string.IsNullOrEmpty(submit?.WalmartOrderId),
                    methodShipping = walmart.shippingInfo.methodCode,
                    orderDate = Util.ConvertEpochUTCToDateTime(walmart.orderDate),
                    PostalCode = walmart.shippingInfo.postalAddress.postalCode,
                    State = walmart.shippingInfo.postalAddress.state,
                    CarrierName = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                    TrackingNumber = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                    Items = CustomMapping.MapItemWalmart(walmart.orderLines!.orderLine)
                };

            }
            return result;
        }

        public async Task<OperationResult> DeleteOrders(List<string> ds)
        {
            var pointDateCanDelete = DateTime.UtcNow.AddMonths(-8);

            FilterDefinition<Order>? filterWalmart = null;
            FilterDefinition<SubmittedWalmartOrder>? filterSubmit = null;
            FilterDefinition<OrderMarketplace>? filterMarket = null;

            foreach (var item in ds)
            {
                Util.AddOrFilter(ref filterWalmart, Builders<Order>.Filter.Eq(x => x.purchaseOrderId, item));
                Util.AddOrFilter(ref filterMarket, Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, item));
                Util.AddOrFilter(ref filterSubmit, Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, item));
            }

            var lstSubmit = (await _walmartRep.FindAll(CollectionName.SubmittedOrderWalmart, filterSubmit));
            var lstWalmart = (await _walmartRep.FindAll(CollectionName.OrderWalmart, filterWalmart));

            var submitErr = new StringBuilder();
            foreach (var item in lstSubmit)
            {
                if (item.SubmittedDate > pointDateCanDelete)
                {
                    submitErr.Append($"{item.WalmartOrderId}, ");
                }
                else
                {
                    await _walmartRep.DeleteMany(CollectionName.SubmittedOrderWalmart,
                        Builders<SubmittedWalmartOrder>.Filter.Eq(x => x.WalmartOrderId, item.WalmartOrderId));
                }
            }

            var walmartErr = new StringBuilder();
            foreach (var item in lstWalmart)
            {
                if (item.orderDate > Util.ConvertDateTimeToEpoch(pointDateCanDelete))
                {
                    walmartErr.Append($"{item.purchaseOrderId}, ");
                }
                else
                {
                    await _walmartRep.DeleteMany(CollectionName.OrderWalmart,
                        Builders<Order>.Filter.Eq(x => x.purchaseOrderId, item.purchaseOrderId));
                }
            }

            var msg = $"{(walmartErr.Length > 0 ? walmartErr + "in Order Walmart delete failed.\n" : "")}" +
                $"{(submitErr.Length > 0 ? submitErr + "in SubmittedOrder Walmart delete failed.\n" : "")}"
                ;

            Util.WriteLog("Delete Mutiple Walmart Err:" + msg);

            if (msg.Trim().Length > 0)
            {
                return new OperationResult() { IsSuccess = false, Message = "System Err" };
            }
            return new OperationResult() { IsSuccess = true };
        }

        public async Task<List<UnshipOrderDto>> GetUnshippedOrderIds(UnshippedOrderIdsParam param)
        {
            //FilterDefinition<Order>? filter = Builders<Order>.Filter
            //    .Or(
            //    Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Acknowledged"),
            //    Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Created")
            //    );

            FilterDefinition<Order>? filter = null;

            if (param != null)
            {
                if (!string.IsNullOrWhiteSpace(param.Provider))
                {
                    switch (param.Provider)
                    {
                        case "Stuller":
                            Util.AddAndFilter(ref filter,
                            Builders<Order>.Filter.Regex("orderLines.orderLine.item.sku", "^S"));
                            break;
                        case "Qgold":
                            Util.AddAndFilter(ref filter,
                            Builders<Order>.Filter.Regex("orderLines.orderLine.item.sku", "^Q"));
                            break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(param.FromDate))
                {
                    var fromDate = Util.ConvertDateTimeToEpochUTC(param.FromDate);
                    Util.AddAndFilter(ref filter, Builders<Order>.Filter.Gt(x => x.orderDate, fromDate));
                }
                if (!string.IsNullOrWhiteSpace(param.ToDate))
                {
                    var toDate = Util.ConvertDateTimeToEpochUTC(DateTime.Parse(param.ToDate).AddDays(1).AddMilliseconds(-1));
                    Util.AddAndFilter(ref filter, Builders<Order>.Filter.Lt(x => x.orderDate, toDate));
                }
            }

            var lstOrder = (await _walmartRep.FindAll(CollectionName.OrderWalmart, filter))
                .GroupBy(
                x => x.customerOrderId,
                x => x.orderLines.orderLine,
                (key, g) => new UnshipOrderDto
                {
                    CustomerOrderId = key,
                    ItemUnShip = ConvertOrderLineToItemUnShip(g)
                }).ToList();

            return lstOrder;
        }

        private List<ItemUnShip> ConvertOrderLineToItemUnShip(IEnumerable<List<OrderLine>> lstGroupCustomerOrderId)
        {
            //lstGroupCustomerOrderId: 1 cusPO thi se co nhieu order line
            List<ItemUnShip> rs = new();

            foreach (var item in lstGroupCustomerOrderId)
            {
                var obj = new ItemUnShip()
                {
                    Sku = item.First().item.sku,
                    Quantity = item.Count().ToString()
                };

                rs.Add(obj);
            }

            return rs;
        }
    }
}
