using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbayApi.JobServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbayApi.DataContext;
using EbayApi.Dtos;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayApi.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace EbayApi.JobServices.Tests
{
    [TestClass()]
    public class UpdatePriceAndQuantiyToMarketplaceServiceTests
    {
        private UpdatePriceAndQuantiyToMarketplaceService service;
        private readonly EbayContext context;
        private readonly string env = "https://api.ebay.com/wsapi";
        public UpdatePriceAndQuantiyToMarketplaceServiceTests()
        {
            context = new EbayContext();
            service = new UpdatePriceAndQuantiyToMarketplaceService(context);
        }

        [TestMethod()]
        public async Task CallUpdatePriceQuantityEbayTest()
        {
            try
            {
                var apiContext = await GetApiContext();
                List<PriceQuantitySkuDto> lstUpdatePriceQuantity = new List<PriceQuantitySkuDto>()
                {
                    new PriceQuantitySkuDto {
                        ItemID = "364875262494",
                        Price = (decimal)86.5,
                        Quantity = 0,
                        Sku = "Q-QBR057-20"
                    },
                    new PriceQuantitySkuDto {
                        ItemID = "364877788577",
                        Price = (decimal)147.4,
                        Quantity = 0,
                        Sku = "Q-010L-10"
                    },
                };
                //test this function
                service.CallUpdatePriceQuantityEbay(lstUpdatePriceQuantity, apiContext);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        private async Task<ApiContext> GetApiContext()
        {
            UtilHelper.WriteLog("GetApiContext");
            var apiContext = new ApiContext()
            {
                SoapApiServerUrl = env,
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
                client.BaseAddress = new Uri("https://api.ebay.com/identity/v1/oauth2/token");

                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", "v^1.1#i^1#r^1#I^3#f^0#p^3#t^Ul4xMF82OkEyRjAyOEE5N0REMzgwQkM4OTFERTg3Qzk4NkRERDBBXzBfMSNFXjI2MA=="},
                    { "scope", string.Join(" ", lstScopes) }
                };
                var content = new FormUrlEncodedContent(formData);
                var clientId = "Vsirius-VsiriusA-PRD-e220d980b-5dc3524f";
                var clientSecret = "PRD-220d980b96a2-e402-4b03-82fd-b910";
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                var response = await client.PostAsync("https://api.ebay.com/identity/v1/oauth2/token", content);
                if (response.IsSuccessStatusCode)
                {
                    var contentReponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AccessTokenEbay>(contentReponse);
                    return result.access_token;
                }
                return "";
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
    }
}