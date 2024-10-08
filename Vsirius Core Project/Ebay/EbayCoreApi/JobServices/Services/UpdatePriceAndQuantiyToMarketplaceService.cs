using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.JobServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace EbayCoreApi.JobServices.Services
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
                var lstProductLocal = await _context.Products.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

                var lstUpdatePriceQuantity = new List<PriceQuantitySkuDto>();
                for (int i = 1; i <= lstSeller.Count; i++)
                {
                    try
                    {
                        var itemSeller = lstSeller[i];
                        var itemLocal = lstProductLocal.FirstOrDefault(x => x.Sku == itemSeller.SKU);
                        if (itemLocal != null)
                        {
                            if (itemLocal.Quantity > 1)
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

                            var api = new EndFixedPriceItemCall(ApiEbayContextHelper.GetApiContext())
                            {
                                SKU = itemSeller.SKU,
                                EndingReason = EndReasonCodeType.NotAvailable
                            };
                            api.Execute();

                        }
                        if (lstUpdatePriceQuantity.Count % 4 == 0 || i == lstSeller.Count)
                        {
                            CallUpdatePriceQuantityEbay(lstUpdatePriceQuantity, ApiEbayContextHelper.GetApiContext());
                            lstUpdatePriceQuantity.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        UtilHelper.WriteLog(ex.Message);
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
            var InventoryStatusTypeCollection = new List<InventoryStatusType>();

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
                lstProductInMarketplace.AddRange(
                    sellerList
                    .Select(x => new ProductSummary()
                    {
                        SKU = x.SKU ?? "",
                        Status = x.SellingStatus.ListingStatus,
                        Tittle = x.Title,
                        ItemID = x.ItemID,
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
                ItemID = x.ItemID,
            })
            .ToList();
        }

    }
}