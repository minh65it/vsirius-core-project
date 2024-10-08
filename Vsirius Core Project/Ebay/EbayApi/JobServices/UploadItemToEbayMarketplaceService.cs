using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using EbayApi.DataContext;
using EbayApi.Dtos;
using EbayApi.JobServices.Interfaces;
using EbayApi.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//https://developer.ebay.com/api-docs/sell/static/feed/data-file-limits.html

namespace EbayApi.JobServices
{
    public class UploadItemToEbayMarketplaceService : IUploadItemToEbayMarketplaceService
    {
        private readonly EbayContext _context;

        public UploadItemToEbayMarketplaceService(
            EbayContext context)
        {
            _context = context;
        }

        public async Task UploadItemToEbayMarketplace()
        {
            UtilHelper.WriteLog("Start UploadItemToEbayMarketplace");
            try
            {
                int totalItemUpload = 5000;
                List<ProductSummary> lstSeller = await GetSellerList();
                if (lstSeller.Count > 1000)
                {
                    return;
                }
                var lstProductLocal = (await _context.Products.AsNoTracking().Where(x => x.IsDeleted == false
                && x.Quantity > 1 && x.Price < 200).ToListAsync());

                //Insert and reupdate product
                var apiContext = await GetApiContext();
                DateTime loopStartTime = DateTime.Now;
                int tokenTime = 7100;
                foreach (var productLocal in lstProductLocal)
                {
                    TimeSpan elapsed = DateTime.Now - loopStartTime;
                    if (elapsed.TotalSeconds >= tokenTime)
                    {
                        apiContext = await GetApiContext();
                        loopStartTime = DateTime.Now;
                    }

                    try
                    {
                        if (!lstSeller.Any(x => x.SKU == productLocal.Sku && x.Status == ListingStatusCodeType.Active))
                        {
                            productLocal.Quantity = 2;
                            ItemType item = BuildItem(productLocal);
                            AddFixedPriceItemCall apiCall = new AddFixedPriceItemCall(apiContext);
                            FeeTypeCollection fees = apiCall.AddFixedPriceItem(item);
                            double listingFee = 0.0;
                            foreach (FeeType fee in fees)
                            {
                                if (fee.Name == "ListingFee")
                                {
                                    listingFee = fee.Fee.Value;
                                    break;
                                }
                            }
                            var entity = await _context.Products.FindAsync(productLocal.Sku);
                            entity.Note = string.Format("Listing fee is: {0}", listingFee);
                            await _context.SaveChangesAsync();
                            totalItemUpload--;
                        }
                    }
                    catch (Exception ex)
                    {
                        var entity = await _context.Products.FindAsync(productLocal.Sku);
                        entity.Note = ex.Message;
                        UtilHelper.WriteLog(ex.Message);
                        await _context.SaveChangesAsync();
                    }

                    if (totalItemUpload == 0)
                    {
                        break;
                    }
                }

                //Delete product
                List<string> lstDeleted = lstSeller.Where(x => x.Status == ListingStatusCodeType.Active
                && !lstProductLocal.Select(a => a.Sku).Contains(x.SKU)).Select(x => x.SKU).ToList();
                if (lstDeleted.Count > 0)
                {
                    apiContext = await GetApiContext();
                    loopStartTime = DateTime.Now;
                    foreach (var deleteSKU in lstDeleted)
                    {
                        TimeSpan elapsed = DateTime.Now - loopStartTime;
                        if (elapsed.TotalSeconds >= tokenTime)
                        {
                            apiContext = await GetApiContext();
                            loopStartTime = DateTime.Now;
                        }

                        try
                        {
                            EndFixedPriceItemCall api = new EndFixedPriceItemCall(apiContext)
                            {

                                SKU = deleteSKU,
                                EndingReason = EndReasonCodeType.NotAvailable
                            };
                            api.Execute();
                        }
                        catch (Exception ex)
                        {
                            UtilHelper.WriteLog(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("End UploadItemToEbayMarketplace");
        }

        public async Task<List<ProductSummary>> GetSellerList()
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
                //sellerListCall.DetailLevelList.Add(DetailLevelCodeType.ReturnSummary);
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
            })
            .ToList();
        }

        public async Task<ApiContext> GetApiContext()
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

        ItemType BuildItem(Models.Product product)
        {
            ItemType item = new ItemType
            {
                SKU = product.Sku,
                // item title
                Title = product.Tittle,
                // item description
                Description = product.Description,
                // listing type
                ListingType = ListingTypeCodeType.FixedPriceItem,
                // listing price
                Currency = CurrencyCodeType.USD,
                // listing duration
                //https://developer.ebay.com/devzone/xml/docs/reference/ebay/types/ListingDurationCodeType.html
                ListingDuration = "GTC",
                // item location and country
                Location = "VsiriusLocation",
                Country = CountryCodeType.US,

                // item condition, New
                ConditionID = 1000,

                ItemSpecifics = new NameValueListTypeCollection()
                {
                    new NameValueListType()
                    {
                        Name = "Brand",
                        Value = new StringCollection() { product.Brand }
                    },
                    new NameValueListType()
                    {
                        Name = "Type",
                        Value = new StringCollection() { product.ProductType }
                    },
                    new NameValueListType()
                    {
                        Name = "Style",
                        Value = new StringCollection() { product.ItemStyle }
                    },
                    new NameValueListType()
                    {
                        Name = "Material",
                        Value = new StringCollection() { product.Material }
                    }
                },

                PictureDetails = new PictureDetailsType()
                {
                    GalleryType = GalleryTypeCodeType.Gallery,
                    PictureURL = new StringCollection()
                    {
                        product.MainImageUrl
                    }
                }
            };
            if (!string.IsNullOrWhiteSpace(product.Image1Url)) item.PictureDetails.PictureURL.Add(product.Image1Url);
            if (!string.IsNullOrWhiteSpace(product.Image2Url)) item.PictureDetails.PictureURL.Add(product.Image2Url);
            if (!string.IsNullOrWhiteSpace(product.Image3Url)) item.PictureDetails.PictureURL.Add(product.Image3Url);
            if (!string.IsNullOrWhiteSpace(product.Image4Url)) item.PictureDetails.PictureURL.Add(product.Image4Url);
            if (!string.IsNullOrWhiteSpace(product.Image5Url)) item.PictureDetails.PictureURL.Add(product.Image5Url);
            if (!string.IsNullOrWhiteSpace(product.Image6Url)) item.PictureDetails.PictureURL.Add(product.Image6Url);
            if (!string.IsNullOrWhiteSpace(product.Image7Url)) item.PictureDetails.PictureURL.Add(product.Image7Url);
            if (!string.IsNullOrWhiteSpace(product.Image8Url)) item.PictureDetails.PictureURL.Add(product.Image8Url);
            if (!string.IsNullOrWhiteSpace(product.Image9Url)) item.PictureDetails.PictureURL.Add(product.Image9Url);

            if (!string.IsNullOrWhiteSpace(product.ItemLength))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Length",
                    Value = new StringCollection() { product.ItemLength }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Size",
                    Value = new StringCollection() { product.ItemSize }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemWeight))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Weight",
                    Value = new StringCollection() { product.ItemWeight }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemColor))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Color",
                    Value = new StringCollection() { product.ItemColor }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneType))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Type",
                    Value = new StringCollection() { product.PrimaryStoneType }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Size",
                    Value = new StringCollection() { product.PrimaryStoneSize }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneShape))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Shape",
                    Value = new StringCollection() { product.PrimaryStoneShape }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ChainType))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Chain Type",
                    Value = new StringCollection() { product.ChainType }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.Origin))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Origin",
                    Value = new StringCollection() { product.Origin }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.RingSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Ring Size",
                    Value = new StringCollection() { product.RingSize }
                });
            }

            // listing category, 
            item.PrimaryCategory = new CategoryType()
            {
                CategoryID = CustomMapper.MultipleCategory(CategoryDictionary.Categories.First(x => x.Key == product.Category).Value, 0)
            };
            var secondCategory = CustomMapper.MultipleCategory(CategoryDictionary.Categories.First(x => x.Key == product.Category).Value, 1);
            if (!string.IsNullOrEmpty(secondCategory))
            {
                item.SecondaryCategory = new CategoryType()
                {
                    CategoryID = secondCategory
                };
            }

            // seller profile (shipping profile + returnPolicy profile + payment profile )       
            item.SellerProfiles = BuildSellerProfiles();

            // payment methods
            item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
            item.PaymentMethods.AddRange(
                new BuyerPaymentMethodCodeType[]
                {
                    BuyerPaymentMethodCodeType.CreditCard,
                    BuyerPaymentMethodCodeType.DirectDebit,
                    BuyerPaymentMethodCodeType.PayPal,
                }
                );
            // email is required if paypal is used as payment method
            item.PayPalEmailAddress = "vsirius.inc@gmail.com";

            // shipping details
            item.ShippingDetails = BuildShippingDetails();

            // return policy
            item.ReturnPolicy = new ReturnPolicyType
            {
                ReturnsAcceptedOption = "ReturnsAccepted",
                RefundOption = "MoneyBack",
                ReturnsWithinOption = "Days_30"
            };

            // handling time is required
            item.DispatchTimeMax = 1;

            //item Start Price
            AmountType amount = new AmountType
            {
                Value = (double)product.Price,
                currencyID = CurrencyCodeType.USD
            };
            item.BuyItNowPrice = amount;
            item.StartPrice = amount;
            item.Quantity = product.Quantity;


            // item variations- Variations not needed for Cookbook
            //item.Variations = buildVariationsType();
            return item;
        }

        static SellerProfilesType BuildSellerProfiles()
        {
            /*
             * Beginning with release 763, some of the item fields from
             * the AddItem/ReviseItem/VerifyItem family of calls have been
             * moved to the Business Policies API. 
             * See http://developer.ebay.com/Devzone/business-policies/Concepts/BusinessPoliciesAPIGuide.html for more
             * 
             * This example uses profiles that were previously created using this api.
             */

            SellerProfilesType sellerProfile = new SellerProfilesType();

            Console.WriteLine("Enter Return policy profile Id:");
            sellerProfile.SellerReturnProfile = new SellerReturnProfileType
            {
                ReturnProfileID = 297572810021
            };

            Console.WriteLine("Enter Shipping profile Id:");
            sellerProfile.SellerShippingProfile = new SellerShippingProfileType
            {
                ShippingProfileID = 297571474021
            };

            Console.WriteLine("Enter Payment profile Id:");
            sellerProfile.SellerPaymentProfile = new SellerPaymentProfileType
            {
                PaymentProfileID = 297571657021
            };

            return sellerProfile;
        }

        static ShippingDetailsType BuildShippingDetails()
        {
            // Shipping details
            ShippingDetailsType sd = new ShippingDetailsType
            {
                ApplyShippingDiscount = true,
                PaymentInstructions = "",
                // Shipping type and shipping service options
                ShippingType = ShippingTypeCodeType.Flat
            };
            ShippingServiceOptionsType shippingOptions = new ShippingServiceOptionsType()
            {
                ShippingService = ShippingServiceCodeType.USPSFirstClass.ToString(),
                ShippingServiceAdditionalCost = new AmountType
                {
                    Value = 1.25,
                    currencyID = CurrencyCodeType.USD
                },
                ShippingServiceCost = new AmountType
                {
                    Value = 7.5, //Refer getCategoryFeatures() for maximum shipping amounts.
                    currencyID = CurrencyCodeType.USD
                },
                ShippingServicePriority = 1,
                //ShippingInsuranceCost = new AmountType
                //{
                //    Value = 1.0,
                //    currencyID = CurrencyCodeType.USD
                //}
            };

            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(
                new ShippingServiceOptionsType[] { shippingOptions }
                );

            return sd;
        }
    }
}