using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Common.Mappers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.JobServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EbayCoreApi.JobServices.Services
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
                int totalItemUpload = 100;
                List<ProductSummary> lstSeller = await GetSellerList();
                if (lstSeller.Count > 1000)
                {
                    return;
                }
                var lstProductLocal = await _context.Products
                    .AsNoTracking()
                    .Where(x => x.IsDeleted == false && x.Quantity > 1 && x.Price < 200)
                    .ToListAsync();

                //Insert or relist product
                foreach (var productLocal in lstProductLocal)
                {
                    try
                    {
                        var itemSeller = lstSeller.FirstOrDefault(x => x.SKU == productLocal.Sku);
                        if (itemSeller == null)
                        {
                            productLocal.Quantity = 2;
                            ItemType item = BuildItem(productLocal);
                            AddFixedPriceItemCall apiCall = new AddFixedPriceItemCall(ApiEbayContextHelper.GetApiContext());
                            var fees = apiCall.AddFixedPriceItem(item);
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
                        else
                        {
                            if (itemSeller.Status == ListingStatusCodeType.Ended)
                            {
                                productLocal.Quantity = 2;
                                ItemType item = BuildItem(productLocal);
                                var apiCall = new RelistFixedPriceItemCall(ApiEbayContextHelper.GetApiContext());

                                var fees = apiCall.RelistFixedPriceItem(item, new List<string>());
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
                && !lstProductLocal.Any(a => a.Sku == x.SKU)).Select(x => x.SKU).ToList();
                if (lstDeleted.Count > 0)
                {
                    foreach (var deleteSKU in lstDeleted)
                    {
                        try
                        {
                            EndFixedPriceItemCall api = new EndFixedPriceItemCall(ApiEbayContextHelper.GetApiContext())
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
                GetSellerListCall apicall = new(ApiEbayContextHelper.GetApiContext())
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
                var sellerList = apicall.GetSellerList();
                lstProductInMarketplace.AddRange(
                    sellerList
                    .Where(x => !string.IsNullOrWhiteSpace(x.SKU))
                    .Select(x => new ProductSummary()
                    {
                        SKU = x.SKU,
                        Status = x.SellingStatus.ListingStatus,
                        Tittle = x.Title,
                    })
                    .ToList());
                int totalPages = (int)apicall.PaginationResult.TotalNumberOfPages;
                for (pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    List<ProductSummary> lstMarketplace = CallApiToGetSellerList(pageNumber);
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
            }
            return lstProductInMarketplace;
        }

        private List<ProductSummary> CallApiToGetSellerList(int pageNumber)
        {
            GetSellerListCall apicall = new GetSellerListCall(ApiEbayContextHelper.GetApiContext())
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
            var sellerList = apicall.GetSellerList();
            return sellerList
            .Select(x => new ProductSummary()
            {
                SKU = x.SKU,
                Status = x.SellingStatus.ListingStatus,
                Tittle = x.Title,
            })
            .ToList();
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

                ItemSpecifics = new List<NameValueListType>()
                {
                    new NameValueListType()
                    {
                        Name = "Brand",
                        Value = new List<string>() { product.Brand }
                    },
                    new NameValueListType()
                    {
                        Name = "Type",
                        Value = new () { product.ProductType }
                    },
                    new NameValueListType()
                    {
                        Name = "Style",
                        Value = new () { product.ItemStyle }
                    },
                    new NameValueListType()
                    {
                        Name = "Material",
                        Value = new () { product.Material }
                    }
                },

                PictureDetails = new PictureDetailsType()
                {
                    GalleryType = GalleryTypeCodeType.Gallery,
                    PictureURL = new()
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
                    Value = new() { product.ItemLength }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Size",
                    Value = new() { product.ItemSize }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemWeight))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Weight",
                    Value = new() { product.ItemWeight }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ItemColor))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Color",
                    Value = new() { product.ItemColor }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneType))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Type",
                    Value = new() { product.PrimaryStoneType }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Size",
                    Value = new() { product.PrimaryStoneSize }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.PrimaryStoneShape))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Primary Stone Shape",
                    Value = new() { product.PrimaryStoneShape }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.ChainType))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Chain Type",
                    Value = new() { product.ChainType }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.Origin))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Origin",
                    Value = new() { product.Origin }
                });
            }
            if (!string.IsNullOrWhiteSpace(product.RingSize))
            {
                item.ItemSpecifics.Add(new NameValueListType()
                {
                    Name = "Ring Size",
                    Value = new() { product.RingSize }
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
            item.PaymentMethods = new List<BuyerPaymentMethodCodeType>();
            item.PaymentMethods.AddRange(
                new BuyerPaymentMethodCodeType[]
                {
                    BuyerPaymentMethodCodeType.CreditCard,
                    BuyerPaymentMethodCodeType.DirectDebit,
                    BuyerPaymentMethodCodeType.PayPal,
                });
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
                    Value = 7.5,
                    currencyID = CurrencyCodeType.USD
                },
                ShippingServicePriority = 1,
            };

            sd.ShippingServiceOptions = new List<ShippingServiceOptionsType>(
            new ShippingServiceOptionsType[] { shippingOptions }
            );

            return sd;
        }
    }
}
