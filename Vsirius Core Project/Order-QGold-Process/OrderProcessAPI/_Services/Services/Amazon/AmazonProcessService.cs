using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Amazon;
using OrderProcessApi.Data;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using System.Text;
using System.Text.RegularExpressions;
using OrderProcessApi.Utilities.Amazon;

namespace OrderProcessApi._Services.Services.Amazon
{
    public class AmazonProcessService : IAmazonProcessService
    {
        IQgoldRepository _qgoldDB;
        IAmazonRepository _amazonDB;
        private readonly ISubmitOrderQgoldService _submitOrderQgoldService;
        private readonly IMapper _mapper;
        private readonly IUpdateFullFields _updateFullFields;

        public AmazonProcessService(
            IQgoldRepository qgoldDB,
            IAmazonRepository amazonDB,
            ISubmitOrderQgoldService submitOrderQgoldService,
            IMapper mapper,
            IUpdateFullFields updateFullFields)
        {
            _qgoldDB = qgoldDB;
            _amazonDB = amazonDB;
            _submitOrderQgoldService = submitOrderQgoldService;
            _mapper = mapper;
            _updateFullFields = updateFullFields;
        }

        public async Task<Result> DeleteOrders(List<string> ds)
        {
            var pointDateCanDelete = DateTime.Now.AddMonths(-8);

            FilterDefinition<OrderAmazonSeller>? filterAmazon = null;
            FilterDefinition<OrderUIAmazon>? filterUI = null;
            FilterDefinition<OrderAmazonSubmitted>? filterSubmit = null;
            FilterDefinition<OrderAmazonTracking>? filterTracking = null;
            FilterDefinition<OrderMarketplace>? filterMarket = null;

            foreach (var item in ds)
            {
                Common.AddOrFilter(ref filterAmazon, Builders<OrderAmazonSeller>.Filter.Eq(x => x.AmazonOrderId, item));
                Common.AddOrFilter(ref filterUI, Builders<OrderUIAmazon>.Filter.Eq(x => x.OrderID, item));
                Common.AddOrFilter(ref filterMarket, Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, item));
                Common.AddOrFilter(ref filterSubmit, Builders<OrderAmazonSubmitted>.Filter.Eq(x => x.Amz_Order_ID, item));
                Common.AddOrFilter(ref filterTracking, Builders<OrderAmazonTracking>.Filter.Eq(x => x.AmazonOrderID, item));
            }

            var lstAmazon = (await _amazonDB.Find(CollectionName.AmazonOrder, filterAmazon)).Data;
            var lstUI = (await _amazonDB.Find(CollectionName.AmazonOrderUI, filterUI)).Data;
            var lstMarketplace = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket)).Data;
            var lstSubmit = (await _qgoldDB.Find(CollectionName.AmazonOrderSubmit, filterSubmit)).Data;
            var lstTracking = (await _qgoldDB.Find(CollectionName.AmazonOrderTracking, filterTracking)).Data;

            var amazError = new StringBuilder();
            foreach (var item in lstAmazon)
            {
                amazError.Append($"{item.AmazonOrderId}, ");
            }

            var amazUIError = new StringBuilder();
            foreach (var item in lstUI)
            {
                if (item.UpdatedDate > pointDateCanDelete)
                {
                    amazError.Append($"{item.OrderID}, ");
                }
                else
                {
                    await _amazonDB.DeleteMany(CollectionName.AmazonOrderUI,
                        Builders<OrderUIAmazon>.Filter.Eq(x => x.OrderID, item.OrderID));
                }
            }

            var amzMarketplaceErr = new StringBuilder();
            foreach (var item in lstMarketplace)
            {
                if (item.TimeStamp > pointDateCanDelete)
                {
                    amzMarketplaceErr.Append($"{item.Seller_Order_ID}, ");
                }
                else
                {
                    await _qgoldDB.DeleteMany(CollectionName.QgoldMarketplace,
                        Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, item.Seller_Order_ID));
                }
            }

            var amzSubmitErr = new StringBuilder();
            foreach (var item in lstSubmit)
            {
                if (item.TimeStamp > pointDateCanDelete)
                {
                    amazError.Append($"{item.Amz_Order_ID}, ");
                }
                else
                {
                    await _amazonDB.DeleteMany(CollectionName.AmazonOrderSubmit,
                        Builders<OrderAmazonSubmitted>.Filter.Eq(x => x.Amz_Order_ID, item.Amz_Order_ID));
                }
            }

            var amzTrackingErr = new StringBuilder();
            foreach (var item in lstTracking)
            {
                if (DateTime.Parse(item.FulfillmentDate) > pointDateCanDelete)
                {
                    amazError.Append($"{item.AmazonOrderID}, ");
                }
                else
                {
                    await _amazonDB.DeleteMany(CollectionName.AmazonOrderTracking,
                        Builders<OrderAmazonTracking>.Filter.Eq(x => x.AmazonOrderID, item.AmazonOrderID));
                }
            }

            var msg = $"{(amazError.Length > 0 ? amazError + "in Order Amazon delete failed.\n" : "")}" +
                $"{(amazUIError.Length > 0 ? amazUIError + "in Order2 Amazon delete failed.\n" : "")}" +
                $"{(amzSubmitErr.Length > 0 ? amzSubmitErr + "in OrderSubmitted Amazon delete failed.\n" : "")}" +
                $"{(amzMarketplaceErr.Length > 0 ? amazError + "in OrderMarketplace Qgold delete failed.\n" : "")}" +
                $"{(amzTrackingErr.Length > 0 ? amazError + "in OrderTracking Amazon delete failed.\n" : "")}"
                ;
            if (msg.Trim().Length > 0)
            {
                return new Result() { IsSuccess = false, Message = "Have Error" };
            }
            return new Result() { IsSuccess = true };
        }

        public async Task<AmazonOrderDetailDTO?> GetDetailOrder(string id)
        {
            AmazonOrderDetailDTO? result = null;
            var orderAmazon = await _amazonDB.FisrtOrDefault(CollectionName.AmazonOrder, Builders<OrderAmazonSeller>.Filter.Eq(x => x.AmazonOrderId, id));
            var orderSubmitAmazon = await _amazonDB.FisrtOrDefault(CollectionName.AmazonOrderSubmit, Builders<OrderAmazonSubmitted>.Filter.Eq(x => x.Amz_Order_ID, id));

            if (orderAmazon is not null)
            {
                var orderQgold = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace, Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, id));

                result = new()
                {
                    AmazonOrderId = orderAmazon.AmazonOrderId,
                    QgoldOrderId = orderQgold?.OrderId,
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
                    OrderItemList = CustomMapping.MapItemOrderAmazon(orderAmazon.OrderItemList),
                };
            }
            else if (orderSubmitAmazon is not null)
            {
                var orderQgold = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace, Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, id));

                result = new()
                {
                    AmazonOrderId = orderSubmitAmazon.Amz_Order_ID,
                    QgoldOrderId = orderQgold?.OrderId,
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
                    OrderItemList = CustomMapping.MapItemOrderAmazon1(orderSubmitAmazon.Item),
                };
            }

            return result;
        }

        public async Task<PaginationResult<AmazonOrderDTO>> GetOrders(PaginationParam paginationParam, AmazonProcessSearchParam seachParam)
        {
            var result = new PaginationResult<AmazonOrderDTO>();

            FilterDefinition<OrderAmazonSeller>? filterOrder = null;
            FilterDefinition<OrderAmazonSubmitted>? filterSubmitAmazon = null;

            if (!string.IsNullOrWhiteSpace(seachParam.QgoldOrderId))
            {
                var filterMarket = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.OrderId, seachParam.QgoldOrderId, RegexOptions.IgnoreCase));
                var marketplace = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket)).Data;
                if (marketplace.Any())
                {
                    FilterDefinition<OrderAmazonSeller>? filterA = null;
                    FilterDefinition<OrderAmazonSubmitted>? filterSub = null;
                    foreach (var item in marketplace)
                    {
                        Common.AddOrFilter(ref filterA,
                            Builders<OrderAmazonSeller>.Filter.Eq(x => x.AmazonOrderId, item.Seller_Order_ID));
                        Common.AddOrFilter(ref filterSub,
                            Builders<OrderAmazonSubmitted>.Filter.Eq(x => x.Amz_Order_ID, item.Seller_Order_ID));
                    }
                    Common.AddAndFilter(ref filterOrder, filterA);
                    Common.AddAndFilter(ref filterSubmitAmazon, filterSub);

                    List<OrderAmazonSeller> lstAmazon = (await _amazonDB.Find(CollectionName.AmazonOrder,
                    filter: filterOrder)).Data;
                    List<OrderAmazonSubmitted> lstSubmit = (await _amazonDB.Find(CollectionName.AmazonOrderSubmit,
                        filter: filterSubmitAmazon)).Data;

                    var joinResult = CustomMapping.ConvertToAmazonOrderDTO(lstAmazon, lstSubmit);

                    result = Common.CreatePagination(joinResult,
                                pageNumber: paginationParam.PageNumber,
                                pageSize: paginationParam.PageSize);

                    if (result.Data.Any())
                    {
                        foreach (var item in result.Data)
                        {
                            item.QgoldOrderId = marketplace.FirstOrDefault(x => x.Seller_Order_ID == item.AmazonOrderId)?.OrderId;
                        }
                    }
                }
                return result;
            }
            else
            {
                List<OrderAmazonSeller> lstAmazon = (await _amazonDB.Find(CollectionName.AmazonOrder,
                    filter: filterOrder)).Data;
                List<OrderAmazonSubmitted> lstSubmit = (await _amazonDB.Find(CollectionName.AmazonOrderSubmit,
                    filter: filterSubmitAmazon)).Data;

                //join result
                var joinResult = CustomMapping.ConvertToAmazonOrderDTO(lstAmazon, lstSubmit);
                joinResult = joinResult.OrderByDescending(x => x.PurchaseDate).ToList();

                if (seachParam != null)
                {
                    if (!string.IsNullOrWhiteSpace(seachParam.AddressLine1))
                    {
                        joinResult = joinResult.Where(x => x.AddressLine1.Contains(seachParam.AddressLine1, StringComparison.OrdinalIgnoreCase)).ToList();

                        //var filter = Builders<OrderAmazonSeller>.Filter.Where(x =>
                        //                    Regex.IsMatch(x.ShippingAddress.AddressLine1, seachParam.AddressLine1, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterOrder, filter);
                        //var filter1 = Builders<OrderAmazonSubmitted>.Filter.Where(x =>
                        //                    Regex.IsMatch(x.ShipToAddress1, seachParam.AddressLine1, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterSubmitAmazon, filter1);
                    }
                    if (!string.IsNullOrWhiteSpace(seachParam.BuyerName))
                    {
                        joinResult = joinResult.Where(x => x.BuyerName.Contains(seachParam.BuyerName, StringComparison.OrdinalIgnoreCase)).ToList();


                        //var filter = Builders<OrderAmazonSeller>.Filter.Where(x =>
                        //                        Regex.IsMatch(x.ShippingAddress.Name, seachParam.BuyerName, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterOrder, filter);
                        //var filter1 = Builders<OrderAmazonSubmitted>.Filter.Where(x =>
                        //                    Regex.IsMatch(x.ShipToName, seachParam.BuyerName, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterSubmitAmazon, filter1);
                    }
                    if (!string.IsNullOrWhiteSpace(seachParam.AmazonOrderId))
                    {
                        joinResult = joinResult.Where(x => x.AmazonOrderId.Contains(seachParam.AmazonOrderId, StringComparison.OrdinalIgnoreCase)).ToList();


                        //var filter = Builders<OrderAmazonSeller>.Filter.Where(x =>
                        //                            Regex.IsMatch(x.AmazonOrderId, seachParam.AmazonOrderId, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterOrder, filter);

                        //var filter1 = Builders<OrderAmazonSubmitted>.Filter.Where(x =>
                        //                    Regex.IsMatch(x.Amz_Order_ID, seachParam.AmazonOrderId, RegexOptions.IgnoreCase));
                        //Common.AddAndFilter(ref filterSubmitAmazon, filter1);
                    }
                    if (!string.IsNullOrWhiteSpace(seachParam.SKU))
                    {
                        joinResult = joinResult.Where(x => x.OrderItemList.Any(x => x.SellerSKU.Contains(seachParam.SKU, StringComparison.OrdinalIgnoreCase))).ToList();


                        //var filter = Builders<OrderAmazonSeller>.Filter.
                        //    Regex("OrderItemList.SellerSKU", new BsonRegularExpression(seachParam.SKU, "i"));
                        //Common.AddAndFilter(ref filterOrder, filter);

                        //var filter1 = Builders<OrderAmazonSubmitted>.Filter
                        //    .Regex("Item.Item", new BsonRegularExpression(seachParam.SKU, "i"));
                        //Common.AddAndFilter(ref filterSubmitAmazon, filter1);
                    }
                    if (!string.IsNullOrWhiteSpace(seachParam.FromDate) && !string.IsNullOrWhiteSpace(seachParam.ToDate))
                    {
                        var fromDate = DateTime.Parse(seachParam.FromDate);
                        var toDate = DateTime.Parse(seachParam.ToDate).AddDays(1).AddMilliseconds(-1);

                        joinResult = joinResult.Where(x => x.PurchaseDate >= fromDate && x.PurchaseDate <= toDate).ToList();


                        //var filter = Builders<OrderAmazonSeller>.Filter.And(
                        //    Builders<OrderAmazonSeller>.Filter.Lte("PurchaseDate", toDate),
                        //    Builders<OrderAmazonSeller>.Filter.Gte("PurchaseDate", fromDate)
                        //);
                        //Common.AddAndFilter(ref filterOrder, filter);

                        //var filter1 = Builders<OrderAmazonSubmitted>.Filter.And(
                        //    Builders<OrderAmazonSubmitted>.Filter.Lte(x => x.TimeStamp, toDate),
                        //    Builders<OrderAmazonSubmitted>.Filter.Gte(x => x.TimeStamp, fromDate)
                        //);
                        //Common.AddAndFilter(ref filterSubmitAmazon, filter1);
                    }

                    if (seachParam?.IsSubmitted is not null)
                    {
                        joinResult = joinResult.Where(x => x.IsSubmitted == seachParam.IsSubmitted).ToList();
                    }
                }

                result = Common.CreatePagination(joinResult,
                            pageNumber: paginationParam.PageNumber,
                            pageSize: paginationParam.PageSize);

                if (result.Data.Any())
                {
                    var lstMap = (
                        await _qgoldDB.Find(CollectionName.QgoldMarketplace,
                        Builders<OrderMarketplace>.Filter
                        .Where(x => result.Data.Select(x => x.AmazonOrderId).Contains(x.Seller_Order_ID))
                        )
                        ).Data;
                    foreach (var item in result.Data)
                    {
                        item.QgoldOrderId = lstMap.FirstOrDefault(x => x.Seller_Order_ID == item.AmazonOrderId)?.OrderId;
                    }
                }
            }

            return result;
        }

        public async Task<OperationResult> ReSubmit(QgoldFtpOrderObject order)
        {
            try
            {
                //await _amazonDB.DeleteMany(CollectionName.AmazonOrderSubmit, Builders<OrderAmazonSeller>.Filter.Eq(x => x.AmazonOrderId, order.Amz_Order_ID));
                List<QgoldFtpOrderObject> listAdd = new List<QgoldFtpOrderObject>() { order };
                var checkSubmit = await _submitOrderQgoldService.SubmitQgold(listAdd, true);
                if (checkSubmit)
                {
                    var dataAdd = _mapper.Map<OrderMarketplace>(order);
                    await _qgoldDB.InsertOne(dataAdd, CollectionName.QgoldReOrder);
                    await _amazonDB.InsertOne(order, CollectionName.AmazonOrderSubmit);
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

        public async Task<OperationResult> GetNewOrders()
        {
            try
            {

                await _updateFullFields.InsertNewOrder();
                return new OperationResult() { Success = true };
            }
            catch (Exception)
            {
                return new OperationResult() { Success = false };
            }
        }
    }
}
