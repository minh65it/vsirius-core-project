using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using FikaAmazonAPI.ConstructFeed.Messages;
using MongoDB.Driver;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment
{
    public interface IGetOrderDB
    {
        public Task<List<OrderFulfillmentMessage>> OrderQgoldFulfillment();
        public Task<List<OrderFulfillmentMessage>> OrderStullerFulfillment();
    }
    public class GetOrderDB : IGetOrderDB
    {
        private readonly IConfiguration _config;
        private readonly IOrderTracking _orderTracking;
        private readonly IQgoldApiClient _qgoldApiClient;
        private readonly IStullerClientApi _stullerClientApi;
        private string? conn = "";

        private string? dataBaseAmazon = "";
        private string? orderCollectionAmazon = "";
        private readonly IMongoCollection<OrderAmazon> _orderAmazonCollection;
        public GetOrderDB(IConfiguration configuration,
            IOrderTracking orderTracking,
            IQgoldApiClient qgoldApiClient,
            IStullerClientApi stullerClientApi)
        {

            _config = configuration;
            _orderTracking = orderTracking;
            _qgoldApiClient = qgoldApiClient;

            conn = _config.GetSection("ConnectionStrings:MongoDB").Value;
            var mongoClient = new MongoClient(conn);

            dataBaseAmazon = _config.GetSection("AmazonLocal:dataBase").Value;
            orderCollectionAmazon = _config.GetSection("AmazonLocal:orderCollection").Value;
            var mongoDatabase = mongoClient.GetDatabase(dataBaseAmazon);
            _orderAmazonCollection = mongoDatabase.GetCollection<OrderAmazon>(orderCollectionAmazon);
            _stullerClientApi = stullerClientApi;
        }

        public async Task<List<OrderQgold>> GetOrderQgoldToAmazon()
        {
            try
            {
                var data = await _qgoldApiClient.GetOrder(45);
                if (data != null)
                {
                    data = data.Where(x => x.OrderStatus == "Invoiced").ToList();
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
        public async Task<List<StullerOrderDto>> GetOrderStullerToAmazon()
        {
            AmazonProcessSearchParam? paramSearch = new AmazonProcessSearchParam();
            paramSearch.FromDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            paramSearch.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
            var data = await _stullerClientApi.GetOrderMarketplace(paramSearch);
            return data;
        }
        public async Task<List<OrderAmazon>> GetOrderAmazonSeller()
        {
            try
            {
                var PurchaseDate = DateTime.Now.AddDays(-15);
                var filter = Builders<OrderAmazon>.Filter.And(
                Builders<OrderAmazon>.Filter.Gt(x => x.PurchaseDate, PurchaseDate),
                Builders<OrderAmazon>.Filter.Ne(x => x.ShippingAddress.Name, null),
                Builders<OrderAmazon>.Filter.Ne(x => x.ShippingAddress.AddressLine1, null)
                );

                var result = await _orderAmazonCollection.Find(filter).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                await Utilities2.WriteLogAsync($"GetOrderAmazonSeller error: {ex.ToString()}");
                return null;
            }
        }
        public async Task<List<OrderFulfillmentMessage>> OrderQgoldFulfillment()
        {
            //Get order from Qgold
            var dataQgold = await GetOrderQgoldToAmazon();
            if (dataQgold is null) return null;
            //Get order tracked
            var dataTracked = await _orderTracking.GetOrderTrackingAmazon();

            List<OrderFulfillmentMessage> resultTmp = new List<OrderFulfillmentMessage>();
            var dataAmazon = await GetOrderAmazonSeller();
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
                                    if (checkSKUAmazonJoin.Contains(checkListSKU[i]) && DateTime.Parse(iQ.Date) >= DateTime.Now.AddDays(-10))
                                        checkSKUCount++;
                                }

                                if (checkSKUCount == checkSKUAmazon.Count)
                                {
                                    string trackingID = iQ.InvoiceDetails.First().Tracking;
                                    if (trackingID != null && iQ.InvoiceDetails.First().ShipVia != null
                                            && dataTracked.Any(x => x.FulfillmentData.ShipperTrackingNumber == trackingID) == false)
                                    {
                                        string ShippingMethod = iQ.InvoiceDetails.First().ShipVia;
                                        string CarrierName = ShippingMethod.Split(" ")[0];
                                        if (ShippingMethod.Contains("CaPost"))
                                        {
                                            CarrierName = "CanadaPost";
                                        }
                                        var fulfillmentDate = DateTime.Parse(iQ.Date).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
                                        if (iA.PurchaseDate.Date > DateTime.Parse(iQ.Date).Date)
                                        {
                                            fulfillmentDate = DateTime.Parse(iQ.Date).AddDays(1).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
                                        }
                                        OrderFulfillmentMessage orderFulfillmentMessage = new OrderFulfillmentMessage()
                                        {
                                            AmazonOrderID = iA.AmazonOrderId,
                                            FulfillmentDate = fulfillmentDate,
                                            FulfillmentData = new FulfillmentData()
                                            {
                                                CarrierName = CarrierName,
                                                ShippingMethod = ShippingMethod,
                                                ShipperTrackingNumber = trackingID
                                            }
                                        };
                                        resultTmp.Add(orderFulfillmentMessage);
                                        dataQgold.Remove(iQ);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Utilities2.WriteLogAsync($"GetOrderQgoldToAmazon error: {ex.ToString()}");
                }
            }
            List<OrderFulfillmentMessage> result = resultTmp.GroupBy(x => x.FulfillmentData.ShipperTrackingNumber).Select(g => g.First()).ToList();
            return result;
        }
        public async Task<List<OrderFulfillmentMessage>> OrderStullerFulfillment()
        {
            //Get order from Stuller
            var dataStuller = await GetOrderStullerToAmazon();
            await Utilities2.WriteLogAsync($"Get Order Stuller To Amazon has {dataStuller?.Count} for submit //==========================================");

            if (dataStuller is null) return null;
            //Get order tracked
            var dataTracked = await _orderTracking.GetOrderTrackingAmazon();

            List<OrderFulfillmentMessage> resultTmp = new List<OrderFulfillmentMessage>();
            var dataAmazon = await GetOrderAmazonSeller();
            dataAmazon = dataAmazon.Where(x => !x.OrderStatus.Contains("Canceled")).ToList();
            foreach (var iA in dataAmazon)
            {
                try
                {
                    var checkStuller = dataStuller.FirstOrDefault(x => x.PO == iA.AmazonOrderId);
                    var checkStullerTracked = dataTracked.FirstOrDefault(x => x.AmazonOrderID == iA.AmazonOrderId);
                    if (checkStuller is not null && checkStullerTracked is null)
                    {
                        OrderFulfillmentMessage orderFulfillmentMessage = new OrderFulfillmentMessage()
                        {
                            AmazonOrderID = iA.AmazonOrderId,
                            FulfillmentDate = DateTime.Parse(checkStuller.ShipDate).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"),
                            FulfillmentData = new FulfillmentData()
                            {
                                CarrierName = checkStuller.Carrier.Contains("USPS") ? "USPS" : checkStuller.Carrier,
                                ShippingMethod = checkStuller.ShippingMethod.Contains("USPS_1ST") ? "USPS Frst Class" : "USPS Priority",
                                ShipperTrackingNumber = checkStuller.Tracking,
                            }
                        };
                        if (!orderFulfillmentMessage.FulfillmentDate.Contains("0001-01-01"))
                            resultTmp.Add(orderFulfillmentMessage);
                    }
                }
                catch (Exception ex)
                {
                    await Utilities2.WriteLogAsync($"GetOrderQgoldToAmazon error: {ex.ToString()}");
                }
            }
            List<OrderFulfillmentMessage> result = resultTmp.GroupBy(x => x.FulfillmentData.ShipperTrackingNumber).Select(g => g.First()).ToList();
            return result;
        }
    }
}
