using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using FikaAmazonAPI.ConstructFeed.Messages;
using MongoDB.Driver;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment
{
    public interface IOrderTracking
    {
        public Task<List<OrderTrackingAmazon>> GetOrderTrackingAmazon();
        public Task InsertOrderTracking(List<OrderFulfillmentMessage> orderInput);
    }
    public class OrderTracking : IOrderTracking
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<OrderTrackingAmazon> _orderTrackingCollection;
        private string? conn = "";
        private string? dataBase = "";
        private string? orderTrackingCollection = "";
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        public OrderTracking(IConfiguration config)
        {
            _config = config;
            conn = _config.GetSection("ConnectionStrings:MongoDB").Value;
            dataBase = _config.GetSection("AmazonLocal:dataBase").Value;
            orderTrackingCollection = _config.GetSection("AmazonLocal:orderTrackingCollection").Value;

            _client = new MongoClient(conn);

            _database = _client.GetDatabase(dataBase);

            _orderTrackingCollection = _database.GetCollection<OrderTrackingAmazon>(orderTrackingCollection);
        }
        public async Task InsertOrderTracking(List<OrderFulfillmentMessage> orderInput)
        {
            await Utilities2.WriteLogAsync($"Insert Order Tracking to Order Processor =====================================================");
            try
            {
                if (orderInput.Count > 0)
                {
                    List<OrderTrackingAmazon> orderTrackingAmazons = new List<OrderTrackingAmazon>();
                    foreach (var item in orderInput)
                    {
                        OrderTrackingAmazon orderFulfillmentMessage = new OrderTrackingAmazon()
                        {
                            AmazonOrderID = item.AmazonOrderID,
                            FulfillmentDate = item.FulfillmentDate,
                            FulfillmentData = new FulfillmentData()
                            {
                                CarrierName = item.FulfillmentData.CarrierName,
                                ShippingMethod = item.FulfillmentData.ShippingMethod,
                                ShipperTrackingNumber = item.FulfillmentData.ShipperTrackingNumber
                            }
                        };
                        orderTrackingAmazons.Add(orderFulfillmentMessage);
                    }

                    await _orderTrackingCollection.InsertManyAsync(orderTrackingAmazons);
                    await Utilities2.WriteLogAsync($"Save new {orderTrackingAmazons.Count()} Order");
                }
                await Utilities2.WriteLogAsync("Insert Order Tracking to Order Amazon Processor done");
            }
            catch (Exception ex)
            {

                await Utilities2.WriteLogAsync($"InsertOrderTracking error: {ex.ToString()}");
            }

        }
        public async Task<List<OrderTrackingAmazon>> GetOrderTrackingAmazon()
        {
            try
            {
                var result = await _orderTrackingCollection.Find(_ => true).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"GetOrderTrackingAmazon error: {ex.ToString()}");
                return null;
            }
        }
    }
}
