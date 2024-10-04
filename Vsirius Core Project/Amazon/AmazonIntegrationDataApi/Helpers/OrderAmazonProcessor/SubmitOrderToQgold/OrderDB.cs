using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using MongoDB.Driver;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using FikaAmazonAPI.ConstructFeed.Messages;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold
{
    public interface IOrderDB
    {
        public Task<List<OrderAmazon>> GetOrderForSubmit();
        public Task<bool> OrderSubmitted(List<QgoldFtpOrderObject>? input);
        public Task<bool> SaveSubmitOrderToStullerUIPath(List<OrderFulfillmentMessage> input);
    }
    public class OrderDB : IOrderDB
    {
        private readonly IConfiguration _config;
        private readonly IQgoldApiClient _qgoldApiClient;
        private string? conn = "";
        private string? dataBaseAmazon = "";
        private string? orderCollectionAmazon = "";
        private readonly IMongoCollection<OrderAmazon> _orderAmazonCollection;

        private string? orderAmazonSubmittedCollection = "";
        private readonly IMongoCollection<QgoldFtpOrderObject> _orderAmazonSubmittedCollection;

        public OrderDB(IConfiguration config,
            IQgoldApiClient qgoldApiClient)
        {
            _config = config;
            conn = _config.GetSection("ConnectionStrings:MongoDB").Value;
            var mongoClient = new MongoClient(conn);

            dataBaseAmazon = _config.GetSection("AmazonLocal:dataBase").Value;
            orderCollectionAmazon = _config.GetSection("AmazonLocal:orderCollection").Value;
            var mongoDatabase2 = mongoClient.GetDatabase(dataBaseAmazon);
            _orderAmazonCollection = mongoDatabase2.GetCollection<OrderAmazon>(orderCollectionAmazon);

            orderAmazonSubmittedCollection = _config.GetSection("AmazonLocal:orderSubmittedCollection").Value;
            _orderAmazonSubmittedCollection = mongoDatabase2.GetCollection<QgoldFtpOrderObject>(orderAmazonSubmittedCollection);

            _qgoldApiClient = qgoldApiClient;
        }
        public async Task<List<OrderAmazon>> GetOrderAmazonSeller()
        {
            try
            {
                var PurchaseDate = DateTime.Now.AddDays(-15);
                var filter = Builders<OrderAmazon>.Filter.And(
                Builders<OrderAmazon>.Filter.Gt(x => x.PurchaseDate, PurchaseDate),
                Builders<OrderAmazon>.Filter.Ne(x => x.ShippingAddress.Name, null),
                Builders<OrderAmazon>.Filter.Ne(x => x.ShippingAddress.AddressLine1, null),
                Builders<OrderAmazon>.Filter.Ne(x => x.ShippingAddress.AddressLine2, null)

                );

                var result = await _orderAmazonCollection.Find(filter).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                await Utilities2.WriteLogAsync($"GetOrderQgoldToAmazon error: {ex.ToString()}");
                return new List<OrderAmazon>();
            }
        }

        public async Task<List<OrderQgold>> GetOrderQgoldToAmazon()
        {
            try
            {
                var data = await _qgoldApiClient.GetOrder(45);
                if (data != null)
                {
                    data = data.Where(x => x.OrderStatus != "Cancelled").ToList();
                    var result = MappingAndReverse.QgoldDtoToQgold(data);
                    return result;
                }
                return new List<OrderQgold>();
            }
            catch (Exception ex)
            {

                await Utilities2.WriteLogAsync($"GetOrderQgoldToAmazon error: {ex.ToString()}");
                return new List<OrderQgold>();
            }

        }
        public async Task<List<QgoldFtpOrderObject>> GetOrderSubmittedQgoldToAmazon()
        {
            try
            {
                var db = await _orderAmazonSubmittedCollection.Find(_ => true).ToListAsync();
                if (db == null)
                {
                    db = new List<QgoldFtpOrderObject>();
                }
                return db;
            }
            catch (Exception ex)
            {

                await Utilities2.WriteLogAsync($"GetOrderSubmittedQgoldToAmazon error: {ex.ToString()}");
                return new List<QgoldFtpOrderObject>();
            }

        }

        public async Task<List<OrderAmazon>> GetOrderForSubmit()
        {
            //Get order from Qgold
            var dataQgold = await GetOrderQgoldToAmazon();

            List<OrderAmazon> listAmazonNotSubmit = new List<OrderAmazon>();

            var dataAmazon = await GetOrderAmazonSeller();
            var dbQgoldSubmitted = await GetOrderSubmittedQgoldToAmazon();
            dataAmazon = dataAmazon.Where(x => !x.OrderStatus.Contains("Canceled")).ToList();
            foreach (var iA in dataAmazon)
            {
                try
                {
                    //check Buyer name
                    char NameA = iA.ShippingAddress.Name.ToLower().ToList()[0];
                    char AddressLine1A = iA.ShippingAddress.AddressLine1.ToLower().ToList()[0];
                    bool PC = iA.ShippingAddress.PostalCode.Contains("-");
                    string PostalCode = PC ? iA.ShippingAddress.PostalCode.Split("-")[0] : iA.ShippingAddress.PostalCode;

                    //Kiễm tra trong OrderSubmitted có order này không
                    bool checQgoldSubmitted = dbQgoldSubmitted.Where(x => x.Amz_Order_ID == iA.AmazonOrderId).FirstOrDefault() != null;
                    //Kiểm tra trong Order get từ Qgold về xem đã có đơn này chưa
                    bool checkSKU = false;

                    List<OrderQgold> checkQgold = dataQgold.FindAll(x =>
                        NameA == x.InvoiceDetails.First()?.ShipToText?.ToLower().ToList()[0]
                        && AddressLine1A == x.InvoiceDetails.First()?.ShipToAddress1?.ToLower().ToList()[0]
                        && PostalCode.Contains(x.InvoiceDetails.First().ZipCode)).ToList();

                    //check SKU in list product
                    List<string> checkSKUAmazon = iA.OrderItemList.Select(x => x.SellerSKU).ToList();
                    if (checkSKUAmazon.Any())
                    {
                        string checkSKUAmazonJoin = string.Join(",", checkSKUAmazon);
                        foreach (var iQ in checkQgold)
                        {
                            List<string> checkListSKU = iQ.InvoiceDetails.First().QgoldInvoiceItems.Select(x => x.Style).ToList();
                            if (checkListSKU.Any())
                            {
                                int checkSKUCount = 0;
                                for (int i = 0; i < checkListSKU.Count; i++)
                                {
                                    if (checkSKUAmazonJoin.Contains(checkListSKU[i]) && DateTime.Parse(iQ.Date) >= DateTime.Now.AddDays(-3))
                                        checkSKUCount++;
                                }
                                if (checkSKUCount == checkListSKU.Count)
                                {
                                    checkSKU = true;
                                }

                            }
                        }
                    }
                    //TH1: nếu Order từ Qgold có => không thêm đơn này
                    //TH2: nếu Order từ Qgold ko có và OrderSubmitted có => cũng không thêm đơn này
                    if (checkSKU || checQgoldSubmitted)
                    {
                        listAmazonNotSubmit.Add(iA);
                    }
                }
                catch (Exception ex)
                {
                    await Utilities2.WriteLogAsync($"GetOrderForSubmit error at {iA.AmazonOrderId}: {ex.ToString()}");
                }
            }
            List<OrderAmazon> result = new List<OrderAmazon>();
            result = dataAmazon.Except(listAmazonNotSubmit).ToList().GroupBy(x => x.AmazonOrderId).Select(g => g.First()).ToList();
            return result;
        }
        public async Task<bool> OrderSubmitted(List<QgoldFtpOrderObject>? input)
        {
            try
            {
                if (input == null) return true;
                var db = await _orderAmazonSubmittedCollection.Find(_ => true).ToListAsync();
                List<QgoldFtpOrderObject> inputAdd = new List<QgoldFtpOrderObject>();
                if (db == null)
                {
                    inputAdd.AddRange(input);
                }
                else
                {
                    foreach (var item in input)
                    {

                        if (db.Where(x => x.ShipToName == item.ShipToName
                            && x.ShipToAddress1 == item.ShipToAddress1
                            && x.ShipToZip == item.ShipToZip
                            && x.Amz_Order_ID == item.Amz_Order_ID).FirstOrDefault() == null)
                        {
                            foreach (var i in item.Item)
                            {
                                i.Item = $"Q-{i.Item}";
                            }
                            inputAdd.Add(item);
                        }
                    }
                }
                if (inputAdd.Any())
                {
                    await _orderAmazonSubmittedCollection.InsertManyAsync(inputAdd);
                }
                await Utilities2.WriteLogAsync($"Save new {inputAdd.Count()} Order Qgold");
                return true;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"OrderSubmitted Qgold error: {ex.ToString()}");
                return false;
            }
        }

        public async Task<bool> SaveSubmitOrderToStullerUIPath(List<OrderFulfillmentMessage> input)
        {
            var orderUnSubmit = await _orderAmazonCollection.Find(x => input.Select(x => x.AmazonOrderID).Contains(x.AmazonOrderId)).ToListAsync();
            List<QgoldFtpOrderObject> result = new List<QgoldFtpOrderObject>();
            foreach (var item in orderUnSubmit)
            {
                try
                {
                    QgoldFtpOrderObject qgoldFtpOrderObject = new QgoldFtpOrderObject()
                    {
                        OrderId = null,
                        Format = null,
                        Customer = null,
                        PO = null,
                        ShipToName = item.ShippingAddress.Name,
                        ShipToAddress1 = item.ShippingAddress.AddressLine1,
                        ShipToAddress2 = item.ShippingAddress.AddressLine2,
                        ShipToAddress3 = item.ShippingAddress.AddressLine3,
                        ShipToCity = item.ShippingAddress.City,
                        ShipToState_Province = item.ShippingAddress.StateOrRegion,
                        ShipToZip = item.ShippingAddress.PostalCode,
                        ShipToCountryCode = item.ShippingAddress.CountryCode,
                        ShipToPhone = item.ShippingAddress.Phone,
                        VSiriusItem = null,
                        Description = null,
                        Ship_Method_Code = null,
                        Ship_Option_Code = null,
                        Gift_Message = null,
                        Cust_Order_Number = null,
                        Amz_Order_ID = item.AmazonOrderId,
                        Amz_Item_ID = null,
                        TimeStamp = DateTime.Now,
                        Status = null,
                        Item = ConvertItemOrderToItemSubmit(item.OrderItemList),
                    };
                    result.Add(qgoldFtpOrderObject);
                }
                catch (Exception ex)
                {
                    await Utilities2.WriteLogAsync($"SaveSubmitOrderToStullerUIPath error: {ex.ToString()}");
                }

            }
            if (result.Any())
            {
                await _orderAmazonSubmittedCollection.InsertManyAsync(result);
            }
            await Utilities2.WriteLogAsync($"Save new {result.Count()} Order Stuller");
            return true;
        }
        public static List<QgoldFtpOrderItem> ConvertItemOrderToItemSubmit(List<OrderItemList> input)
        {
            try
            {
                List<QgoldFtpOrderItem> result = new List<QgoldFtpOrderItem>();
                foreach (var i in input)
                {
                    QgoldFtpOrderItem qgoldFtpOrderItem = new QgoldFtpOrderItem()
                    {
                        Item = i.SellerSKU,
                        Qty = i.QuantityOrdered.ToString(),
                        Price = null,
                        Size = null
                    };
                    result.Add(qgoldFtpOrderItem);
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
