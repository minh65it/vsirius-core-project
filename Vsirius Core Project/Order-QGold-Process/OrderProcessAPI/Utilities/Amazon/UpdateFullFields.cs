using FikaAmazonAPI.Parameter.Order;
using FikaAmazonAPI;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi.DTOs.Amazon;
using static FikaAmazonAPI.Utils.Constants;
using MongoDB.Bson.Serialization;

namespace OrderProcessApi.Utilities.Amazon
{
    public interface IUpdateFullFields
    {
        Task InsertNewOrder();
    }

    public class UpdateFullFields : IUpdateFullFields
    {
        IAmazonRepository _amazonDB;
        private readonly IConfiguration _config;

        public UpdateFullFields(IAmazonRepository amazonDB, IConfiguration configuration)
        {
            _amazonDB = amazonDB;
            _config = configuration;
        }

        public async Task<List<OrderAmazon>> GetOrder1()
        {
            var filter = Builders<OrderAmazon>.Filter.And(
            Builders<OrderAmazon>.Filter.Eq(x => x.IsUpdated, false)
            );

            var result = (await _amazonDB.Find(CollectionName.AmazonOrder1, filter)).Data;
            return result;
        }
        public async Task<List<Order2Amazon>> GetOrder2()
        {
            var result = (await _amazonDB.Find<Order2Amazon>(CollectionName.AmazonOrderUI)).Data;
            return result;
        }
        public async Task InsertOrder()
        {
            await Common.WriteLogAsync($"Insert Order Amazon", $"Insert_Order_Amazon_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
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
                await _amazonDB.DropCollection(CollectionName.AmazonOrder);
                await _amazonDB.InsertMany(orderInsert, CollectionName.AmazonOrder);
                await Common.WriteLogAsync($"Save new {orderInsert.Count()} Order", $"Save_new_{orderInsert.Count()}_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
            }
            await Common.WriteLogAsync($"Insert Order1,2 to Order Amazon Processor done", $"Insert_Order12_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
        }
        private async Task InsertOrder1(AmazonConnection a)
        {
            ParameterOrderList serachOrderList = new ParameterOrderList();
            serachOrderList.CreatedAfter = DateTime.UtcNow.AddMinutes(-21600); // 21600 nửa tháng

            serachOrderList.OrderStatuses = new List<OrderStatuses>();
            serachOrderList.OrderStatuses.Add(OrderStatuses.Unshipped);

            serachOrderList.AmazonOrderIds = new List<string>();

            FikaAmazonAPI.AmazonSpApiSDK.Models.Orders.OrderList orders = a.Orders.GetOrders(serachOrderList);
            if (orders.Count > 0)
            {
                var documentList = new List<BsonDocument>();
                foreach (var obj in orders)
                {
                    var orderItemList = a.Orders.GetOrderItems(obj.AmazonOrderId);
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
                        await _amazonDB.InsertMany(documentList, CollectionName.AmazonOrder1);
                        await Common.WriteLogAsync($"Save new {documentList.Count()} order1", $"Insert_Order1_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                    }
                    catch (Exception ex)
                    {
                        await Common.WriteLogAsync($"Save new {documentList.Count()} order ERROR {ex.ToString()}", $"Insert_Order1_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                    }
                }
            }
            await Common.WriteLogAsync($"GetOrder DONE", $"GetOrder_DONE{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
        }
        public async Task InsertNewOrder()
        {
            await _amazonDB.DropCollection(CollectionName.AmazonOrder1);
            await _amazonDB.DropCollection(CollectionName.AmazonOrder);

            var amazonConnection = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = _config.GetSection("FikaAmazonAPI:AccessKey").Value,
                SecretKey = _config.GetSection("FikaAmazonAPI:SecretKey").Value,
                RoleArn = _config.GetSection("FikaAmazonAPI:RoleArn").Value,
                ClientId = _config.GetSection("FikaAmazonAPI:ClientId").Value,
                ClientSecret = _config.GetSection("FikaAmazonAPI:ClientSecret").Value,
                RefreshToken = _config.GetSection("FikaAmazonAPI:RefreshToken").Value,
                MarketPlaceID = _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value,
                SellerID = _config.GetSection("FikaAmazonAPI:SellerId").Value,
                IsDebugMode = true,
                //Environment = Environments.Sandbox
            });
            var amazonConnectionCA = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = _config.GetSection("FikaAmazonAPI:AccessKey").Value,
                SecretKey = _config.GetSection("FikaAmazonAPI:SecretKey").Value,
                RoleArn = _config.GetSection("FikaAmazonAPI:RoleArn").Value,
                ClientId = _config.GetSection("FikaAmazonAPI:ClientId").Value,
                ClientSecret = _config.GetSection("FikaAmazonAPI:ClientSecret").Value,
                RefreshToken = _config.GetSection("FikaAmazonAPI:RefreshToken").Value,
                MarketPlaceID = _config.GetSection("FikaAmazonAPI:MarketPlaceIDCanada").Value,
                SellerID = _config.GetSection("FikaAmazonAPI:SellerId").Value,
                IsDebugMode = true,
                //Environment = Environments.Sandbox
            });
            int retry = 1;
            try
            {
                await InsertOrder1(amazonConnection);
                await InsertOrder1(amazonConnectionCA);
                //
                await InsertOrder();
            }
            catch (Exception ex)
            {
                if (retry == 1)
                {
                    retry++;
                    await InsertOrder1(amazonConnection);
                    await InsertOrder1(amazonConnectionCA);
                    //
                    await InsertOrder();
                }
                await Common.WriteLogAsync($"Insert New Order ERROR {ex.ToString()}", $"Insert_New_Order{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
            }
            
        }
    }
}
