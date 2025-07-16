using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Etsy.Etsy
{
    public class MapPIMToEtsyDBService : IMapPIMToEtsyDBService
    {
        private ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private IPIMApiClient _pIMApiClient;
        private IExcludedProduct _excludedProduct;
        private IInsertProduct _insertProduct;
        private readonly DataContext _context;
        private readonly bool _isProduction;

        public MapPIMToEtsyDBService(ILoggerCustom logService,
            IPIMApiClient pIMApiClient,
            DataContext context,
            IExcludedProduct excludedProduct,
            IInsertProduct insertProduct,
            IConfiguration config)
        {
            _logService = logService;
            _pIMApiClient = pIMApiClient;
            _context = context;
            _excludedProduct = excludedProduct;
            _insertProduct = insertProduct;
            _config = config;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
        }

        public async Task<OperationResult> MapToEtsyDB()
        {
            Console.WriteLine("Run MapToEtsyDB");
            try
            {
                await _logService.CreateLogInformation("Map data PIM to EtsyDB START //========================================================");
                var pageSize = 3000;
                var dataFirstCall = await _pIMApiClient.GetPaginationResult_RFT(1, pageSize, new SemaphoreSlim(2));
                var totalPages = 0;
                //first data
                List<ExcludedProductDto>? lstExcludedProduct = await _excludedProduct.GetExcludedProducts();
                var checkLstImg = _context.ImageProduct.ToList();
                if (dataFirstCall != null)
                {
                    if (_isProduction)
                    {
                        totalPages = dataFirstCall.Pagination.TotalPage;
                    }
                    else
                    {
                        totalPages = 20;
                    }
                    await _insertProduct.Insert(dataFirstCall.Result, lstExcludedProduct, checkLstImg);
                }
                else
                {
                    await _logService.CreateLogInformation($"Get data first null");
                    return new OperationResult { Success = false };
                }

                //List rest data
                List<Task<PaginationUtility<ProductPIMDto>>> tasks = new();
                int amountRunTask = 0;
                int maxConcurrency = 3;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                bool checkNull = false;
                for (int i = 2; i <= totalPages; i++)
                {

                    //// using async await
                    var result = await _pIMApiClient.GetPaginationResult_RFT(i, pageSize, semaphore);
                    if (result == null)
                    {
                        Thread.Sleep(5000);
                        result = await _pIMApiClient.GetPaginationResult_RFT(i, pageSize, semaphore);
                    }
                    if (result == null)
                    {
                        checkNull = true;
                    }
                    else
                    {
                        await _logService.CreateLogInformation($"Get data page: {pageSize}");
                        await _insertProduct.Insert(result.Result, lstExcludedProduct, checkLstImg);
                    }
                }
                //Delete or Update quantity
                DateTime updateTime = checkNull ? DateTime.Now.AddHours(-12) : DateTime.Now.AddHours(-2);
                var predicate = PredicateBuilder.New<ProductEtsy>(x => x.updatedDate <= updateTime);
                var querySKUDelete = await _context.ProductEtsy.Where(predicate).Select(x => x.item_sku).ToListAsync();
                if (querySKUDelete.Count > 0)
                {
                    if (querySKUDelete.Count >= 1000)
                    {
                        querySKUDelete = querySKUDelete.Take(1000).ToList();
                    }
                    var curDate = DateTime.Now;
                    await _context.ProductEtsy.Where(x => querySKUDelete.Contains(x.item_sku))
                         .ExecuteUpdateAsync(s => s
                         .SetProperty(i => i.updatedDate, i => curDate)
                         .SetProperty(i => i.quantity, i => 0)
                         );
                }
                await _logService.CreateLogInformation("Map data PIM to EtsyDB DONE //========================================================");
                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(ex.Message, "Map data PIM to EtsyDB ERROR!!!");
                return new OperationResult { Success = false };
            }
            throw new NotImplementedException();
        }
    }
}
