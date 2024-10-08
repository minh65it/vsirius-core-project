using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayApi.Dtos;
using EbayApi.JobServices.Interfaces;
using EbayApi.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EbayApi.DataContext;
using System.Data.Entity;

namespace EbayApi.JobServices
{
    public class UpdatePriceAndQuantiyToMarketplaceService : IUpdatePriceAndQuantiyToMarketplaceService
    {
        private readonly EbayContext _context;

        public UpdatePriceAndQuantiyToMarketplaceService(EbayContext context)
        {
            _context = context;
        }

        public async Task UpdatePriceAndQuantiyToMarketplace()
        {
            UtilHelper.WriteLog("Start UpdatePriceAndQuantiyToMarketplace");
            try
            {
                List<ProductSummary> lstSeller = await GetSellerList();
                lstSeller = lstSeller.Where(x => x.Status == ListingStatusCodeType.Active).ToList();
                var lstProductLocal = (await _context.Products.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync());
                var apiContext = await GetApiContext();
                int tokenTime = 7100;
                DateTime loopStartTime = DateTime.Now;
                var lstUpdatePriceQuantity = new List<PriceQuantitySkuDto>();
                for (int i = 1; i <= lstSeller.Count; i++)
                {
                    TimeSpan elapsed = DateTime.Now - loopStartTime;
                    if (elapsed.TotalSeconds >= tokenTime)
                    {
                        apiContext = await GetApiContext();
                        loopStartTime = DateTime.Now;
                    }
                    var itemSeller = lstSeller[i];
                    var itemLocal = lstProductLocal.FirstOrDefault(x => x.Sku == itemSeller.SKU);
                    if (itemLocal != null)
                    {
                        if(itemLocal.Quantity > 1)
                        {
                            itemLocal.Quantity = 2;
                        }
                        lstUpdatePriceQuantity.Add(new PriceQuantitySkuDto()
                        {
                            ItemID = itemSeller.ItemID,
                            Sku = itemSeller.SKU,
                            Price = itemLocal.Price,
                            Quantity = itemLocal.Quantity,
                        });
                    }
                    else
                    {
                        lstUpdatePriceQuantity.Add(new PriceQuantitySkuDto()
                        {
                            ItemID = itemSeller.ItemID,
                            Sku = itemSeller.SKU,
                            Price = 1999,
                            Quantity = 0
                        });
                    }
                    if (lstUpdatePriceQuantity.Count % 4 == 0 || i == lstSeller.Count)
                    {
                        CallUpdatePriceQuantityEbay(lstUpdatePriceQuantity, apiContext);
                        lstUpdatePriceQuantity.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("End UpdatePriceAndQuantiyToMarketplace");

        }

        public void CallUpdatePriceQuantityEbay(List<PriceQuantitySkuDto> lstUpdatePriceQuantity, ApiContext apiContext)
        {
            ReviseInventoryStatusRequestType requestType = new ReviseInventoryStatusRequestType();
            var InventoryStatusTypeCollection = new InventoryStatusTypeCollection();

            foreach (var item in lstUpdatePriceQuantity)
            {
                InventoryStatusTypeCollection.Add(new InventoryStatusType()
                {
                    ItemID = item.ItemID,
                    SKU = item.Sku,
                    Quantity = item.Quantity,
                    StartPrice = new AmountType()
                    {
                        currencyID = CurrencyCodeType.USD,
                        Value = (double)item.Price
                    },
                });
            }
            requestType.InventoryStatus = InventoryStatusTypeCollection;

            ReviseInventoryStatusCall apiCall = new ReviseInventoryStatusCall(apiContext);
            apiCall.ExecuteRequest(requestType);
        }

        private async Task<List<ProductSummary>> GetSellerList()
        {
            var lstProductInMarketplace = new List<ProductSummary>();
            int pageNumber = 1;
            try
            {
                //First call
                var apiContext = await GetApiContext();
                int tokenTime = 7000;

                DateTime loopStartTime = DateTime.Now;

                GetSellerListCall apicall = new GetSellerListCall(apiContext)
                {
                    EndTimeFilter = new TimeFilter()
                    {
                        TimeFrom = DateTime.Now.AddDays(-59),
                        TimeTo = DateTime.Now.AddDays(59)
                    },
                    GranularityLevel = GranularityLevelCodeType.Coarse,
                    UserID = "minh65it",
                    Pagination = new PaginationType()
                    {
                        PageNumber = pageNumber,
                        EntriesPerPage = 150
                    }
                };
                // Execute the call
                ItemTypeCollection sellerList = apicall.GetSellerList();
                lstProductInMarketplace.AddRange(
                    sellerList
                    .ToArray()
                    .ToList()
                    .Select(x => new ProductSummary()
                    {
                        SKU = x.SKU ?? "",
                        Status = x.SellingStatus.ListingStatus,
                        Tittle = x.Title,
                        ItemID = x.ItemID,
                    })
                    .ToList());
                int totalPages = apicall.PaginationResult.TotalNumberOfPages;
                for (pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    TimeSpan elapsed = DateTime.Now - loopStartTime;
                    if (elapsed.TotalSeconds >= tokenTime)
                    {
                        apiContext = await GetApiContext();
                        loopStartTime = DateTime.Now;
                    }
                    List<ProductSummary> lstMarketplace = CallApiToGetSellerList(pageNumber, apiContext);
                    if (pageNumber % 2 == 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(15));
                    }
                    lstProductInMarketplace.AddRange(lstMarketplace);
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
                throw;
            }
            return lstProductInMarketplace;
        }

        private List<ProductSummary> CallApiToGetSellerList(int pageNumber, ApiContext apiContext)
        {
            GetSellerListCall apicall = new GetSellerListCall(apiContext)
            {
                EndTimeFilter = new TimeFilter()
                {
                    TimeFrom = DateTime.Now.AddDays(-59),
                    TimeTo = DateTime.Now.AddDays(59)
                },
                GranularityLevel = GranularityLevelCodeType.Coarse,
                UserID = "minh65it",
                Pagination = new PaginationType()
                {
                    PageNumber = pageNumber,
                    EntriesPerPage = 150
                }
            };

            // Execute the call
            ItemTypeCollection sellerList = apicall.GetSellerList();
            return sellerList.ToArray().ToList()
            .Select(x => new ProductSummary()
            {
                SKU = x.SKU,
                Status = x.SellingStatus.ListingStatus,
                Tittle = x.Title,
                ItemID = x.ItemID,
            })
            .ToList();
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