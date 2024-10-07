using MongoDB.Bson;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.Qgold;
using System.Text;
using System.Text.RegularExpressions;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class QgoldProcessService : IQgoldProcessService
    {
        IQgoldRepository _qgoldDB;

        public QgoldProcessService(IQgoldRepository qgoldDB)
        {
            _qgoldDB = qgoldDB;
        }

        public async Task<Result> DeleteOrders(List<string> ds)
        {
            var pointDateCanDelete = DateTime.Now.AddMonths(-6);

            FilterDefinition<OrderQgold>? filterQgold = null;
            FilterDefinition<OrderMarketplace>? filterMarket = null;
            FilterDefinition<TrackingQgoldOrder>? filterTracking = null;

            foreach (var item in ds)
            {
                Common.AddOrFilter(ref filterQgold, Builders<OrderQgold>.Filter.Eq(x => x.Order, item));
                Common.AddOrFilter(ref filterMarket, Builders<OrderMarketplace>.Filter.Eq(x => x.OrderId, item));
                Common.AddOrFilter(ref filterTracking, Builders<TrackingQgoldOrder>.Filter.Eq(x => x.OrderQgoldId, item));
            }

            var lstQgold = (await _qgoldDB.Find(CollectionName.QgoldOrder, filterQgold)).Data;
            var lstMarket = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket)).Data;
            var lstTracking = (await _qgoldDB.Find(CollectionName.QgoldTracking, filterTracking)).Data;

            var qgoldErr = new StringBuilder();
            foreach (var item in lstQgold)
            {
                if (item.UpdatedDate > pointDateCanDelete)
                {
                    qgoldErr.Append($"{item.Order}, ");
                }
                else
                {
                    await _qgoldDB.DeleteMany(CollectionName.QgoldOrder,
                        Builders<OrderQgold>.Filter.Eq(x => x.Order, item.Order));
                }
            }

            var marketErr = new StringBuilder();
            foreach (var item in lstMarket)
            {
                if (item.TimeStamp > pointDateCanDelete)
                {
                    marketErr.Append($"{item.OrderId}, ");
                }
                else
                {
                    await _qgoldDB.DeleteMany(CollectionName.QgoldMarketplace,
                        Builders<OrderMarketplace>.Filter.Eq(x => x.OrderId, item.OrderId));
                }
            }

            var trackingErr = new StringBuilder();
            foreach (var item in lstTracking)
            {
                if (item.UpdatedDate > pointDateCanDelete)
                {
                    trackingErr.Append($"{item.OrderQgoldId}, ");
                }
                else
                {
                    await _qgoldDB.DeleteMany(CollectionName.QgoldTracking,
                        Builders<TrackingQgoldOrder>.Filter.Eq(x => x.OrderQgoldId, item.OrderQgoldId));
                }
            }

            var msg = $"{(qgoldErr.Length > 0 ? qgoldErr + "in Order Qgold delete failed.\n" : "")}" +
                 $"{(marketErr.Length > 0 ? marketErr + "in OrderMarketplace Qgold delete failed.\n" : "")}" +
                 $"{(trackingErr.Length > 0 ? trackingErr + "in Tracking Qgold delete failed.\n" : "")}"
                 ;
            if (msg.Trim().Length > 0)
            {
                return new Result() { IsSuccess = false, Message = msg };
            }
            return new Result() { IsSuccess = true };
        }

        public async Task<QgoldOrderDetailDTO?> GetDetailOrder(string id)
        {
            QgoldOrderDetailDTO? result = null;
            FilterDefinition<OrderQgold> filterUI = Builders<OrderQgold>.Filter.Eq(x => x.Order, id);
            FilterDefinition<OrderMarketplace> filterMarket = Builders<OrderMarketplace>.Filter.Eq(x => x.OrderId, id);

            var orderUI = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldOrder, filterUI);
            if (orderUI != null)
            {
                var orderMarketplace = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace, filterMarket);
                var lostPackage = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldLostPackage, filterMarket);
                result = CustomMapping.ConvertToDetailOrder(orderUI, orderMarketplace, lostPackage);
            }
            return result;
        }

        public async Task<PaginationResult<QgoldOrderDTO>> GetOrders(PaginationParam paginationParam, SearchQgoldProcessParam seachParam)
        {
            var result = new PaginationResult<QgoldOrderDTO>();

            FilterDefinition<OrderQgold>? filterQgold = null;
            FilterDefinition<OrderMarketplace>? filterMarket = null;
            var pagOrderUIQgold = new PaginationResult<OrderQgold>();
            var lstMarketplace = new List<OrderMarketplace>();
            var lostPackage = new List<OrderMarketplace>();
            bool isUseFilterMarket = false;

            if (seachParam != null)
            {
                //Filter Order Qgold
                if (seachParam.OrderStatus is not null)
                {
                    switch (seachParam.OrderStatus)
                    {
                        case OrderQgoldStatus.Invoiced:
                        case OrderQgoldStatus.Cancelled:
                        case OrderQgoldStatus.Pending:
                        case OrderQgoldStatus.Hold:
                        case OrderQgoldStatus.Backordered:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, seachParam.OrderStatus.ToString());
                            break;
                        case OrderQgoldStatus.Other:
                            filterQgold = Builders<OrderQgold>.Filter.And(
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Invoiced.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Cancelled.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Pending.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Hold.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Backordered.ToString())
                                );
                            break;

                    }
                }
                if (!string.IsNullOrWhiteSpace(seachParam.BuyerName))
                {
                    var filter = Builders<OrderQgold>.Filter.
                        Regex("InvoiceDetails.ShipToText", new BsonRegularExpression(seachParam.BuyerName, "i"));
                    Common.AddAndFilter(ref filterQgold, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.OrderQgoldId))
                {
                    var filter = Builders<OrderQgold>.Filter.Where(x =>
                                    Regex.IsMatch(x.Order, seachParam.OrderQgoldId, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterQgold, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Style))
                {
                    var filter = Builders<OrderQgold>.Filter.
                        Regex("InvoiceDetails.QgoldInvoiceItems.Style", new BsonRegularExpression(seachParam.Style, "i"));
                    Common.AddAndFilter(ref filterQgold, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam?.FromDate) && !string.IsNullOrWhiteSpace(seachParam?.ToDate))
                {
                    var fromDate = DateTime.Parse(seachParam.FromDate);
                    var toDate = DateTime.Parse(seachParam.ToDate);

                    List<string> lstDateRange = GetDateRange(fromDate, toDate);

                    var filter = Builders<OrderQgold>.Filter.Where(x => lstDateRange.Contains(x.Date));

                    Common.AddAndFilter(ref filterQgold, filter);
                }

                //Filter Marketplace
                if (!string.IsNullOrWhiteSpace(seachParam?.Marketplace))
                {
                    var filter = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.Marketplace, seachParam.Marketplace, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterMarket, filter);
                    isUseFilterMarket = true;
                }
                if (!string.IsNullOrWhiteSpace(seachParam?.MarketOrderId))
                {
                    var filter = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.Seller_Order_ID, seachParam.MarketOrderId, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterMarket, filter);
                    isUseFilterMarket = true;
                }

                var sortQgoldOrder = Builders<OrderQgold>.Sort.Descending(x => x.Order);

                if (isUseFilterMarket)
                {
                    lstMarketplace = (await _qgoldDB.Find(CollectionName.QgoldMarketplace, filterMarket)).Data;

                    var lstMarketplaceID = lstMarketplace.Select(x => x.OrderId).ToList();
                    Common.AddAndFilter(ref filterQgold, Builders<OrderQgold>.Filter.Where(x => lstMarketplaceID.Contains(x.Order)));

                    pagOrderUIQgold = await _qgoldDB
                        .Find(CollectionName.QgoldOrder,
                        filterQgold,
                        paginationParam.PageNumber,
                        paginationParam.PageSize,
                        sortQgoldOrder
                        );
                    var lstOrderQgoldMarketplace = pagOrderUIQgold.Data.Select(x => x.Order).ToList();

                    lostPackage = (await _qgoldDB
                        .Find(CollectionName.QgoldLostPackage,
                        Builders<OrderMarketplace>.Filter.Where(x => lstOrderQgoldMarketplace.Contains(x.OrderId))
                        )).Data;

                    // true => lstMarketplace còn khá nhiều kq nhưng sẽ ko dùng
                    result = CustomMapping.ConvertToQgoldOrderDTO(pagOrderUIQgold, lstMarketplace, lostPackage, isUseFilterMarket);
                }
                else
                {
                    pagOrderUIQgold = await _qgoldDB
                        .Find(CollectionName.QgoldOrder,
                        filterQgold,
                        paginationParam.PageNumber,
                        paginationParam.PageSize,
                        sortQgoldOrder
                        );
                    var lstOrderQgoldMarketplace = pagOrderUIQgold.Data.Select(x => x.Order).ToList();

                    lstMarketplace = (await _qgoldDB
                        .Find(CollectionName.QgoldMarketplace,
                        Builders<OrderMarketplace>.Filter.Where(x => lstOrderQgoldMarketplace.Contains(x.OrderId))
                        )).Data;

                    lostPackage = (await _qgoldDB
                        .Find(CollectionName.QgoldLostPackage,
                        Builders<OrderMarketplace>.Filter.Where(x => lstOrderQgoldMarketplace.Contains(x.OrderId))
                        )).Data;

                    // false => lstMarketplace ít kết quả
                    result = CustomMapping.ConvertToQgoldOrderDTO(pagOrderUIQgold, lstMarketplace, lostPackage, isUseFilterMarket);
                }
            }

            return result;
        }

        private List<string> GetDateRange(DateTime fromDate, DateTime toDate)
        {
            List<string> dateList = new();

            // Thêm từng ngày vào danh sách
            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                dateList.Add(date.ToString("M/d/yyyy"));
            }

            return dateList;
        }

        public async Task<PaginationResult<QgoldOrderDTO>> GetReOrders(PaginationParam paginationParam, SearchQgoldProcessParam seachParam)
        {
            FilterDefinition<OrderMarketplace>? filterResubmit = null;
            FilterDefinition<OrderQgold>? filterQgold = null;

            var result = new PaginationResult<QgoldOrderDTO>();

            if (seachParam != null)
            {
                if (seachParam.OrderStatus is not null)
                {
                    switch (seachParam.OrderStatus)
                    {
                        case OrderQgoldStatus.Invoiced:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, OrderQgoldStatus.Invoiced.ToString());
                            break;
                        case OrderQgoldStatus.Cancelled:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, OrderQgoldStatus.Cancelled.ToString());
                            break;
                        case OrderQgoldStatus.Pending:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, OrderQgoldStatus.Pending.ToString());
                            break;
                        case OrderQgoldStatus.Hold:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, OrderQgoldStatus.Hold.ToString());
                            break;
                        case OrderQgoldStatus.Backordered:
                            filterQgold = Builders<OrderQgold>.Filter.Eq(x => x.Status, OrderQgoldStatus.Backordered.ToString());
                            break;
                        case OrderQgoldStatus.Other:
                            filterQgold = Builders<OrderQgold>.Filter.And(
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Invoiced.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Cancelled.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Pending.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Hold.ToString()),
                                Builders<OrderQgold>.Filter.Ne(x => x.Status, OrderQgoldStatus.Backordered.ToString())
                                );
                            break;

                    }
                }
                if (!string.IsNullOrWhiteSpace(seachParam.BuyerName))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("ShipToName", new BsonRegularExpression(seachParam.BuyerName, "i"));
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.OrderQgoldId))
                {
                    var filter = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.OrderId, seachParam.OrderQgoldId, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Style))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("Item.Item", new BsonRegularExpression(seachParam.Style, "i"));
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Marketplace))
                {
                    var filter = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.Marketplace, seachParam.Marketplace, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.MarketOrderId))
                {
                    var filter = Builders<OrderMarketplace>.Filter.Where(x =>
                                    Regex.IsMatch(x.Seller_Order_ID, seachParam.MarketOrderId, RegexOptions.IgnoreCase));
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.FromDate) && !string.IsNullOrWhiteSpace(seachParam.ToDate))
                {
                    var fromDate = DateTime.Parse(seachParam.FromDate);
                    var toDate = DateTime.Parse(seachParam.ToDate).AddDays(1).AddSeconds(-1);

                    var filter = Builders<OrderMarketplace>.Filter.And(
                        Builders<OrderMarketplace>.Filter.Lte(x => x.TimeStamp, toDate),
                        Builders<OrderMarketplace>.Filter.Gte(x => x.TimeStamp, fromDate)
                        );
                    Common.AddAndFilter(ref filterResubmit, filter);
                }
            }

            if (filterQgold != null)
            {
                var lstReoOrder = (await _qgoldDB.Find(CollectionName.QgoldReOrder, filterResubmit)).Data;
                FilterDefinition<OrderQgold>? filter1 = null;
                foreach (var item in lstReoOrder)
                {
                    Common.AddOrFilter(ref filter1, Builders<OrderQgold>.Filter.Eq(x => x.Order, item.OrderId));
                }
                Common.AddAndFilter(ref filterQgold, filter1);
                var sort = Builders<OrderQgold>.Sort.Descending(x => x.Order);

                var paginationQgold = await _qgoldDB.Find(CollectionName.QgoldOrder,
                    filterQgold,
                    paginationParam.PageNumber,
                    paginationParam.PageSize,
                    sort
                    );
                result = new()
                {
                    PageSize = paginationParam.PageSize,
                    PageCount = paginationQgold.PageCount,
                    TotalCount = paginationQgold.TotalCount,
                    PageNumber = paginationQgold.PageNumber,
                    Data = paginationQgold.Data.Select(CustomMapping.QgoldToQgoldOrderDTO).ToList()
                };

                foreach (var item in result.Data)
                {
                    var obj = lstReoOrder.FirstOrDefault(x => x.OrderId == item.OrderQgoldId);
                    item.MarketOrderId = obj?.Seller_Order_ID;
                    item.Marketplace = obj?.Marketplace;
                }
            }
            else
            {
                var sort = Builders<OrderMarketplace>.Sort.Descending(x => x.TimeStamp);
                var pagReOrderQgold = await _qgoldDB.Find(CollectionName.QgoldReOrder,
                    filterResubmit, paginationParam.PageNumber, paginationParam.PageSize, sort);
                foreach (var item in pagReOrderQgold.Data)
                {
                    Common.AddOrFilter(ref filterQgold, Builders<OrderQgold>.Filter.Eq(x => x.Order, item.OrderId));
                }
                var lstQgold = (await _qgoldDB.Find(CollectionName.QgoldOrder, filterQgold)).Data;
                result = new PaginationResult<QgoldOrderDTO>()
                {
                    PageNumber = pagReOrderQgold.PageNumber,
                    PageCount = pagReOrderQgold.PageCount,
                    PageSize = pagReOrderQgold.PageSize,
                    TotalCount = pagReOrderQgold.TotalCount,
                    Data = pagReOrderQgold.Data.Select(CustomMapping.MapReOrderToQgoldOrderDTO).ToList()
                };
                foreach (var item in result.Data)
                {
                    var obj = lstQgold.FirstOrDefault(x => x.Order == item.OrderQgoldId);
                    item.OrderStatus = obj?.Status;
                    item.ShipVia = obj?.InvoiceDetails[0]?.ShipVia;
                    item.Total = obj?.Total;
                    item.Tracking = obj?.InvoiceDetails[0]?.Tracking;
                    item.ZipCode = obj?.InvoiceDetails[0]?.ZipCode;
                }
            }

            return result;
        }

        public async Task<bool> UpdateReOrderViaName(List<OrderMarketplaceParams> models)
        {
            bool result = true;
            models = models.OrderBy(x => x.OrderId).ToList();
            var listReOrder = (await _qgoldDB.Find(CollectionName.QgoldReOrder, Builders<OrderMarketplace>.Filter.Eq(x => x.OrderId, null))).Data;
            foreach (var item in models)
            {
                try
                {
                    var checkOrder = listReOrder.Where(x => x.ShipToName[0].ToString().ToLower() == item.ShipToName[0].ToString().ToLower()
                        && x.ShipToAddress1[0].ToString().ToLower() == item.ShipToAddress1[0].ToString().ToLower()
                        && x.ShipToZip.Contains(item.ShipToZip)).FirstOrDefault();

                    if (checkOrder is null)
                    {
                        continue;
                    }
                    checkOrder.OrderId = item.OrderId;
                    await _qgoldDB.ReplaceOne(CollectionName.QgoldReOrder, checkOrder, Builders<OrderMarketplace>.Filter.Eq(x => x._id, checkOrder._id));
                }
                catch (Exception ex)
                {
                    await Common.WriteLogAsync($"Update Re-Order Via Name error: {ex.ToString()}", $"Update_Re_OrderViaName_Error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                    result = false;
                }
            }
            return result;
        }
    }
}
