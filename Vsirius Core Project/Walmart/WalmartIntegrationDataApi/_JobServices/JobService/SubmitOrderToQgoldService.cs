using AutoMapper;
using MongoDB.Driver;
using System.Text;
using System.Text.Json;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi.CollectionMongo;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Qgold;
using WalmartIntegrationDataApi.Models;
using WalmartIntegrationDataApi.Models.Temp;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class SubmitOrderToQgoldService : ISubmitOrderToQgoldService
    {
        private readonly IOrderQgoldRepository _orderQgoldRepository;
        private readonly IOrderWalmartRepository _orderWalmartRepository;
        private readonly IConfiguration _configuration;
        private readonly OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService _qgoldProcessService;
        private readonly IMapper _mapper;

        public SubmitOrderToQgoldService(
            IOrderQgoldRepository orderQgoldRepository,
            IOrderWalmartRepository orderWalmartRepository,
            IConfiguration configuration,
            IMapper mapper,
            OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService qgoldProcessService)
        {
            _orderQgoldRepository = orderQgoldRepository;
            _orderWalmartRepository = orderWalmartRepository; 
            _configuration = configuration;
            _qgoldProcessService = qgoldProcessService;
            _mapper = mapper;
        }

        public async Task SendWalmartOrderToQgold()
        {
            Util.WriteLog("---------------------------Submit Order To Qgold Process Start-------------------------------------");

            try
            {
                Util.WriteLog("GetSubmittedWalmartOrder Start");
                var lstSubmit = await GetSubmittedWalmartOrder();
                Util.WriteLog($"Amount Submit Order in DB: {lstSubmit.Count}");
                Util.WriteLog("GetSubmittedWalmartOrder End");

                Util.WriteLog("Get Order From Qgold DB in 14 days Start");
                List<OrderQgold> lstOrderQgold = await GetQgoldOrder();
                Util.WriteLog($"Amount Order Qgold in DB: {lstOrderQgold.Count}");
                Util.WriteLog("Get Order From Qgold DB in 14 days End");

                Util.WriteLog("GetOrderWalmart Start");
                var lstOrderWalmart = await GetOrderWalmart();
                Util.WriteLog($"Amount Order Walmart in DB: {lstOrderWalmart.Count}");
                Util.WriteLog("GetOrderWalmart End");

                Util.WriteLog("SubmitOrderProcess Start");
                var lstPOIsSubmitted = lstSubmit.Select(x => x.WalmartOrderId).ToList();
                var lstPONoNeedSubmit = new List<string>() { };

                foreach (var qgold in lstOrderQgold)
                {
                    foreach (var walmart in lstOrderWalmart)
                    {
                        if (
                            walmart.shippingInfo.postalAddress.name.TrimStart()
                            .StartsWith(qgold.InvoiceDetails[0].ShipToText.TrimStart()[0].ToString(), StringComparison.OrdinalIgnoreCase)

                            && walmart.shippingInfo.postalAddress.address1.TrimStart()
                            .StartsWith(qgold.InvoiceDetails[0].ShipToAddress1.TrimStart()[0].ToString(), StringComparison.OrdinalIgnoreCase)

                            && qgold.InvoiceDetails[0].ZipCode.Contains(walmart.shippingInfo.postalAddress.postalCode, StringComparison.OrdinalIgnoreCase)
                            )
                        {
                            var lstSkuItemInWalmart = walmart.orderLines.orderLine.Where(x => x.item.sku.StartsWith("Q"))
                                .Select(x => Util.SKUQgold(x.item.sku)).ToList();
                            if (qgold.InvoiceDetails[0].QgoldInvoiceItems.Any(x => lstSkuItemInWalmart.Contains(x.Style)))
                            {
                                lstPONoNeedSubmit.Add(walmart.purchaseOrderId);
                                break;
                            }
                        }
                    }
                }

                var lstOrderNeedSubmit = lstOrderWalmart
                    .Where(x =>
                    !lstPOIsSubmitted.Contains(x.purchaseOrderId)
                    && !lstPONoNeedSubmit.Contains(x.purchaseOrderId)).ToList();
                Util.WriteLog($"Amount Order Need Submit: {lstOrderNeedSubmit.Count}");
                await SubmitOrderProcess(lstOrderNeedSubmit);

                lstSubmit = null;
                lstOrderQgold = null;
                lstOrderWalmart = null;
                lstPOIsSubmitted = null;
                lstPONoNeedSubmit = null;
                lstOrderNeedSubmit = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Error when submit order to Qgold: {ex.Message}");
            }
            
            Util.WriteLog("---------------------------Submit Order To Qgold Process End-------------------------------------");        
        }

        private async Task SubmitOrderProcess(List<Order> lstOrderNeedSubmit)
        {
            List<OrderQgoldParam> lstUpload = new List<OrderQgoldParam>();
            OrderQgoldParam paramQgold;
            foreach (var item in lstOrderNeedSubmit)
            {
                paramQgold = new OrderQgoldParam()
                {
                    orderId = null,
                    format = "5",
                    customer = "58772",
                    timeStamp = DateTime.Now,
                    shipToName = item.shippingInfo.postalAddress.name,
                    shipToAddress1 = item.shippingInfo.postalAddress.address1,
                    shipToAddress2 = item.shippingInfo.postalAddress.address2,
                    shipToCity = item.shippingInfo.postalAddress.city,
                    shipToState_Province = item.shippingInfo.postalAddress.state,
                    shipToZip = item.shippingInfo.postalAddress.postalCode,
                    shipToCountryCode = "US",
                    shipToPhone = item.shippingInfo.phone,
                    ship_Method_Code = "1102",
                    ship_Option_Code = "100",
                    amz_Order_ID = item.purchaseOrderId,
                    marketplace = "Walmart",
                    item = new List<ItemQgoldParam>()
                };

                foreach (var item2 in item.orderLines.orderLine)
                {
                    if (item2.item.sku.StartsWith("S"))
                    {
                        //De submit order Stuller sau
                        continue;
                    }
                    //hang Qgold
                    if (item2.item.sku.StartsWith("Q"))
                    {
                        var sku = Util.SKUQgold(item2.item.sku);
                        var obj = paramQgold.item.FirstOrDefault(x => x.item.Equals(sku));
                        if (obj == null)
                        {
                            paramQgold.item.Add(new ItemQgoldParam()
                            {
                                item = sku,
                                qty = item2.orderLineQuantity.amount,
                                price = item2.charges.charge[0].chargeAmount.amount.ToString()
                            });
                        }
                        else
                        {
                            obj.qty = (int.Parse(obj.qty) + 1).ToString();
                        }
                    }
                }

                if (paramQgold.item.Count > 0)
                {
                    lstUpload.Add(paramQgold);
                }
            }

            var isSuccess = await CallSubmittedApi_RFT(lstUpload);

            if (isSuccess)
            {
                foreach (var item in lstUpload)
                {
                    await AddInfoSubmitOrderWalmartToDB(item);
                }
                Util.WriteLog($"AddInfoSubmitOrderWalmartToDB Success Detail: {JsonSerializer.Serialize(lstUpload)}");
            }
            else
            {
                Util.WriteLog($"AddInfoSubmitOrderWalmartToDB Failed Detail: {JsonSerializer.Serialize(lstUpload)}");
            }
        }

        private async Task AddInfoSubmitOrderWalmartToDB(OrderQgoldParam item)
        {
            var obj = new SubmittedWalmartOrder()
            {
                SubmittedDate = DateTime.Now,
                WalmartOrderId = item.amz_Order_ID
            };
            await _orderWalmartRepository.InsertOne(obj, CollectionName.SubmittedOrderWalmart);
        }

        private async Task<bool> CallSubmittedApi(List<OrderQgoldParam> lstUpload)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("clientId", $"{_configuration.GetSection("OrderProcessAPI:clientId").Value}");
                httpClient.DefaultRequestHeaders.Add("clientSecret", $"{_configuration.GetSection("OrderProcessAPI:clientSecret").Value}");

                var jsonContent = JsonSerializer.Serialize(lstUpload);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(_configuration.GetSection("OrderProcessAPI:url").Value, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<bool>(result);
                }
                Util.WriteLog($"CallSubmittedApi Err: {JsonSerializer.Serialize(response)}");
                return false;

            }
            catch (Exception ex)
            {
                Util.WriteLog($"CallSubmittedApi Err: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> CallSubmittedApi_RFT(List<OrderQgoldParam> lstUpload)
        {
            try
            {
                var list = new List<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>();
                foreach (var item in lstUpload)
                {
                    var dto = _mapper.Map<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>(item);
                    list.Add(dto);
                }
                var result = await _qgoldProcessService.SubmitQgold(list, null);
                return result;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"CallSubmittedApi Err: {ex.Message}");
                return false;
            }
        }


        // Exten function
        private async Task<List<Order>> GetOrderWalmart()
        {
            List<Order> lstOrder;

            var filter = Builders<Order>.Filter
            .Or(
                Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Created"),
                Builders<Order>.Filter.Eq("orderLines.orderLine.orderLineStatuses.orderLineStatus.status", "Acknowledged")
            );

            lstOrder = await _orderWalmartRepository.FindAll(CollectionName.OrderWalmart, filter);
            return lstOrder;
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

            filter = Builders<OrderQgold>.Filter.And(
                Builders<OrderQgold>.Filter.Where(x => lst14PastDays.Contains(x.Date)),
                Builders<OrderQgold>.Filter.Or(
                    Builders<OrderQgold>.Filter.Where(x => x.Status == "Hold"),
                    Builders<OrderQgold>.Filter.Where(x => x.Status == "Invoiced"),
                    Builders<OrderQgold>.Filter.Where(x => x.Status == "Pending")
                    )
                );

            lstOrder = await _orderQgoldRepository.FindAll(CollectionName.OrderQgold, filter);

            return lstOrder;
        }

        private async Task<List<SubmittedWalmartOrder>> GetSubmittedWalmartOrder()
        {
            List<SubmittedWalmartOrder> lstOrder;

            lstOrder = await _orderWalmartRepository.FindAll<SubmittedWalmartOrder>(CollectionName.SubmittedOrderWalmart);

            return lstOrder;
        }
    }
}
