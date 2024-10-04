using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class MapVSiriusDBToAmazonDB : IMapVSiriusDBToAmazonDB
    {
        private readonly IVSiriusProductApiClient _vSiriusProductApiClient;
        private readonly IInsertProduct _insertProduct;
        private readonly IExcludedProduct _excludedProduct;
        private readonly DBContext _context;

        public MapVSiriusDBToAmazonDB(IVSiriusProductApiClient vSiriusProductApiClient,
            IInsertProduct insertProduct,
            DBContext context,
            IExcludedProduct excludedProduct)
        {
            _vSiriusProductApiClient = vSiriusProductApiClient;
            _insertProduct = insertProduct;
            _context = context;
            _excludedProduct = excludedProduct;
        }

        public async Task<OperationResult> MapToAmazonDB()
        {
            try
            {
                await Utilities2.WriteLogAsync("Map data VSiriusDB to AmazonDB START //========================================================");
                var pageSize = 3000;
                var dataFirstCall = await _vSiriusProductApiClient.GetPaginationResult(1, pageSize, new SemaphoreSlim(2));
                var totalPages = 0;
                //first data
                List<ExcludedProductDto>? lstExcludedProduct = await _excludedProduct.GetExcludedProducts();
                if (dataFirstCall != null)
                {
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    await _insertProduct.Insert(dataFirstCall.Result, lstExcludedProduct);
                }
                else
                {
                    await Utilities2.WriteLogAsync($"Get data first null");
                    return new OperationResult { IsSuccess = false };
                }

                //List rest data
                List<Task<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>>> tasks = new();
                int amountRunTask = 0;
                int maxConcurrency = 3;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                bool checkNull = false;
                for (int i = 2; i <= totalPages; i++)
                {

                    //// using async await
                    var result = await _vSiriusProductApiClient.GetPaginationResult(i, pageSize, semaphore);
                    if (result == null)
                    {
                        Thread.Sleep(5000);
                        result = await _vSiriusProductApiClient.GetPaginationResult(i, pageSize, semaphore);
                    }
                    if (result == null)
                    {
                        checkNull = true;
                    }
                    else
                    {
                        await Utilities2.WriteLogAsync($"Get data page: {pageSize}");
                        await _insertProduct.Insert(result.Result, lstExcludedProduct);
                    }

                    //// multi task, cho 6 task chạy cùng 1 lúc, nếu nhiều hơn sẽ chờ đợi 3 task xong và chạy tiếp
                    //amountRunTask++;
                    //tasks.Add(_vSiriusProductApiClient.GetPaginationResult(i, pageSize, semaphore));

                    ////run multi task
                    //if (amountRunTask > maxConcurrency)
                    //{
                    //    amountRunTask = 0;
                    //    while (tasks.Count > 0)
                    //    {
                    //        var result = await Task.WhenAll(tasks);
                    //        foreach (var item in result)
                    //        {
                    //            await Utilities2.WriteLogAsync($"Insert page: {pageSize}");
                    //            await _insertProduct.Insert(item.Result);
                    //        }
                    //        tasks.RemoveAll(x => x.IsCompleted);
                    //    }
                    //}
                }
                //var result2 = await Task.WhenAll(tasks);
                //foreach (var item in result2)
                //{
                //    await _insertProduct.Insert(item.Result);
                //}
                //tasks.RemoveAll(x => x.IsCompleted);

                //Delete lstExcludedProduct
                if (lstExcludedProduct != null)
                {
                    var querySKUExcludedProduct = await _context.AmazonJewelryDataFeedItems.Where(x => lstExcludedProduct.Select(x => x.MarketplaceSKU).Contains(x.item_sku) && x.IsDeleted == false).Select(x => x.item_sku).ToListAsync();
                    if (querySKUExcludedProduct.Count >= 0)
                    {
                        var curDate = DateTime.Now;
                        await _context.AmazonJewelryDataFeedItems.Where(x => querySKUExcludedProduct.Contains(x.item_sku))
                             .ExecuteUpdateAsync(s => s
                             .SetProperty(i => i.IsDeleted, i => true)
                             .SetProperty(i => i.UpdatedDate, i => curDate)
                             .SetProperty(i => i.quantity, i => "0")
                             );
                    }
                }
                //Delete or Update quantity
                DateTime updateTime = checkNull ? DateTime.Now.AddHours(-12) : DateTime.Now.AddHours(-2);
                var predicate = PredicateBuilder.New<AmazonJewelryDataFeedItemV3>(x => x.UpdatedDate <= updateTime && x.IsDeleted == false);
                var querySKUDelete = await _context.AmazonJewelryDataFeedItems.Where(predicate).Select(x => x.item_sku).ToListAsync();
                if (querySKUDelete.Count >= 0)
                {
                    if (querySKUDelete.Count >= 1000)
                    {
                        querySKUDelete = querySKUDelete.Take(1000).ToList();
                    }
                    var curDate = DateTime.Now;
                    await _context.AmazonJewelryDataFeedItems.Where(x => querySKUDelete.Contains(x.item_sku))
                         .ExecuteUpdateAsync(s => s
                         .SetProperty(i => i.IsDeleted, i => true)
                         .SetProperty(i => i.UpdatedDate, i => curDate)
                         .SetProperty(i => i.quantity, i => "0")
                         );
                }
                await Utilities2.WriteLogAsync("Map data VSiriusDB to AmazonDB DONE //========================================================");
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Get Data Failed with exception: {ex.Message} ");
                return new OperationResult { IsSuccess = false };
            }

        }
    }
}
