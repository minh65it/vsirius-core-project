using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using FikaAmazonAPI;
using FikaAmazonAPI.Parameter.Order;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using static FikaAmazonAPI.Utils.Constants;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetNewOrder
{
    public interface IUpdateFullFields
    {
        public Task GetOrder(AmazonConnection a, List<string> MarketplaceIds, bool _isProduction);
        public Task DropCollection();
        public Task InsertOrder();
    }
    public class UpdateFullFields : IUpdateFullFields
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<OrderAmazon> _orderCollection;
        private readonly IMongoCollection<OrderAmazon> _orderCollection1;
        private readonly IMongoCollection<BsonDocument> _orderCollection1Insert;
        private readonly IMongoCollection<Order2Amazon> _orderCollection2;
        private string? conn = "";
        private string? dataBase = "";
        private string? orderCollection = "";
        private string? orderCollection1 = "";
        private string? orderCollection2 = "";
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        public UpdateFullFields(IConfiguration config)
        {
            _config = config;
            conn = _config.GetSection("ConnectionStrings:MongoDB").Value;
            dataBase = _config.GetSection("AmazonLocal:dataBase").Value;
            orderCollection = _config.GetSection("AmazonLocal:orderCollection").Value;
            orderCollection1 = _config.GetSection("AmazonLocal:orderCollection1").Value;
            orderCollection2 = _config.GetSection("AmazonLocal:orderCollection2").Value;

            _client = new MongoClient(conn);

            _database = _client.GetDatabase(dataBase);

            _orderCollection = _database.GetCollection<OrderAmazon>(orderCollection);
            _orderCollection1 = _database.GetCollection<OrderAmazon>(orderCollection1);
            _orderCollection1Insert = _database.GetCollection<BsonDocument>(orderCollection1);
            _orderCollection2 = _database.GetCollection<Order2Amazon>(orderCollection2);
        }
        public async Task<List<OrderAmazon>> GetOrder1()
        {
            var filter = Builders<OrderAmazon>.Filter.And(
               Builders<OrderAmazon>.Filter.Eq(x => x.IsUpdated, false)
               );

            var result = await _orderCollection1.Find(filter).ToListAsync();
            return result;
        }
        public async Task<List<Order2Amazon>> GetOrder2()
        {

            try
            {
                var result = await _orderCollection2.Find(_ => true).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"GetOrder2 error");
                return null;
            }
        }
        public async Task GetOrder(AmazonConnection a, List<string> MarketplaceIds, bool _isProduction)
        {
            await Utilities2.WriteLogAsync($"GetOrder Start");
            ParameterOrderList serachOrderList = new ParameterOrderList();
            serachOrderList.CreatedAfter = DateTime.UtcNow.AddMinutes(-21600); // 21600 nửa tháng

            serachOrderList.OrderStatuses = new List<OrderStatuses>();
            serachOrderList.OrderStatuses.Add(OrderStatuses.Unshipped);
            serachOrderList.MarketplaceIds = MarketplaceIds;
            if (!_isProduction)
            {
                serachOrderList.TestCase = TestCase200;
            }

            var orders = a.Orders.GetOrders(serachOrderList);
            if (orders.Count > 0)
            {
                var documentList = new List<BsonDocument>();
                foreach (var obj in orders)
                {
                    var orderItemList = _isProduction ? a.Orders.GetOrderItems(obj.AmazonOrderId) : new FikaAmazonAPI.AmazonSpApiSDK.Models.Orders.OrderItemList();

                    var objDocument = obj.ToBsonDocument();
                    objDocument["_id"] = obj.AmazonOrderId.ToString();
                    objDocument["OrderStatus"] = obj.OrderStatus.ToString();
                    objDocument["OrderType"] = obj.OrderType.ToString();
                    objDocument["PaymentMethod"] = obj.PaymentMethod.ToString();
                    objDocument["FulfillmentChannel"] = obj.FulfillmentChannel.ToString();
                    objDocument["IsUpdated"] = false;
                    objDocument["OrderItemList"] = BsonSerializer.Deserialize<BsonArray>(orderItemList?.ToJson());
                    documentList.Add(objDocument);
                }
                if (documentList.Count > 0)
                {
                    try
                    {

                        await _orderCollection1Insert.InsertManyAsync(documentList);
                        await Utilities2.WriteLogAsync($"Save new {documentList.Count()} order");
                    }
                    catch (Exception ex)
                    {

                        await Utilities2.WriteLogAsync($"Save new {documentList.Count()} order ERROR {ex.ToString()}");
                    }
                }
            }
            await Utilities2.WriteLogAsync($"GetOrder DONE");
        }

        public async Task InsertOrder()
        {
            await Utilities2.WriteLogAsync($"Insert Order1 to Order Processor =====================================================");

            var orders1 = await GetOrder1();
            var orderInsert = new List<OrderAmazon>();
            var orders2 = await GetOrder2();

            foreach (var obj in orders1)
            {
                // check data in DB
                var checkID = orders2.Where(x => x.OrderID == obj.AmazonOrderId).FirstOrDefault();
                if (checkID != null)
                {
                    obj.ShippingAddress.Name = checkID.BuyerName;
                    obj.ShippingAddress.AddressLine1 = checkID.Address1;
                    obj.ShippingAddress.AddressLine2 = checkID.Address2;
                    obj.ShippingAddress.Phone = checkID.Phone;
                    orderInsert.Add(obj);

                }
            }
            if (orderInsert.Count > 0)
            {
                await _database.DropCollectionAsync(orderCollection);
                await _orderCollection.InsertManyAsync(orderInsert);
                await Utilities2.WriteLogAsync($"Save new {orderInsert.Count()} Order");
            }
            await Utilities2.WriteLogAsync("Insert Order1,2 to Order Amazon Processor done");
        }

        public async Task InsertOrderV1()
        {
            await Utilities2.WriteLogAsync($"Insert Order1 to Order Processor =====================================================");

            var orders1 = await GetOrder1();
            var orderInsert = new List<OrderAmazon>();
            var orders2 = await GetOrder2();

            foreach (var obj in orders1)
            {
                // check data in DB
                var checkID = orders2.Where(x => x.OrderID == obj.AmazonOrderId).FirstOrDefault();
                if (checkID != null)
                {
                    obj.ShippingAddress.Name = checkID.BuyerName;
                    obj.ShippingAddress.AddressLine1 = checkID.Address1;
                    obj.ShippingAddress.AddressLine2 = checkID.Address2;
                    obj.ShippingAddress.Phone = checkID.Phone;
                    orderInsert.Add(obj);

                }
            }
            if (orderInsert.Count > 0)
            {
                await _orderCollection.InsertManyAsync(orderInsert);
                await Utilities2.WriteLogAsync($"Save new {orderInsert.Count()} Order");

                //Update
                foreach (var i in orderInsert)
                {
                    //Update
                    var filterUpdate = Builders<OrderAmazon>.Filter
                        .Eq(x => x.AmazonOrderId, i.AmazonOrderId);
                    var update = Builders<OrderAmazon>.Update.Set("IsUpdated", true);
                    await _orderCollection1.UpdateManyAsync(filterUpdate, update);
                }
                await Utilities2.WriteLogAsync($"Updated {orderInsert.Count()} record Order1");
            }
            await Utilities2.WriteLogAsync("Insert Order1,2 to Order Amazon Processor done");
        }

        public async Task DropCollection()
        {
            await _database.DropCollectionAsync(orderCollection);
            await _database.DropCollectionAsync(orderCollection1);
        }
    }
}
