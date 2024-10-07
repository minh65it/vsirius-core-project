using EFCore.BulkExtensions;
using MongoDB.Driver;
using PimApi._Repositories.Interfaces;
using PimApi._Services.Interfaces;
using PimApi.Context;
using PimApi.Helpers.Utilities;
using PimApi.Models;
using PimApi.Models.ETL;
using VSiriusSystemLog.WriteLog;

namespace PimApi._Services.Services
{
    public class ETLOrderService : IETLOrderService
    {
        private readonly IAmazonMongoRepository _amazonMongoRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IWalmartMongoRepository _walmartMongoRepository;
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _configuration;
        private string _amazonOrderSubmitted = "";
        private string _amazonOrderRetun = "";
        private string _walmartOrder = "";
        private string _walmartOrderReturn = "";
        private readonly DataContext _context;

        public ETLOrderService(IAmazonMongoRepository amazonMongoRepository,
            IOrderRepository orderRepository,
            IWalmartMongoRepository walmartMongoRepository,
            IConfiguration configuration,
            DataContext context,
            ILoggerCustom logService)
        {
            _amazonMongoRepository = amazonMongoRepository;
            _orderRepository = orderRepository;
            _walmartMongoRepository = walmartMongoRepository;
            _configuration = configuration;
            _amazonOrderSubmitted = _configuration.GetSection("Amazon:OrderSubmitted").Value!;
            _amazonOrderRetun = _configuration.GetSection("Amazon:ReturnOrder").Value!;
            _walmartOrder = _configuration.GetSection("Walmart:Order").Value!;
            _walmartOrderReturn = _configuration.GetSection("Walmart:ReturnOrder").Value!;
            _context = context;
            _logService = logService;
        }

        public async Task<OperationResult> ConvertOrder()
        {
            try
            {
                //Map to SQL
                List<Order> lstOrders = new List<Order>();
                DateTime now = DateTime.Now;

                //Amazon
                FilterDefinition<OrderAmazonSubmitted>? filter = null;
                var lstSubmit = await _amazonMongoRepository.FindAll(_amazonOrderSubmitted, filter);
                foreach (var item in lstSubmit)
                {
                    if (item.Item is not null && item.Item.Count > 0)
                    {
                        foreach (var i in item.Item)
                        {
                            if (!string.IsNullOrWhiteSpace(i.Item))
                                lstOrders.Add(new Order()
                                {
                                    OrderId = item.Amz_Order_ID,
                                    ItemSku = i.Item,
                                    ShipToName = item.ShipToName,
                                    ShipToPhone = item.ShipToPhone,
                                    ShipToAddress = item.ShipToAddress1,
                                    ShipToZip = item.ShipToZip,
                                    MethodCode = item.Ship_Method_Code,
                                    TrackingId = null,
                                    OrderQuantity = int.Parse(i.Qty),
                                    ReturnQuantity = 0,
                                    Marketplace = "Amazon",
                                    CreatedDate = now,
                                });
                        }
                    }
                }

                //Amazon Return
                FilterDefinition<OrderAmazonReturn>? filterReturn = null;
                var lstAmazonReturn = await _amazonMongoRepository.FindAll(_amazonOrderRetun, filterReturn);
                foreach (var item in lstAmazonReturn)
                {
                    if (!string.IsNullOrWhiteSpace(item.MerchantSKU))
                        lstOrders.Add(new Order()
                        {
                            OrderId = item.AmazonRMAID,
                            ItemSku = item.MerchantSKU,
                            ShipToName = null,
                            ShipToPhone = null,
                            ShipToAddress = null,
                            ShipToZip = null,
                            MethodCode = null,
                            TrackingId = item.TrackingID,
                            OrderQuantity = 0,
                            ReturnQuantity = item.ReturnQuantity,
                            Marketplace = "Amazon",
                            CreatedDate = now,
                        });
                }

                //Walmart
                FilterDefinition<OrderWalmart>? filterWalmart = null;
                var lstOrder = await _walmartMongoRepository.FindAll(_walmartOrder, filterWalmart);
                List<Order> lstOrdersWalmart = new List<Order>();
                foreach (var item in lstOrder)
                {
                    if (item.orderLines is not null)
                    {
                        if (item.orderLines.orderLine is not null && item.orderLines.orderLine.Count > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(item.orderLines.orderLine[0].item.sku))
                            lstOrdersWalmart.Add(new Order()
                            {
                                OrderId = item.purchaseOrderId,
                                ItemSku = item.orderLines.orderLine[0].item.sku,
                                ShipToName = item.shippingInfo.postalAddress.name,
                                ShipToPhone = item.shippingInfo.phone,
                                ShipToAddress = item.shippingInfo.postalAddress.address1,
                                ShipToZip = item.shippingInfo.postalAddress.postalCode,
                                MethodCode = item.shippingInfo.methodCode,
                                TrackingId = null,
                                OrderQuantity = item.orderLines.orderLine.Count,
                                ReturnQuantity = 0,
                                Marketplace = "Walmart",
                                CreatedDate = now,
                            });
                        }
                    }
                }
                foreach (var order in lstOrdersWalmart)
                {
                    if (order.ItemSku.StartsWith("QU-"))
                    {
                        var itemSku = order.ItemSku[2..];
                        order.ItemSku = "Q" + itemSku;
                    }
                }
                lstOrders.AddRange(lstOrdersWalmart);

                //Walmart Return
                FilterDefinition<OrderWalmartReturn>? filterReturnWalmart = null;
                var lstOrderWalmartReturn = await _walmartMongoRepository.FindAll(_walmartOrderReturn, filterReturnWalmart);
                List<Order> lstOrdersReturnWalmart = new List<Order>();
                foreach (var item in lstOrderWalmartReturn)
                {
                    if (item.returnOrderLines is not null && item.returnOrderLines.Count > 0)
                    {
                        foreach (var i in item.returnOrderLines)
                        {
                            if (!string.IsNullOrWhiteSpace(i.item.sku))
                                lstOrdersReturnWalmart.Add(new Order()
                                {
                                    OrderId = item.returnOrderId,
                                    ItemSku = i.item.sku,
                                    ShipToName = null,
                                    ShipToPhone = null,
                                    ShipToAddress = null,
                                    ShipToZip = null,
                                    MethodCode = null,
                                    TrackingId = null,
                                    OrderQuantity = 0,
                                    ReturnQuantity = (int)decimal.Parse(i.refundedQty),
                                    Marketplace = "Walmart",
                                    CreatedDate = now,
                                });
                        }
                    }
                }
                foreach (var order in lstOrdersReturnWalmart)
                {
                    if (order.ItemSku.StartsWith("QU-"))
                    {
                        var itemSku = order.ItemSku[2..];
                        order.ItemSku = "Q" + itemSku;
                    }
                }
                lstOrders.AddRange(lstOrdersReturnWalmart);
                //Add Group by
                var groupedOrders = lstOrders
                    .GroupBy(o => new { o.OrderId, o.ItemSku, o.Marketplace })
                    .Select(g => new Order()
                    {
                        OrderId = g.Key.OrderId,
                        ItemSku = g.Key.ItemSku,
                        ShipToName = g.FirstOrDefault()?.ShipToName,
                        ShipToPhone = g.FirstOrDefault()?.ShipToPhone,
                        ShipToAddress = g.FirstOrDefault()?.ShipToAddress,
                        ShipToZip = g.FirstOrDefault()?.ShipToZip,
                        MethodCode = g.FirstOrDefault()?.MethodCode,
                        TrackingId = g.FirstOrDefault()?.TrackingId,
                        OrderQuantity = g.FirstOrDefault()!.OrderQuantity,
                        ReturnQuantity = g.FirstOrDefault()!.ReturnQuantity,
                        Marketplace = g.Key.Marketplace,
                        CreatedDate = now,
                    }).ToList();
                try
                {
                    await _context.BulkInsertOrUpdateAsync(groupedOrders);
                }
                catch (Exception)
                {
                    throw;
                }
                return new OperationResult() { Success = true };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogInformation($"ConvertOrder Trigger Error: {ex.Message}");
                return new OperationResult(false);
            }
        }
    }
}
