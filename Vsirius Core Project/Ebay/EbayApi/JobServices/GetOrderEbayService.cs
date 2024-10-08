using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayApi.DataContext;
using EbayApi.Dtos;
using EbayApi.JobServices.Interfaces;
using EbayApi.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eBay.Service.Call;
using EbayApi.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;


namespace EbayApi.JobServices
{
    public class GetOrderEbayService : IGetOrderEbayService
    {
        private readonly EbayContext _context;

        public GetOrderEbayService(EbayContext context)
        {
            _context = context;
        }

        public async Task GetOrderFromEbay()
        {
            UtilHelper.WriteLog("Get Ebay Order Start");
            try
            {
                ApiContext apiContext = await GetApiContext();

                GetOrdersCall apiCall = new GetOrdersCall(apiContext);
                //apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);

                DateTime fromDate = DateTime.Now.AddDays(-4);
                DateTime toDate = DateTime.Now;

                OrderTypeCollection orders = apiCall
                    .GetOrders(
                    fromDate,
                    toDate,
                    TradingRoleCodeType.Seller,
                    OrderStatusCodeType.All);

                if (orders.Count > 0)
                {
                    foreach (OrderType order in orders)
                    {
                        var orderId = order.OrderID;
                        var orderEntity = new Order()
                        {
                            OrderId = orderId,
                            Address1 = order.ShippingAddress.Street1,
                            Address2 = order.ShippingAddress.Street2,
                            State = order.ShippingAddress.StateOrProvince,
                            ZipCode = order.ShippingAddress.PostalCode,
                            City = order.ShippingAddress.CityName,
                            CustomerName = order.ShippingAddress.Name,
                            Country = order.ShippingAddress.Country.ToString(),
                            TrackingNo = order.ShippingDetails?.ShipmentTrackingDetails[0]?.ShipmentTrackingNumber,
                            CustomerPhone = order.ShippingAddress.Phone,
                            CustomerId = order.BuyerUserID,
                            CustomerEmail = order.TransactionArray[0]?.Buyer?.Email,
                            OrderDate = order.CreatedTime,
                            StatusOrder = order.OrderStatus.ToString(),
                            TotalPrice = order.Subtotal.Value,
                            IsSubmit = false,
                            OrderDetails = new List<OrderDetail>()
                        };
                        foreach (TransactionType detail in order.TransactionArray)
                        {
                            ItemType item = detail.Item;
                            orderEntity.OrderDetails.Add(new OrderDetail()
                            {
                                OrderDetailId = detail.OrderLineItemID,
                                Price = detail.TransactionPrice.Value,
                                Quantity = item.Quantity,
                                OrderId = order.OrderID,
                                Order = orderEntity,
                                Sku = item.SKU,
                            });
                        }

                        var existingOrder = await _context.Orders.
                            Include(o => o.OrderDetails).AsNoTracking()
                            .FirstOrDefaultAsync(o => o.OrderId == orderId);

                        //Update
                        if (existingOrder != null)
                        {
                            orderEntity.IsSubmit = existingOrder.IsSubmit;
                        }

                        _context.Orders.AddOrUpdate(orderEntity);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    Console.WriteLine("No orders found.");
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("Get Ebay Order End");
        }
        private async Task<ApiContext> GetApiContext()
        {
            UtilHelper.WriteLog("GetApiContext");
            var apiContext = new ApiContext()
            {
                SoapApiServerUrl = ConfigurationManager.AppSettings["Ebay:Enviroment"],
                ApiCredential = new ApiCredential()
                {
                    oAuthToken = await GetApiCredential(),
                },
                Site = SiteCodeType.US,
                ApiLogManager = new ApiLogManager()
                {
                    //ApiLoggerList = new ApiLoggerCollection() { new FileLogger(Path.Combine("Storage", "Logs", $"Log_{DateTime.Now:yyyyMMdd}.txt"), true, true, true) },
                    EnableLogging = true
                }
            };
            return apiContext;
        }
        private async Task<string> GetApiCredential()
        {
            IList<string> lstScopes = new List<string>()
            {
                "https://api.ebay.com/oauth/api_scope",
                "https://api.ebay.com/oauth/api_scope/sell.marketing.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.marketing",
                "https://api.ebay.com/oauth/api_scope/sell.inventory.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.inventory",
                "https://api.ebay.com/oauth/api_scope/sell.account.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.account",
                "https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.fulfillment",
                "https://api.ebay.com/oauth/api_scope/sell.analytics.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.finances",
                "https://api.ebay.com/oauth/api_scope/sell.payment.dispute",
                "https://api.ebay.com/oauth/api_scope/commerce.identity.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.reputation",
                "https://api.ebay.com/oauth/api_scope/sell.reputation.readonly",
                "https://api.ebay.com/oauth/api_scope/commerce.notification.subscription",
                "https://api.ebay.com/oauth/api_scope/commerce.notification.subscription.readonly",
                "https://api.ebay.com/oauth/api_scope/sell.stores",
                "https://api.ebay.com/oauth/api_scope/sell.stores.readonly"
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Ebay:UrlAccessToken"]);

                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", ConfigurationManager.AppSettings["Ebay:RefreshToken"] },
                    { "scope", string.Join(" ", lstScopes) }
                };
                var content = new FormUrlEncodedContent(formData);
                var clientId = ConfigurationManager.AppSettings["Ebay:ClientId"];
                var clientSecret = ConfigurationManager.AppSettings["Ebay:ClientSecret"];
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                var response = await client.PostAsync(ConfigurationManager.AppSettings["Ebay:UrlAccessToken"], content);
                if (response.IsSuccessStatusCode)
                {
                    var contentReponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AccessTokenEbay>(contentReponse);
                    return result.access_token;
                }
                return "";
            }

        }
    }
}