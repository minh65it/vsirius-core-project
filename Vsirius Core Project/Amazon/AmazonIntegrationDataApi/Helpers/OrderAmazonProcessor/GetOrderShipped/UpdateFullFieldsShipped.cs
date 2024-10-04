using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using FikaAmazonAPI.Parameter.Order;
using FikaAmazonAPI;
using MongoDB.Bson;
using MongoDB.Driver;
using static FikaAmazonAPI.Utils.Constants;
using MongoDB.Bson.Serialization;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetOrderShipped
{
    public interface IUpdateFullFieldsShipped
    {
        public Task GetOrderShipped(AmazonConnection a, List<string> MarketplaceIds, bool _isProduction);
        public Task DropCollection();
        public Task InsertOrderShipped();
    }
    public class UpdateFullFieldsShipped : IUpdateFullFieldsShipped
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<BsonDocument> _orderCollection1Insert;
        private string? conn = "";
        private string? dataBase = "";
        private string? orderCollection1 = "";
        private string? orderCollection2 = "";
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<OrderAmazon> _orderShippedCollection;
        private readonly IMongoCollection<OrderAmazon> _orderCollection1;
        private readonly IMongoCollection<Order2Amazon> _orderCollection2;
        private string? orderShippedCollection = "";
        public UpdateFullFieldsShipped(IConfiguration config)
        {
            _config = config;
            conn = _config.GetSection("ConnectionStrings:MongoDB").Value;
            dataBase = _config.GetSection("AmazonLocal:dataBase").Value;
            orderShippedCollection = _config.GetSection("AmazonLocal:orderShippedCollection").Value;
            orderCollection1 = _config.GetSection("AmazonLocal:orderCollection1").Value;
            orderCollection2 = _config.GetSection("AmazonLocal:orderCollection2").Value;

            _client = new MongoClient(conn);

            _database = _client.GetDatabase(dataBase);

            _orderShippedCollection = _database.GetCollection<OrderAmazon>(orderShippedCollection);
            _orderCollection1Insert = _database.GetCollection<BsonDocument>(orderCollection1);
            _orderCollection1 = _database.GetCollection<OrderAmazon>(orderCollection1);
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

            var result = await _orderCollection2.Find(_ => true).ToListAsync();
            return result;
        }
        public async Task GetOrderShipped(AmazonConnection a, List<string> MarketplaceIds, bool _isProduction)
        {
            await Utilities2.WriteLogAsync($"GetOrderShipped Start");
            ParameterOrderList serachOrderList = new ParameterOrderList();
            serachOrderList.CreatedAfter = DateTime.UtcNow.AddMinutes(-21600); // 21600 nửa tháng

            serachOrderList.OrderStatuses = new List<OrderStatuses>();
            serachOrderList.OrderStatuses.Add(OrderStatuses.Shipped);
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
                    objDocument["OrderItemList"] = BsonSerializer.Deserialize<BsonArray>(orderItemList.ToJson());
                    documentList.Add(objDocument);
                }
                if (documentList.Count > 0)
                {
                    try
                    {

                        await _orderCollection1Insert.InsertManyAsync(documentList);
                        await Utilities2.WriteLogAsync($"Save new {documentList.Count()} order shipped");
                    }
                    catch (Exception ex)
                    {

                        await Utilities2.WriteLogAsync($"Save new {documentList.Count()} order shipped ERROR {ex.ToString()}");
                    }
                }
            }
            await Utilities2.WriteLogAsync($"GetOrder shipped DONE");
        }

        public async Task InsertOrderShipped()
        {
            await Utilities2.WriteLogAsync($"Insert Order1 to Order Shipped Processor =====================================================");

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
                await _database.DropCollectionAsync(orderShippedCollection);
                await _orderShippedCollection.InsertManyAsync(orderInsert);
                await Utilities2.WriteLogAsync($"Save new {orderInsert.Count()} Order");
            }
            await Utilities2.WriteLogAsync("Insert Order1,2 to Order Shipped Amazon Processor done");
        }

        public async Task DropCollection()
        {
            await _database.DropCollectionAsync(orderShippedCollection);
            await _database.DropCollectionAsync(orderCollection1);
        }
    }
}
