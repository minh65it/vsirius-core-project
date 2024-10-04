using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using MongoDB.Driver;
using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AutoMapper;
using MongoDB.Bson;
using Korzh.EasyQuery;
using System.Xml.Linq;
using System.Drawing;
using System.Linq;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class OrderAmazonProcessor : IOrderAmazonProcessor
    {
        private readonly IQgoldClientApi _ggoldClientApi;
        private readonly IStullerClientApi _stullerClientApi;
        private readonly IAmazonMongoRepository _amazonMongoRepository;
        private readonly IConfiguration _config;
        private readonly IQgoldApiClient _qgoldApiClient;
        private readonly IMapper _mapper;
        private string amazonOrder = "";
        private string amazonOrder1 = "";
        private string amazonOrderUI = "";
        private string amazonOrderSubmit = "";
        private string amazonOrderTracking = "";
        private readonly bool _isProduction = false;

        public OrderAmazonProcessor(IQgoldClientApi ggoldClientApi,
            IAmazonMongoRepository amazonMongoRepository,
            IConfiguration config,
            IQgoldApiClient qgoldApiClient,
            IMapper mapper,
            IStullerClientApi stullerClientApi)
        {
            _ggoldClientApi = ggoldClientApi;
            _amazonMongoRepository = amazonMongoRepository;
            _qgoldApiClient = qgoldApiClient;
            _stullerClientApi = stullerClientApi;
            _config = config;
            _mapper = mapper;
            amazonOrder = _config.GetSection("AmazonLocal:orderCollection").Value!;
            amazonOrder1 = _config.GetSection("AmazonLocal:orderCollection1").Value!;
            amazonOrderUI = _config.GetSection("AmazonLocal:orderCollection2").Value!;
            amazonOrderSubmit = _config.GetSection("AmazonLocal:orderSubmittedCollection").Value!;
            amazonOrderTracking = _config.GetSection("AmazonLocal:orderTrackingCollection").Value!;
            _isProduction = _config.GetSection("FikaAmazonAPI:Environment").Value == "Production" ? true : false;
        }

        public Task<OperationResult> DeleteOrders(List<string> ds)
        {
            throw new NotImplementedException();
        }

        public async Task<AmazonOrderDetailDTO?> GetDetailOrder(string sellerOrderId)
        {
            AmazonOrderDetailDTO? result = null;
            var orderAmazon = await _amazonMongoRepository.FisrtOrDefault(amazonOrder, Builders<OrderAmazon>.Filter.Eq(x => x.AmazonOrderId, sellerOrderId));
            var orderSubmitAmazon = await _amazonMongoRepository.FisrtOrDefault(amazonOrderSubmit, Builders<OrderAmazonSubmitted>.Filter.Eq(x => x.Amz_Order_ID, sellerOrderId));

            var orderQgold = await _ggoldClientApi.GetDetailOrderBySellerMarketplace(sellerOrderId);
            var orderStuller = await _stullerClientApi.GetDetailOrderByIdMarketplace(sellerOrderId);
            string? SupplierOrderId = null;
            string? Supplier = null;
            if(orderQgold?.OrderId != null)
            {
                SupplierOrderId = orderQgold.OrderId;
                Supplier = "Qgold";
            }
            else if(orderStuller?.OrderID != null)
            {
                SupplierOrderId = orderStuller.OrderID;
                Supplier = "Stuller";
            }
            else
            {

            }

            if (orderAmazon is not null)
            {

                result = new()
                {
                    AmazonOrderId = orderAmazon.AmazonOrderId,
                    SupplierOrderId = SupplierOrderId,
                    Supplier = Supplier,
                    BuyerEmail = orderAmazon.BuyerInfo.BuyerEmail,
                    BuyerName = orderAmazon.ShippingAddress.Name,
                    AddressLine1 = orderAmazon.ShippingAddress.AddressLine1,
                    AddressLine2 = orderAmazon.ShippingAddress.AddressLine2,
                    AddressLine3 = orderAmazon.ShippingAddress.AddressLine3,
                    City = orderAmazon.ShippingAddress.City,
                    CountryCode = orderAmazon.ShippingAddress.CountryCode,
                    EarliestDeliveryDate = orderAmazon.EarliestDeliveryDate,
                    EarliestShipDate = orderAmazon.EarliestShipDate,
                    FulfillmentChannel = orderAmazon.FulfillmentChannel,
                    LatestDeliveryDate = orderAmazon.LatestDeliveryDate,
                    LatestShipDate = orderAmazon.LatestShipDate,
                    MarketplaceId = orderAmazon.MarketplaceId,
                    OrderStatus = orderAmazon.OrderStatus,
                    OrderType = orderAmazon.OrderType,
                    PaymentMethod = orderAmazon.PaymentMethod,
                    Phone = orderAmazon.ShippingAddress.Phone,
                    PurchaseDate = orderAmazon.PurchaseDate,
                    SalesChannel = orderAmazon.SalesChannel,
                    ShipmentServiceLevelCategory = orderAmazon.ShipmentServiceLevelCategory,
                    ShipServiceLevel = orderAmazon.ShipServiceLevel,
                    StateOrRegion = orderAmazon.ShippingAddress.StateOrRegion,
                    Total = orderAmazon.OrderTotal.Amount + orderAmazon.OrderTotal.CurrencyCode,
                    PostalCode = orderAmazon.ShippingAddress.PostalCode,
                    IsSubmitted = string.IsNullOrWhiteSpace(orderSubmitAmazon?.Amz_Order_ID),
                    OrderItemList = CustomMappingOrder.MapItemOrderAmazon(orderAmazon.OrderItemList),
                };
            }
            else if (orderSubmitAmazon is not null)
            {

                result = new()
                {
                    AmazonOrderId = orderSubmitAmazon.Amz_Order_ID,
                    SupplierOrderId = SupplierOrderId,
                    Supplier = Supplier,
                    BuyerEmail = null,
                    BuyerName = orderSubmitAmazon.ShipToName,
                    AddressLine1 = orderSubmitAmazon.ShipToAddress1,
                    AddressLine2 = orderSubmitAmazon.ShipToAddress2,
                    AddressLine3 = orderSubmitAmazon.ShipToAddress3,
                    City = orderSubmitAmazon.ShipToCity,
                    CountryCode = orderSubmitAmazon.ShipToCountryCode,
                    EarliestDeliveryDate = null,
                    EarliestShipDate = null,
                    FulfillmentChannel = null,
                    LatestDeliveryDate = null,
                    LatestShipDate = null,
                    MarketplaceId = null,
                    OrderStatus = null,
                    OrderType = "Shipped",
                    PaymentMethod = orderSubmitAmazon.Ship_Method_Code,
                    Phone = orderSubmitAmazon.ShipToPhone,
                    PurchaseDate = orderSubmitAmazon.TimeStamp,
                    SalesChannel = null,
                    ShipmentServiceLevelCategory = null,
                    ShipServiceLevel = orderSubmitAmazon.Ship_Method_Code,
                    StateOrRegion = orderSubmitAmazon.ShipToState_Province,
                    Total = null,
                    PostalCode = orderSubmitAmazon.ShipToZip,
                    IsSubmitted = true,
                    OrderItemList = CustomMappingOrder.MapItemOrderAmazon1(orderSubmitAmazon.Item),
                };
            }

            return result;
        }

        public async Task<PaginationResult<AmazonOrderDto>> GetOrders(Helpers.MongoDB.PaginationParam paginationParam, AmazonProcessSearchParam searchParam)
        {

            var result = new PaginationResult<AmazonOrderDto>();

            FilterDefinition<OrderAmazon>? filterOrder = null;
            FilterDefinition<OrderAmazonSubmitted>? filterSubmitAmazon = null;
            if (searchParam != null)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.AddressLine1))
                {
                    Utilities2.AddAndFilter(ref filterOrder,
                        Builders<OrderAmazon>.Filter.Regex(x => x.ShippingAddress.AddressLine1,
                        new BsonRegularExpression(searchParam.AddressLine1, "i")));
                    Utilities2.AddAndFilter(ref filterSubmitAmazon,
                        Builders<OrderAmazonSubmitted>.Filter.Regex(x => x.ShipToAddress1,
                        new BsonRegularExpression(searchParam.AddressLine1, "i")));

                }
                if (!string.IsNullOrWhiteSpace(searchParam.BuyerName))
                {
                    Utilities2.AddAndFilter(ref filterOrder, Builders<OrderAmazon>.Filter.Regex(x => x.ShippingAddress.Name, new BsonRegularExpression(searchParam.BuyerName, "i")));
                    Utilities2.AddAndFilter(ref filterSubmitAmazon, Builders<OrderAmazonSubmitted>.Filter.Regex(x => x.ShipToName, new BsonRegularExpression(searchParam.BuyerName, "i")));

                }
                if (!string.IsNullOrWhiteSpace(searchParam.AmazonOrderId))
                {
                    Utilities2.AddAndFilter(ref filterOrder, Builders<OrderAmazon>.Filter.Regex(x => x.AmazonOrderId, new BsonRegularExpression(searchParam.AmazonOrderId, "i")));
                    Utilities2.AddAndFilter(ref filterSubmitAmazon, Builders<OrderAmazonSubmitted>.Filter.Regex(x => x.Amz_Order_ID, new BsonRegularExpression(searchParam.AmazonOrderId, "i")));

                }
                if (!string.IsNullOrWhiteSpace(searchParam.SKU))
                {
                    Utilities2.AddAndFilter(ref filterOrder, Builders<OrderAmazon>.Filter.ElemMatch(x => x.OrderItemList, Builders<OrderItemList>.Filter.Regex(x => x.SellerSKU, new BsonRegularExpression(searchParam.SKU, "i"))));
                    Utilities2.AddAndFilter(ref filterSubmitAmazon, Builders<OrderAmazonSubmitted>.Filter.ElemMatch(x => x.Item, Builders<ItemSubmitAmazon>.Filter.Regex(x => x.Item, new BsonRegularExpression(searchParam.SKU, "i"))));

                }
                if (!string.IsNullOrWhiteSpace(searchParam.FromDate) && !string.IsNullOrWhiteSpace(searchParam.ToDate))
                {
                    var fromDate = DateTime.Parse(searchParam.FromDate);
                    var toDate = DateTime.Parse(searchParam.ToDate).AddDays(1).AddMilliseconds(-1);

                    Utilities2.AddAndFilter(ref filterOrder, Builders<OrderAmazon>.Filter.Gte(x => x.PurchaseDate, fromDate));
                    Utilities2.AddAndFilter(ref filterOrder, Builders<OrderAmazon>.Filter.Lte(x => x.PurchaseDate, toDate));

                    Utilities2.AddAndFilter(ref filterSubmitAmazon, Builders<OrderAmazonSubmitted>.Filter.Gte(x => x.TimeStamp, fromDate));
                    Utilities2.AddAndFilter(ref filterSubmitAmazon, Builders<OrderAmazonSubmitted>.Filter.Lte(x => x.TimeStamp, toDate));
                }

                if (searchParam?.IsSubmitted is not null)
                {
                }
            }

            List<OrderAmazon> lstAmazon = (await _amazonMongoRepository.Find(amazonOrder,
                    filter: filterOrder)).Data;
            List<OrderAmazonSubmitted> lstSubmit = (await _amazonMongoRepository.Find(amazonOrderSubmit,
                filter: filterSubmitAmazon)).Data;

            //join result
            var resultOrderNotSubmit = CustomMappingOrder.ConvertToAmazonOrderDTONotSubmit(lstAmazon);
            var resultOrderUnShipped = CustomMappingOrder.ConvertToAmazonOrderDTOUnShipped(lstAmazon, lstSubmit);
            var joinResult = CustomMappingOrder.ConvertToAmazonOrderDTOShipped(lstSubmit);
            joinResult = joinResult.OrderByDescending(x => x.PurchaseDate).ToList();

            if (joinResult.Any())
            {
                //UnSubmit
                var skuExcept = resultOrderNotSubmit.Select(x => x.AmazonOrderId).Except(joinResult.Select(x => x.AmazonOrderId)).ToList();
                resultOrderNotSubmit = resultOrderNotSubmit.Where(x => skuExcept.Contains(x.AmazonOrderId)).ToList();
                //UnShipped
                var skuIntersect = resultOrderUnShipped.Select(x => x.AmazonOrderId).Intersect(joinResult.Select(x => x.AmazonOrderId)).ToList();
                resultOrderUnShipped = resultOrderUnShipped.Where(x => skuIntersect.Contains(x.AmazonOrderId)).ToList();
                //Shipped
                joinResult = joinResult.Where(x => !skuIntersect.Contains(x.AmazonOrderId)).ToList();

                OrderMarketplaceParams searchMarket = new OrderMarketplaceParams() { };
                if (!string.IsNullOrWhiteSpace(searchParam.Supplier))
                {
                    if (searchParam.Supplier == "Qgold")
                    {
                        var lstMap = await _ggoldClientApi.GetOrderMarketplace(searchMarket);
                        lstMap = lstMap?.Where(x => joinResult.Select(x => x.AmazonOrderId).Contains(x.Seller_Order_ID)).ToList();
                        joinResult = lstMap != null ? CustomMappingOrder.MapQgoldToAmazon(joinResult, lstMap) : new List<AmazonOrderDto>() { };
                        joinResult.AddRange(resultOrderNotSubmit.Where(x => x.Supplier == "Qgold").ToList());
                        joinResult.AddRange(resultOrderUnShipped.Where(x => x.Supplier == "Qgold").ToList());
                    }
                    else if (searchParam.Supplier == "Stuller")
                    {
                        var lstMap = await _stullerClientApi.GetOrderMarketplace(searchParam);
                        lstMap = lstMap?.Where(x => joinResult.Select(x => x.AmazonOrderId).Contains(x.MarketplaceOrderId)).ToList();
                        joinResult = lstMap != null ? CustomMappingStullerOrder.MapStullerToAmazon(joinResult, lstMap) : new List<AmazonOrderDto>() { };
                        joinResult.AddRange(resultOrderNotSubmit.Where(x => x.Supplier == "Stuller").ToList());
                        joinResult.AddRange(resultOrderUnShipped.Where(x => x.Supplier == "Stuller").ToList());
                    }
                    else
                    {
                        joinResult = new List<AmazonOrderDto> { };
                    }
                }
                else
                {
                    //Join
                    var lstMapQgold = await _ggoldClientApi.GetOrderMarketplace(searchMarket);
                    lstMapQgold = lstMapQgold?.Where(x => joinResult.Select(x => x.AmazonOrderId).Contains(x.Seller_Order_ID)).ToList();
                    var dataQgold = lstMapQgold != null ? CustomMappingOrder.MapQgoldToAmazon(joinResult, lstMapQgold) : joinResult;
                    //No data in Qgold
                    var skunotSupplierQgold = joinResult.Select(x => x.AmazonOrderId).Except(dataQgold.Select(x => x.AmazonOrderId)).ToList();
                    joinResult = joinResult.Where(x => skunotSupplierQgold.Contains(x.AmazonOrderId)).ToList();

                    //Join
                    var lstMapStuller = await _stullerClientApi.GetOrderMarketplace(searchParam);
                    lstMapStuller = lstMapStuller?.Where(x => joinResult.Select(x => x.AmazonOrderId).Contains(x.MarketplaceOrderId)).ToList();
                    var dataStuller = lstMapStuller != null ? CustomMappingStullerOrder.MapStullerToAmazon(joinResult, lstMapStuller) : new List<AmazonOrderDto>() { };
                    //No data in Stuller
                    var skunotSupplierStuller = joinResult.Select(x => x.AmazonOrderId).Except(dataQgold.Select(x => x.AmazonOrderId)).ToList();
                    joinResult = joinResult.Where(x => skunotSupplierStuller.Contains(x.AmazonOrderId)).ToList();



                    joinResult.AddRange(dataQgold);
                    joinResult.AddRange(dataStuller);
                    joinResult.AddRange(resultOrderNotSubmit.Where(x => x.Supplier != "Many").ToList());
                    joinResult.AddRange(resultOrderUnShipped.Where(x => x.Supplier != "Many").ToList());
                    joinResult = joinResult.OrderByDescending(x => x.PurchaseDate).ToList();
                }
            }
            else
            {
                joinResult.AddRange(resultOrderNotSubmit.Where(x => x.Supplier != "Many").ToList());
                joinResult.AddRange(resultOrderUnShipped.Where(x => x.Supplier != "Many").ToList());
                joinResult = joinResult.OrderByDescending(x => x.PurchaseDate).ToList();
            }
            if (searchParam != null)
            {
                if (searchParam?.IsSubmitted is not null)
                {
                    joinResult = joinResult.Where(x => x.IsSubmitted == searchParam.IsSubmitted).ToList();
                }
            }
            result = Utilities2.CreatePagination(joinResult,
                        pageNumber: paginationParam.PageNumber,
                        pageSize: paginationParam.PageSize);

            return result;
        }

        public async Task<OperationResult> ReSubmit(QgoldFtpOrderObject order)
        {
            try
            {
                if (order == null)
                {
                    return new OperationResult() { IsSuccess = false, Data = "Data is not null" };
                }
                order._id = null;
                order.TimeStamp = DateTime.Now;
                List<QgoldFtpOrderObject> listAdd = new() { order };
                bool checkSubmit = false;
                if (_isProduction)
                {
                    checkSubmit = await _qgoldApiClient.SubmitOrder(listAdd, true);
                }
                if (checkSubmit)
                {
                    var dataAdd = _mapper.Map<OrderMarketplaceDto>(order);
                    //await _ggoldClientApi.AddReSubmitOrder(dataAdd);
                    await _amazonMongoRepository.InsertOne(order, amazonOrderSubmit);
                    return new OperationResult() { IsSuccess = true, Data = "Resubmit done" };
                }
                return new OperationResult() { IsSuccess = true, Data = "Re-Submit failed when call await _submitOrderQgoldService.SubmitQgold(model)" };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"ReSubmit error: {ex.Message}");
                return new OperationResult() { IsSuccess = false, Data = "System err" };
            }
        }

        public async Task<List<UnshipOrderDto>> GetUnshippedOrderIds(UnshippedOrderIdsParam param)
        {
            FilterDefinition<OrderAmazon>? filter = null;

            if (param != null)
            {
                if (!string.IsNullOrWhiteSpace(param.Provider))
                {
                    switch (param.Provider)
                    {
                        case "Stuller":
                            Utilities2.AddAndFilter(ref filter,
                            Builders<OrderAmazon>.Filter.Regex("OrderItemList.SellerSKU", "^S"));
                            break;
                        case "Qgold":
                            Utilities2.AddAndFilter(ref filter,
                            Builders<OrderAmazon>.Filter.Regex("OrderItemList.SellerSKU", "^Q"));
                            break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(param.FromDate))
                {
                    var fromDate = DateTime.Parse(param.FromDate);
                    Utilities2.AddAndFilter(ref filter, Builders<OrderAmazon>.Filter.Gt(x => x.PurchaseDate, fromDate));
                }
                if (!string.IsNullOrWhiteSpace(param.ToDate))
                {
                    var toDate = DateTime.Parse(param.ToDate).AddDays(1).AddMilliseconds(-1);
                    Utilities2.AddAndFilter(ref filter, Builders<OrderAmazon>.Filter.Lt(x => x.PurchaseDate, toDate));
                }
            }

            return (await _amazonMongoRepository.Find("Order", filter)).Data
                .Select(x => new UnshipOrderDto
                {
                    CustomerOrderId = x._id,
                    ItemUnShip = x.OrderItemList.Select(o => new ItemUnShip
                    {
                        Quantity = o.QuantityOrdered.ToString(),
                        Sku = o.SellerSKU,
                    }).ToList()
                }).ToList();
        }
    }
}
