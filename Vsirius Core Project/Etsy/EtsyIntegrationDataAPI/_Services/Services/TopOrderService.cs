using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class TopOrderService : ITopOrderService
    {
        private ILoggerCustom _logService;
        private readonly IPIMApiClient _pIMApiClient;
        private readonly IConfiguration _config;
        private readonly bool _isProduction;

        public TopOrderService(IPIMApiClient pIMApiClient, IConfiguration config, ILoggerCustom logService)
        {
            _pIMApiClient = pIMApiClient;
            _config = config;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _logService = logService;
        }

        public async Task<List<string>> GetTopListProduct()
        {
            try
            {
                List<string> results = new List<string>();
                var pageSize = 3000;
                var dataFirstCall = await _pIMApiClient.GetProductTopPaginationResult(1, pageSize, new SemaphoreSlim(2));
                var totalPages = 0;

                if (dataFirstCall != null)
                {
                    if (_isProduction)
                    {
                        totalPages = dataFirstCall.Pagination.TotalPage;
                    }
                    else
                    {
                        totalPages = 1;
                    }
                    results.AddRange(dataFirstCall.Result.Select(x => x.item_sku).ToList());
                }
                else
                {
                    await _logService.CreateLogInformation($"Get data first null");
                    return null;
                }
                //List rest data
                int maxConcurrency = 3;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                for (int i = 2; i <= totalPages; i++)
                {

                    //// using async await
                    var result = await _pIMApiClient.GetPaginationResult(i, pageSize, semaphore);
                    if (result == null)
                    {
                        Thread.Sleep(5000);
                        result = await _pIMApiClient.GetPaginationResult(i, pageSize, semaphore);
                    }
                    else
                    {
                        await _logService.CreateLogInformation($"Get data page: {pageSize}");
                        results.AddRange(result.Result.Select(x => x.item_sku).ToList());
                    }
                }
                return results;
            }
            catch (Exception ex)
            {

                await _logService.CreateLogError($"Get data top order error", ex.ToString());
                return null;
            }
        }
    }
}
