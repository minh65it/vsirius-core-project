using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi.Models.Temp;
using WalmartIntegrationDataApi.Models;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using MongoDB.Driver;
using WalmartIntegrationDataApi.Dtos.Order;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.CollectionMongo;
using System.Text;
using Newtonsoft.Json;
using WalmartIntegrationDataApi.Common.Helpers;
using AutoMapper;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class TrackingWalmartOrderService : ITrackingWalmartOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderWalmartRepository _orderWalmartRepository;
        private readonly IInteractWalmartSellerCommonService _interactWalmartSellerService;
        private readonly IOrderQgoldRepository _orderQgoldRepository;
        private readonly StullerOrderProcessAPI._Services.Interfaces.IOrderStullerService _orderStullerService;
        private readonly IMapper _mapper;

        public TrackingWalmartOrderService(
            IOrderWalmartRepository orderWalmartRepository,
            IInteractWalmartSellerCommonService interactWalmartSellerService,
            IConfiguration configuration,
            IOrderQgoldRepository orderQgoldRepository,
            StullerOrderProcessAPI._Services.Interfaces.IOrderStullerService orderStullerService,
            IMapper mapper)
        {
            _orderWalmartRepository = orderWalmartRepository;
            _interactWalmartSellerService = interactWalmartSellerService;
            _configuration = configuration;
            _orderQgoldRepository = orderQgoldRepository;
            _orderStullerService = orderStullerService;
            _mapper = mapper;
        }

        public async Task UpdateTrackingWalmartOrder()
        {
            try
            {
                Util.WriteLog("---------------------------Update Tracking Process Start-------------------------------------");

                Util.WriteLog("Get Order From Walmart DB Start");
                List<Order> lstOrderWalmart = await GetNewWalmartOrder();
                //lst OrderCustomerID có thể có 2 PO 
                List<CustomerOrderID> lstCustomerOrderID = lstOrderWalmart
                    .GroupBy(x => x.customerOrderId)
                    .Select(x => new CustomerOrderID()
                    {
                        customerOrderId = x.Key,
                        amountPO = x.Count()
                    }).ToList();
                Util.WriteLog("Get Order From Walmart DB in End");

                Util.WriteLog("Get Order From Qgold DB in 14 days Start");
                await ConfirmOrder(lstOrderWalmart);
                Util.WriteLog("Get Order From Qgold DB in 14 days End");

                Util.WriteLog("Get Order From Stuller DB in 14 days Start");
                List<OrderStullerDTO> lstOrderStuller = await GetStullerOrder();
                Util.WriteLog("Get Order From Stuller DB in 14 days End");

                Util.WriteLog("Get Order From Qgold DB in 7 days Start");
                List<OrderQgold> lstOrderQgold = await GetQgoldOrder();
                Util.WriteLog("Get Order From Qgold DB in 7 days End");

                Util.WriteLog("Filter lstUpdate Start");
                List<UpdateStatusOrder> lstUpdate = new();
                //lay order can update tu qgold
                foreach (var qgold in lstOrderQgold)
                {
                    if (string.IsNullOrWhiteSpace(qgold.InvoiceDetails[0].ShipToText))
                    {
                        Util.WriteLog($"Qgold Order UIPath error when get Buyer Name Order ID: {qgold.Order}");
                        continue;
                    }

                    var isSaveTracking = false;
                    foreach (var customerOrderID in lstCustomerOrderID)
                    {
                        var lstPOWalmart = lstOrderWalmart.Where(x => x.customerOrderId == customerOrderID.customerOrderId).ToList();
                        var firstPO = lstPOWalmart.FirstOrDefault();
                        if (firstPO!.shippingInfo.postalAddress.name
                            .StartsWith(qgold.InvoiceDetails[0].ShipToText.TrimStart()[0].ToString(), StringComparison.OrdinalIgnoreCase)

                            && firstPO.shippingInfo.postalAddress.address1
                            .StartsWith(qgold.InvoiceDetails[0].ShipToAddress1.TrimStart()[0].ToString(), StringComparison.OrdinalIgnoreCase)

                            && firstPO.shippingInfo.postalAddress.postalCode
                            .Contains(qgold.InvoiceDetails[0].ZipCode.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var walmartPO in lstPOWalmart)
                            {
                                var lstSkuItemInWalmart = walmartPO.orderLines.orderLine.Where(x => x.item.sku.StartsWith("Q"))
                                    .Select(x => Util.SKUQgold(x.item.sku)).ToList();                              
                                if (qgold.InvoiceDetails[0].QgoldInvoiceItems.Any(x => lstSkuItemInWalmart.Contains(x.Style)))
                                {
                                    isSaveTracking = true;
                                    var obj = new UpdateStatusOrder
                                    {
                                        purchaseOrderId = walmartPO.purchaseOrderId,
                                        orderShipment = new OrderShipment
                                        {
                                            orderLines = new UOrderLines()
                                        }
                                    };
                                    obj.orderShipment.orderLines.orderLine = new List<UOrderLine>();

                                    foreach (var item in walmartPO.orderLines.orderLine)
                                    {
                                        var uOrderLine = new UOrderLine()
                                        {
                                            lineNumber = item.lineNumber,
                                            orderLineStatuses = new UOrderLineStatuses()
                                            {
                                                orderLineStatus = new List<UOrderLineStatus> { }
                                            }
                                        };

                                        foreach (var line in item.orderLineStatuses.orderLineStatus)
                                        {
                                            uOrderLine.orderLineStatuses.orderLineStatus
                                            .Add(new UOrderLineStatus()
                                            {
                                                status = "Shipped",
                                                statusQuantity = new UStatusQuantity()
                                                {
                                                    unitOfMeasurement = line.statusQuantity.unitOfMeasurement,
                                                    amount = line.statusQuantity.amount
                                                },
                                                trackingInfo = new UTrackingInfo()
                                                {
                                                    shipDateTime = Util.ConvertDateTimeToEpoch(qgold.Date),
                                                    carrierName = new UCarrierName()
                                                    {
                                                        carrier = qgold.InvoiceDetails[0].ShipVia.Trim().Split(' ')[0]
                                                        //otherCarrier = qgold.InvoiceDetails[0].ShipVia
                                                    },
                                                    methodCode = "Standard",
                                                    trackingNumber = qgold.InvoiceDetails[0].Tracking,
                                                }
                                            });
                                        }

                                        obj.orderShipment.orderLines.orderLine.Add(uOrderLine);
                                    }
                                    lstUpdate.Add(obj);
                                }

                                
                            }
                        }
                        if (isSaveTracking) { await SaveTrackingOrder(qgold); break; }
                    }
                }

                //lay order can update tu stuller               
                foreach (var stuller in lstOrderStuller)
                {
                    try
                    {
                        var isSaveTracking = false;
                        var lstPOWalmart = lstOrderWalmart.Where(x => x.customerOrderId == stuller.PO).ToList();
                        foreach (var walmartPO in lstPOWalmart)
                        {
                            var lstSkuItemInWalmart = walmartPO.orderLines.orderLine.Where(x => x.item.sku.StartsWith("S"))
                                        .Select(x => x.item.sku[2..]).ToList();
                            if(lstSkuItemInWalmart.Any()) 
                            {
                                if (stuller.Items.Any(x => lstSkuItemInWalmart.Contains(x.ProductSku)))
                                {
                                    if (!string.IsNullOrWhiteSpace(stuller.Tracking))
                                    {
                                        isSaveTracking = true;
                                        var obj = new UpdateStatusOrder
                                        {
                                            purchaseOrderId = walmartPO.purchaseOrderId,
                                            orderShipment = new OrderShipment
                                            {
                                                orderLines = new UOrderLines()
                                            }
                                        };
                                        obj.orderShipment.orderLines.orderLine = new List<UOrderLine>();

                                        foreach (var item in walmartPO.orderLines.orderLine)
                                        {
                                            var uOrderLine = new UOrderLine()
                                            {
                                                lineNumber = item.lineNumber,
                                                orderLineStatuses = new UOrderLineStatuses()
                                                {
                                                    orderLineStatus = new List<UOrderLineStatus> { }
                                                }
                                            };

                                            foreach (var line in item.orderLineStatuses.orderLineStatus)
                                            {
                                                uOrderLine.orderLineStatuses.orderLineStatus
                                                .Add(new UOrderLineStatus()
                                                {
                                                    status = "Shipped",
                                                    statusQuantity = new UStatusQuantity()
                                                    {
                                                        unitOfMeasurement = line.statusQuantity.unitOfMeasurement,
                                                        amount = line.statusQuantity.amount
                                                    },
                                                    trackingInfo = new UTrackingInfo()
                                                    {
                                                        shipDateTime = Util.ConvertDateTimeToEpoch(DateTime.Now.ToString()),
                                                        carrierName = new UCarrierName()
                                                        {
                                                            carrier = Util.GetCarrier(stuller.Carrier)
                                                            //otherCarrier = qgold.InvoiceDetails[0].ShipVia
                                                        },
                                                        methodCode = "Standard",
                                                        trackingNumber = stuller.Tracking,
                                                    }
                                                });
                                            }

                                            obj.orderShipment.orderLines.orderLine.Add(uOrderLine);
                                        }
                                        lstUpdate.Add(obj);
                                    }                                   
                                }
                            }
                            else
                            {
                                continue;
                            }                         
                        }
                        if (isSaveTracking) { await SaveTrackingOrder(stuller); }
                    }
                    catch (Exception ex)
                    {
                        Util.WriteLog($"Err update stuller. Exception: {ex.Message}");
                    }
                }

                Util.WriteLog("Filter lstUpdate End");

                Util.WriteLog("Call Api Seller To Update Status Start");
                await CallApiSellerUpdate(lstUpdate);
                Util.WriteLog("Call Api Seller To Update Status End");

                Util.WriteLog("---------------------------Update Tracking Process End-------------------------------------");
                lstOrderWalmart = null;
                lstCustomerOrderID = null;
                lstOrderQgold = null;
                lstUpdate = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Update tracking failed. Err:{ex.Message}");
            }
        }

        private async Task SaveTrackingOrder(OrderStullerDTO stuller)
        {
            var obj = new 
            {
                OrderStullerId = stuller.OrderID ?? "",
                UpdatedDate = DateTime.Now,
            };
            await _orderWalmartRepository.InsertOne(obj, "TrackingStullerOrder");
        }

        private async Task<List<OrderStullerDTO>> GetStullerOrder()
        {
            var result = new List<OrderStullerDTO>();
            try
            {
                PaginationUtility<OrderStullerDTO>? firstGet = await CallApiToGetOrderStuller_RFT(1, 1000);
                
                if (firstGet != null)
                {
                    result.AddRange(firstGet.Result);

                    for (int pageNumber = 2; pageNumber <= firstGet.Pagination.TotalPage; pageNumber++)
                    {
                        var data = await CallApiToGetOrderStuller_RFT(pageNumber, 1000);
                        result.AddRange(data!.Result);
                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"GetStullerOrder failed. Exception: {ex.Message}");
            }
            return result;
        }

        private async Task<PaginationUtility<OrderStullerDTO>?> CallApiToGetOrderStuller(int pageNumber, int pageSize)
        {
            try
            {
                var url = $"{_configuration.GetSection("StullerAPI:GetOrders").Value}?PageNumber={pageNumber}&PageSize={pageSize}";
                using HttpClient client = new();

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PaginationUtility<OrderStullerDTO>>(responseBody)!;
                }
                else
                {
                    Util.WriteLog($"CallApiToGetOrderStuller failed. Error code: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"CallApiToGetOrderStuller failed. Exception: {ex.Message}");
            }

            return null;
        }

        private async Task<PaginationUtility<OrderStullerDTO>?> CallApiToGetOrderStuller_RFT(int pageNumber, int pageSize)
        {
            PaginationUtility<OrderStullerDTO> result = new PaginationUtility<OrderStullerDTO>(null, 1, 1, 1, 1);
            try
            {
                StullerOrderProcessAPI.Common.Helpers.PaginationParam pagination = new StullerOrderProcessAPI.Common.Helpers.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;
                var data = await _orderStullerService.GetOrders(pagination, new StullerOrderProcessAPI.Dtos.SearchOrderStullerParam());

                result.Result = new List<OrderStullerDTO>();
                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.Skip = data.Pagination.Skip;
                foreach (var item in data.Result)
                {
                    OrderStullerDTO dto = _mapper.Map<OrderStullerDTO>(item);
                    result.Result.Add(dto);
                }
                return result;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"CallApiToGetOrderStuller failed. Exception: {ex.Message}");
                return result;
            }
            

        }

        private async Task CallApiSellerUpdate(List<UpdateStatusOrder> lstUpdate)
        {
            if (lstUpdate.Count > 0)
            {
                var tokenResult = await _interactWalmartSellerService.GetTokenFromWalmartSeller();

                if (!string.IsNullOrEmpty(tokenResult.AccessToken))
                {
                    foreach (var item in lstUpdate)
                    {
                        var BASE_URL = $"{_configuration.GetSection("Walmart:UpdateOrder").Value}";
                        var url = BASE_URL.Replace("purchaseOrderId", item.purchaseOrderId);
                        using HttpClient client = new();
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                        client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                        client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");

                        var contentModel = new UpdateStatusOrderApi()
                        {
                            orderShipment = item.orderShipment
                        };

                        HttpContent content = new StringContent(JsonConvert.SerializeObject(contentModel), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(url, content);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        Util.WriteLog($"Update Result {responseBody}");
                    }
                }
            }
            else
            {
                Util.WriteLog("No order need update tracking id");
            }
        }

        private async Task SaveTrackingOrder(OrderQgold qgold)
        {
            var obj = new TrackingQgoldOrder()
            {
                OrderQgoldId = qgold.Order,
                UpdatedDate = DateTime.Now,
            };
            await _orderQgoldRepository.InsertOne(obj, "TrackingOrder");
        }

        private async Task<List<OrderQgold>> GetQgoldOrder()
        {
            List<OrderQgold> lstOrder;

            FilterDefinition<OrderQgold>? filter;
            var fromDate = DateTime.Now.AddDays(-5);

            List<string> lst14PastDays = new();

            for (var i = DateTime.Now; i >= fromDate; i = i.AddDays(-1))
            {
                lst14PastDays.Add(i.ToString("M/d/yyyy"));
            }
            filter = Builders<OrderQgold>.Filter.Where(x => lst14PastDays.Contains(x.Date));

            lstOrder = await _orderQgoldRepository.FindAll(CollectionName.OrderQgold, filter);

            return lstOrder;
        }

        private async Task ConfirmOrder(List<Order> lstOrderWalmart)
        {
            var tokenResult = await _interactWalmartSellerService.GetTokenFromWalmartSeller();

            foreach (var item in lstOrderWalmart)
            {
                try
                {
                    if (item.orderLines.orderLine[0].orderLineStatuses.orderLineStatus[0].status.Equals("Created"))
                    {
                        await ConfirmSingleOrder(item.purchaseOrderId, tokenResult);
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteLog($"ConfirmOrder Failed with exception: {ex.Message}");
                }

            }
        }

        private async Task ConfirmSingleOrder(string purchaseOrderId, TokenResult tokenResult)
        {
            if (!string.IsNullOrEmpty(tokenResult.AccessToken))
            {
                var BASE_URL = $"{_configuration.GetSection("Walmart:ConfirmOrder").Value}";
                var url = BASE_URL.Replace("purchaseOrderId", purchaseOrderId);
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);

                HttpResponseMessage response = await client.PostAsync(url, null);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Util.WriteLog($"Confirm Success: {responseBody}");
                }
                else
                {
                    Util.WriteLog($"Confirm Failed: {responseBody}");
                }
            }
        }

        private async Task<List<Order>> GetNewWalmartOrder()
        {
            List<Order> lstOrderSeller;

            var filter = Builders<Order>.Filter
                .And(
                    Builders<Order>.Filter.Or(
                    Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Created"),
                    Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Acknowledged")
                    )
                );

            lstOrderSeller = await _orderWalmartRepository.FindAll("Order", filter);

            return lstOrderSeller;
        }
    }
}
