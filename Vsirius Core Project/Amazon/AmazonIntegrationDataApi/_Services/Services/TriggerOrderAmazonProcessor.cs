using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetNewOrder;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetOrderShipped;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;
using FikaAmazonAPI;
using FikaAmazonAPI.ConstructFeed.Messages;
using VSiriusSystemLog.WriteLog;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class TriggerOrderAmazonProcessor : ITriggerOrderAmazonProcessor
    {
        private readonly IUpdateFullFields _updateFullFields;
        private readonly IUpdateFullFieldsShipped _updateFullFieldsShipped;
        private readonly IGetOrderDB _getOrderDB;
        private readonly IOrderTracking _orderTracking;
        private readonly IOrderFulfillment _orderFulfillment;
        private readonly IOrderDB _orderDB;
        private readonly IMappingOrder _mappingOrder;
        private readonly IQgoldApiClient _qgoldApiClient;
        private readonly IStullerApiClient _stullerApiClient;
        private readonly IOrderDBForStuller _orderDBForStuller;
        private readonly IMappingOrderStuller _mappingOrderStuller;
        private readonly IConfiguration _config;
        private AmazonConnection _amazonConnection;
        private readonly bool _isProduction = false;
        private ILoggerCustom _logService;

        public TriggerOrderAmazonProcessor(IUpdateFullFields updateFullFields,
            IConfiguration configuration,
            IOrderDB orderDB,
            IMappingOrder mappingOrder,
            IQgoldApiClient qgoldApiClient,
            IGetOrderDB getOrderDB,
            IOrderTracking orderTracking,
            IOrderFulfillment orderFulfillment,
            IUpdateFullFieldsShipped updateFullFieldsShipped,
            IStullerApiClient stullerApiClient,
            IOrderDBForStuller orderDBForStuller,
            IMappingOrderStuller mappingOrderStuller,
            ILoggerCustom logService)
        {
            _updateFullFields = updateFullFields;
            _orderDB = orderDB;
            _mappingOrder = mappingOrder;
            _qgoldApiClient = qgoldApiClient;
            _config = configuration;
            _getOrderDB = getOrderDB;
            _orderTracking = orderTracking;
            _orderFulfillment = orderFulfillment;
            _updateFullFieldsShipped = updateFullFieldsShipped;
            _stullerApiClient = stullerApiClient;
            _orderDBForStuller = orderDBForStuller;
            _mappingOrderStuller = mappingOrderStuller;
            _logService = logService;
            _amazonConnection = new AmazonConnection(new AmazonCredential()
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
                Environment = _config.GetSection("FikaAmazonAPI:Environment").Value == "Production"
                    ? FikaAmazonAPI.Utils.Constants.Environments.Production : FikaAmazonAPI.Utils.Constants.Environments.Sandbox
            });
            _isProduction = _config.GetSection("FikaAmazonAPI:Environment").Value == "Production" ? true : false;
        }

        public async Task<OperationResult> GetNewOrder()
        {
            int retry = 1;
            try
            {
                await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor START //=====================================================");
                await Utilities2.WriteLogAsync(_config.GetSection("FikaAmazonAPI:Environment").Value!);
                //Drop Collection
                await _updateFullFields.DropCollection();
                //Insert Order1
                List<string> marketplaceIds = new List<string>()
                    {
                        _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value!,
                        _config.GetSection("FikaAmazonAPI:MarketPlaceIDCanada").Value!
                    };
                await _updateFullFields.GetOrder(_amazonConnection, marketplaceIds, _isProduction);

                await Utilities2.WriteLogAsync("Get Order AmazonSeller Processor done");
                //Update Order
                await _updateFullFields.InsertOrder();
                await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor DONE //=====================================================");
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Program.Main error: {ex.ToString()}");
                if (retry == 1)
                {
                    await Utilities2.WriteLogAsync($"Program.Main try again!");
                    retry++;
                    await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor START again");
                    //Drop Collection
                    await _updateFullFields.DropCollection();
                    //Insert Order1
                    List<string> marketplaceIds = new List<string>()
                        {
                            _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value!,
                            _config.GetSection("FikaAmazonAPI:MarketPlaceIDCanada").Value!
                        };
                    await _updateFullFields.GetOrder(_amazonConnection, marketplaceIds, _isProduction);

                    await Utilities2.WriteLogAsync("Get Order AmazonSeller Processor done");
                    //Update Order
                    await _updateFullFields.InsertOrder();
                    await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor DONE again");
                    return new OperationResult() { IsSuccess = true };
                }
                await _logService.CreateLogError(ex.Message, "GetNewOrder Amazon");
                return new OperationResult() { IsSuccess = false };
            }
        }

        public async Task<OperationResult> GetOrderShipped()
        {
            int retry = 1;
            try
            {
                await Utilities2.WriteLogAsync($"Get Order Shipped AmazonSeller Processor START //=====================================================");
                //Drop Collection
                await _updateFullFieldsShipped.DropCollection();
                //Insert Order1
                List<string> marketplaceIds = new List<string>()
                    {
                        _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value!,
                        _config.GetSection("FikaAmazonAPI:MarketPlaceIDCanada").Value!
                    };
                await _updateFullFieldsShipped.GetOrderShipped(_amazonConnection, marketplaceIds, _isProduction);

                await Utilities2.WriteLogAsync("Get Order shipped AmazonSeller Processor done");
                //Update Order
                await _updateFullFieldsShipped.InsertOrderShipped();
                await Utilities2.WriteLogAsync($"Get Order shipped AmazonSeller Processor DONE //=====================================================");
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Program.Main error: {ex.ToString()}");
                if (retry == 1)
                {
                    await Utilities2.WriteLogAsync($"Program.Main try again!");
                    retry++;
                    await Utilities2.WriteLogAsync($"Get Order shipped AmazonSeller Processor START again");
                    //Drop Collection
                    await _updateFullFieldsShipped.DropCollection();
                    //Insert Order1
                    List<string> marketplaceIds = new List<string>()
                        {
                            _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value!,
                            _config.GetSection("FikaAmazonAPI:MarketPlaceIDCanada").Value!
                        };
                    await _updateFullFieldsShipped.GetOrderShipped(_amazonConnection, marketplaceIds, _isProduction);

                    await Utilities2.WriteLogAsync("Get Order shipped AmazonSeller Processor done");
                    //Update Order
                    await _updateFullFieldsShipped.InsertOrderShipped();
                    await Utilities2.WriteLogAsync($"Get Order shipped AmazonSeller Processor DONE again");
                    return new OperationResult() { IsSuccess = true };
                }
                return new OperationResult() { IsSuccess = false };
            }
        }

        public async Task<OperationResult> SubmitOrderToQgold()
        {
            await Utilities2.WriteLogAsync($"Submit Order Qgold Processor START =====================================================");
            //Get
            var orderDB = await _orderDB.GetOrderForSubmit();
            await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor done {orderDB?.Count}");
            //Convert Order
            var orders = _mappingOrder.ConvertOrder(orderDB);

            //Call Api Submit
            if (orders.Any())
            {
                var check = false;
                if (_isProduction)
                {
                    check = await _qgoldApiClient.SubmitOrder(orders, false);
                }
                if (check)
                {
                    await Utilities2.WriteLogAsync($"SubmitOrder to API DONE {orders?.Count} //==========================================");
                    //Save Submitted
                    var checkSave = await _orderDB.OrderSubmitted(orders);
                    if (!checkSave)
                    {
                        await Utilities2.WriteLogAsync($"OrderSubmitted not save file {orders?.Count} //==========================================");
                    }
                    return new OperationResult() { IsSuccess = true };
                }
                else
                {
                    await _logService.CreateLogError($"SubmitOrder to API failed {orders?.Count} ", "SubmitOrderToQgold");
                    await Utilities2.WriteLogAsync($"SubmitOrder to API failed {orders?.Count} //==========================================");
                    return new OperationResult() { IsSuccess = false };
                }
            }
            return new OperationResult() { IsSuccess = true };
        }

        public async Task<OperationResult> UpdateTrackingOrder()
        {
            await Utilities2.WriteLogAsync($"Update tracking to amazon START //==========================================");
            List<OrderFulfillmentMessage> data = new List<OrderFulfillmentMessage>();
            List<OrderFulfillmentMessage> dataQ = new List<OrderFulfillmentMessage>();
            List<OrderFulfillmentMessage> dataS = new List<OrderFulfillmentMessage>();
            //=============================get order form Qgold and AmazonSeller from Database local ======================
            try
            {
                dataQ = await _getOrderDB.OrderQgoldFulfillment();
                if (dataQ != null)
                {
                    data.AddRange(dataQ);
                }
            }
            catch (Exception e)
            {
                await Utilities2.WriteLogAsync($"OrderQgoldFulfillment error: {e.ToString()}");
            }
            //=============================get order form Stuller and AmazonSeller from Database local ======================
            try
            {
                dataS = await _getOrderDB.OrderStullerFulfillment();
                //Save Submitted
                await Utilities2.WriteLogAsync($"Stuller has {dataS?.Count} for submit //==========================================");
                if (dataS != null)
                {
                    var checkSave = await _orderDB.SaveSubmitOrderToStullerUIPath(dataS);
                    if (!checkSave)
                    {
                        await Utilities2.WriteLogAsync($"OrderSubmitted Stuller not save file {dataS?.Count} //==========================================");
                    }
                    data.AddRange(dataS);
                }

            }
            catch (Exception e)
            {
                await Utilities2.WriteLogAsync($"OrderStullerFulfillment error: {e.ToString()}");
            }

            //=============================submit order ==================================================
            if (data != null)
            {
                if (_isProduction)
                {
                    _orderFulfillment.FeedPostOrderFulfillment(_amazonConnection, data);
                }
                await Utilities2.WriteLogAsync($"Update {data.Count()} orders");

                //Insert order tracked
                await _orderTracking.InsertOrderTracking(data);

                await Utilities2.WriteLogAsync($"Update tracking to amazon DONE //==========================================");
            }
            else
            {
                await Utilities2.WriteLogAsync($"No data Update tracking to amazon DONE //==========================================");
            }
            return new OperationResult { IsSuccess = true };
        }

        public async Task<OperationResult> SubmitOrderToStuller()
        {
            await Utilities2.WriteLogAsync($"Submit Order Qgold Processor START =====================================================");
            //Get
            var orderDB = await _orderDBForStuller.GetOrderForSubmit();
            await Utilities2.WriteLogAsync($"Get Order AmazonSeller Processor done {orderDB?.Count}");
            //Convert Order
            var orders = _mappingOrderStuller.ConvertOrder(orderDB);
            var ordersSave = _mappingOrderStuller.ConvertOrderSave(orderDB);

            //Call Api Submit
            if (orders.Any())
            {
                var check = false;
                if (_isProduction)
                {
                    foreach (var item in orders)
                    {
                        check = await _stullerApiClient.SubmitOrder(item, false);
                    }
                }
                if (check)
                {
                    await Utilities2.WriteLogAsync($"SubmitOrder to API DONE {orders?.Count} //==========================================");
                    //Save Submitted
                    var checkSave = await _orderDB.OrderSubmitted(ordersSave);
                    if (!checkSave)
                    {
                        await Utilities2.WriteLogAsync($"OrderSubmitted not save file {orders?.Count} //==========================================");
                    }
                    return new OperationResult() { IsSuccess = true };
                }
                else
                {
                    await Utilities2.WriteLogAsync($"SubmitOrder to API failed {orders?.Count} //==========================================");
                    return new OperationResult() { IsSuccess = false };
                }
            }
            return new OperationResult() { IsSuccess = true };
        }
    }
}
