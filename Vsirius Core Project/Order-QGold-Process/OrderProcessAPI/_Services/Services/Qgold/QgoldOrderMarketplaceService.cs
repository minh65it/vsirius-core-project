using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderProcessApi.Utilities;
using OrderProcessApi.Data;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Repositories.IRepositories;
using MongoDB.Bson;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class QgoldOrderMarketplaceService : IQgoldOrderMarketplaceService
    {
        private readonly IMongoCollection<OrderMarketplace> _orderMarketplace;
        private readonly IMapper _mapper;
        private readonly IQgoldProcessService _qgoldProcessService;
        private readonly IQgoldRepository _qgoldDB;

        public QgoldOrderMarketplaceService(IOptions<MongoDbSettings> databaseSettings, IMapper mapper, IQgoldProcessService qgoldProcessService, IQgoldRepository qgoldDB)
        {
            var mongoClient = new MongoClient(
             databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.QgoldDatabaseName);
            _orderMarketplace = mongoDatabase.GetCollection<OrderMarketplace>(databaseSettings.Value.QgoldMarketplace);
            _mapper = mapper;
            _qgoldProcessService = qgoldProcessService;
            _qgoldDB = qgoldDB;
        }

        public async Task<bool> AddOrder(List<QgoldFtpOrderObject> models)
        {

            try
            {
                var dataAdd = _mapper.Map<List<OrderMarketplace>>(models);
                await _orderMarketplace.InsertManyAsync(dataAdd);
                return true;
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Add Order Marketplace error: {ex.ToString()}", $"AddOrder_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return false;
            }
        }

        public Task<bool> DeleteOrder(List<string> orderid)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderMarketplaceDto> DetailOrderBySellerId(string orderSellerId)
        {
            OrderMarketplaceDto result = new OrderMarketplaceDto();
            FilterDefinition<OrderMarketplace> filter = Builders<OrderMarketplace>.Filter.Eq(x => x.Seller_Order_ID, orderSellerId);

            var data = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace, filter);
            if (data != null)
            {
                result = _mapper.Map<OrderMarketplaceDto>(data);
            }
            return result;
        }

        public async Task<OrderMarketplaceDto> DetailOrderById(string orderId)
        {
            OrderMarketplaceDto result = new OrderMarketplaceDto();
            FilterDefinition<OrderMarketplace> filter = Builders<OrderMarketplace>.Filter.Eq(x => x.OrderId, orderId);

            var data = await _qgoldDB.FisrtOrDefault(CollectionName.QgoldMarketplace, filter);
            if (data != null)
            {
                result = _mapper.Map<OrderMarketplaceDto>(data);
            }
            return result;
        }

        public async Task<PaginationResult<OrderMarketplaceDto>> GetOrder(OrderMarketplaceParams seachParam, PaginationParam page)
        {

            var result = new PaginationResult<OrderMarketplaceDto>();

            FilterDefinition<OrderMarketplace>? filters = null;

            if (seachParam != null)
            {

                if (!string.IsNullOrWhiteSpace(seachParam.OrderId))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("OrderId", new BsonRegularExpression(seachParam.OrderId));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Seller_Order_ID))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("Seller_Order_ID", new BsonRegularExpression(seachParam.Seller_Order_ID));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ShipToAddress1))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("ShipToAddress1", new BsonRegularExpression(seachParam.ShipToAddress1));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ShipToAddress2))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("ShipToAddress2", new BsonRegularExpression(seachParam.ShipToAddress2));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ShipToAddress3))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("ShipToAddress3", new BsonRegularExpression(seachParam.ShipToAddress3));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Marketplace))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("Marketplace", new BsonRegularExpression(seachParam.Marketplace));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ShipToName))
                {
                    var filter = Builders<OrderMarketplace>.Filter.
                        Regex("ShipToName", new BsonRegularExpression(seachParam.ShipToName));
                    Common.AddAndFilter(ref filters, filter);
                }

            }
            var data = await _qgoldDB.Find(CollectionName.QgoldMarketplace, filters, page.PageNumber, page.PageSize);
            result.PageNumber = data.PageNumber;
            result.PageSize = data.PageSize;
            result.TotalCount = data.TotalCount;
            result.PageCount = data.PageCount;
            result.Data = _mapper.Map<List<OrderMarketplaceDto>>(data.Data);

            return result;
        }

        public async Task<List<OrderMarketplace>> GetRecentOrder(int day)
        {
            try
            {
                var filter = Builders<OrderMarketplace>.Filter.And(
                Builders<OrderMarketplace>.Filter.Gte(x => x.TimeStamp, DateTime.Now.AddDays(day))
                );
                var result = await _orderMarketplace.Find(filter).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                await Common.WriteLogAsync($"Get Recent Order error: {ex.ToString()}", $"GetRecentOrder_Error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new List<OrderMarketplace>();
            }
        }

        public Task<bool> UpdateOrder(List<OrderMarketplace> model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOrderViaName(List<OrderMarketplaceParams> models)
        {
            bool result = true;
            bool result2 = true;
            models = models.OrderBy(x => x.OrderId).ToList();
            var listOrderMarket = await _orderMarketplace.Find(x => x.OrderId == null).Sort(Builders<OrderMarketplace>.Sort.Ascending(x => x.TimeStamp)).ToListAsync();
            foreach (var item in models)
            {
                try
                {
                    var checkOrder = listOrderMarket.Where(x => x.ShipToName[0].ToString().ToLower() == item.ShipToName[0].ToString().ToLower()
                        && x.ShipToAddress1[0].ToString().ToLower() == item.ShipToAddress1[0].ToString().ToLower()
                        && x.ShipToZip.Contains(item.ShipToZip)).FirstOrDefault();

                    if (checkOrder is null)
                    {
                        continue;
                    }
                    checkOrder.OrderId = item.OrderId;
                    await _orderMarketplace.ReplaceOneAsync(x => x._id == checkOrder._id, checkOrder);
                }
                catch (Exception ex)
                {
                    await Common.WriteLogAsync($"Update Order Via Name error: {ex.ToString()}", $"UpdateOrderViaName_Error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                    result = false;
                }
            }
            result2 = await _qgoldProcessService.UpdateReOrderViaName(models);

            return result && result2;
        }
    }
}
