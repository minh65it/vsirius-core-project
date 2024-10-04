using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using MongoDB.Driver;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller
{
    public interface IOrderDBForStuller
    {
        public Task<List<OrderAmazon>> GetOrderForSubmit();
    }
    public class OrderDBForStuller : IOrderDBForStuller
    {
        private readonly IConfiguration _config;
        private readonly IStullerApiClient _stullerApiClient;
        private string? conn = "";
        private string? dataBaseAmazon = "";
        private string? orderCollectionAmazon = "";
        private readonly IMongoCollection<OrderAmazon> _orderAmazonCollection;

        private string? orderAmazonSubmittedCollection = "";
        private readonly IMongoCollection<QgoldFtpOrderObject> _orderAmazonSubmittedCollection;

        public OrderDBForStuller(IConfiguration config,
            IStullerApiClient stullerApiClient)
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

            _stullerApiClient = stullerApiClient;
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

        public async Task<List<OrderQgold>> GetOrderStullerToAmazon()
        {
            try
            {
                var data = await _stullerApiClient.GetOrder(145);
                if (data != null)
                {
                    data = data.Where(x => x.Status != "Cancelled").ToList();
                    var result = MappingAndReverseStuller.StullerDtoToQgold(data);
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
        public async Task<List<QgoldFtpOrderObject>> GetOrderSubmittedStullerToAmazon()
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
            var dataStuller = await GetOrderStullerToAmazon();

            List<OrderAmazon> listAmazonNotSubmit = new List<OrderAmazon>();

            var dataAmazon = await GetOrderAmazonSeller();
            var dbQgoldSubmitted = await GetOrderSubmittedStullerToAmazon();
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

                    List<OrderQgold> checkQgold = dataStuller.FindAll(x =>
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
    }
}
