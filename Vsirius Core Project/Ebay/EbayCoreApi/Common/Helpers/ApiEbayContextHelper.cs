using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EbayCoreApi.Common.Helpers
{
    public static class ApiEbayContextHelper
    {
        public static AccessTimeout accessInfo = GetAccessToken();

        public static ApiContext GetApiContext()
        {
            var accessInfo = GetAccessToken();

            return new ApiContext()
            {
                SoapApiServerUrl = ConfigHolder.Configuration["Ebay:Enviroment"],
                RuName = ConfigHolder.Configuration["Ebay:Ru"],
                ApiCredential = new ApiCredential()
                {
                    oAuthToken = accessInfo.access_token,
                    eBayToken = accessInfo.access_token,
                    ApiAccount = new ApiAccount()
                    {
                        Application = ConfigHolder.Configuration["Ebay:ClientId"],
                        Certificate = ConfigHolder.Configuration["Ebay:ClientSecret"],
                        Developer = ConfigHolder.Configuration["Ebay:DevID"],
                    }
                },
                Site = SiteCodeType.US,
                ApiLogManager = new ApiLogManager()
                {
                    EnableLogging = true
                }
            };
        }

        private static AccessTimeout GetAccessToken()
        {
            if (accessInfo == null || accessInfo.created_at.AddHours(1) < DateTime.Now)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigHolder.Configuration["Ebay:UrlAccessToken"]);

                    var formData = new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", ConfigHolder.Configuration["Ebay:RefreshToken"] },
                        { "scope", Scopes }
                    };
                    var content = new FormUrlEncodedContent(formData);
                    var clientId = ConfigHolder.Configuration["Ebay:ClientId"];
                    var clientSecret = ConfigHolder.Configuration["Ebay:ClientSecret"];
                    var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                    var response = client.PostAsync(ConfigHolder.Configuration["Ebay:UrlAccessToken"], content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var contentReponse = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<AccessTokenEbay>(contentReponse);
                        return new AccessTimeout
                        {
                            access_token = result.access_token,
                            expires_in = result.expires_in,
                            token_type = result.token_type,
                            created_at = DateTime.Now,
                        };
                    }
                    return null;
                }
            }
            else
            {
                return accessInfo;
            }

        }

        private static string Scopes = string.Join(" ",
            new List<string>() {
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
            });
    }

}
