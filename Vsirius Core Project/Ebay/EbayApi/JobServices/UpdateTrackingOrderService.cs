using EbayApi.DataContext;
using EbayApi.Dtos;
using EbayApi.JobServices.Interfaces;
using EbayApi.Models;
using EbayApi.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EbayApi.JobServices
{
    public class UpdateTrackingOrderService : IUpdateTrackingOrderService
    {
        private readonly EbayContext _context;

        public UpdateTrackingOrderService(EbayContext context)
        {
            _context = context;
        }

        public async Task UpdateTrackingOrder()
        {
            UtilHelper.WriteLog("Update Tracking Ebay Order Start");
            try
            {
                var lstOrderNeedUpdateTracking = await _context.Orders.AsNoTracking().Include(x => x.OrderDetails)
                .Where(x => string.IsNullOrEmpty(x.TrackingNo) && x.IsSubmit == true)
                .ToListAsync();

                List<OrderQgoldDto> lstOrderQgold = await GetQgoldOrder();

                List<OrderStullerDto> lstOrderStuller = await GetStullerOrder();

                var lstTracking = new List<TrackingDto>();

                foreach (var ebay in lstOrderNeedUpdateTracking)
                {
                    if (ebay.OrderDetails.Any(x => x.Sku.StartsWith("Q")))
                    {
                        var qgold = lstOrderQgold.FirstOrDefault(
                            q => q.Address1.TrimStart()[0].ToString().ToLower() == ebay.Address1.TrimStart()[0].ToString().ToLower()
                            && q.BuyerName.TrimStart()[0].ToString().ToLower() == ebay.CustomerName.TrimStart()[0].ToString().ToLower()
                            && q.ZipCode.TrimStart()[0].ToString().ToLower() == ebay.ZipCode.TrimStart()[0].ToString().ToLower()
                            && q.Items.Select(x => x.Style).Any(x => ebay.OrderDetails.Where(e => e.Sku.StartsWith("Q"))
                            .Select(e => UtilHelper.GetOriginSKU(e.Sku)).Contains(x)));
                        if (qgold != null)
                        {
                            lstTracking.Add(new TrackingDto()
                            {
                                OrderId = ebay.OrderId,
                                DetailItems = ebay.OrderDetails.Select(x => new TrackingDetailDto()
                                {
                                    LineItemId = x.OrderDetailId,
                                    Quantity = x.Quantity
                                }),
                                TrackingNo = qgold.Tracking
                            });
                        }
                    }
                    if (ebay.OrderDetails.Any(x => x.Sku.StartsWith("S")))
                    {
                        var stuller = lstOrderStuller.FirstOrDefault(s => s.po == ebay.OrderId);
                        if (stuller != null)
                        {
                            lstTracking.Add(new TrackingDto()
                            {
                                OrderId = ebay.OrderId,
                                DetailItems = ebay.OrderDetails.Select(x => new TrackingDetailDto()
                                {
                                    LineItemId = x.OrderDetailId,
                                    Quantity = x.Quantity
                                }),
                                TrackingNo = stuller.tracking
                            });
                        }
                    }
                }

                if (lstTracking.Count > 0)
                {
                    await CallApiUpdateTrackingEbay(lstTracking);
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("Update Tracking Ebay Order End");
        }

        private async Task CallApiUpdateTrackingEbay(List<TrackingDto> lstTracking)
        {
            string oauthToken = await GetOauthToken();
            foreach (var tracking in lstTracking)
            {
                HttpClient client = new HttpClient();
                var shippingFulfillmentRequest = new
                {
                    lineItems = tracking.DetailItems.Select(x => new
                    {
                        lineItemId = x.LineItemId,
                        quantity = x.Quantity
                    }),
                    shippingCarrierCode = "USPS",
                    trackingNumber = tracking.TrackingNo
                };

                var jsonRequest = JsonConvert.SerializeObject(shippingFulfillmentRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

                var response = await client.PostAsync($"{ConfigurationManager.AppSettings["Ebay:UpdateTrackingUrl"]}{tracking.OrderId}/shipping_fulfillment", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var entity = _context.Orders.FirstOrDefault(x => x.OrderId == tracking.OrderId);
                    if (entity != null)
                    {
                        entity.TrackingNo = tracking.TrackingNo;
                        await _context.SaveChangesAsync();
                    }
                    Console.WriteLine("Shipping fulfillment created successfully!");
                }
                else
                {
                    Console.WriteLine("Error creating shipping fulfillment:");
                }
                UtilHelper.WriteLog($"Update Order Tracking:{tracking.OrderId},{responseBody}|{response.ReasonPhrase}");
            }
        }

        private async Task<string> GetOauthToken()
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

        private async Task<List<OrderStullerDto>> GetStullerOrder()
        {
            var client = new HttpClient();
            var url = ConfigurationManager.AppSettings["StullerApi:GetOrder"]
                + $"?PageSize=300&FromDate={DateTime.Now.AddDays(-5):yyyy-MM-dd}&ToDate={DateTime.Now:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PaginationUtil<OrderStullerDto>>(content).Result;
            }
            return default;
        }

        private async Task<List<OrderQgoldDto>> GetQgoldOrder()
        {
            var client = new HttpClient();
            var url = ConfigurationManager.AppSettings["QgoldOrderApi:GetOrder"]
                + $"?PageSize=300&FromDate={DateTime.Now.AddDays(-5):yyyy-MM-dd}&ToDate={DateTime.Now:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PaginationDto<OrderQgoldDto>>(content).Data;
            }
            return default;
        }
    }
}