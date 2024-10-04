using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models;
using AutoMapper;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public interface IGetDataAmazonLocal
    {
        public Task<List<AmazonJewelryDataFeedItem>> GetListPaginationResult();
    }
    public class GetDataAmazonLocal : IGetDataAmazonLocal
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public GetDataAmazonLocal(IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task<PaginationUtility<AmazonJewelryDataFeedItem>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<AmazonJewelryDataFeedItem> result = null;
            try
            {
                PaginationParam pagination = new PaginationParam()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IAmazonJewelryDataFeedItemService>();
                    var data = await service.GetData(pagination, null, true);
                    result = new PaginationUtility<AmazonJewelryDataFeedItem>(
                        _mapper.Map<List<AmazonJewelryDataFeedItem>>(data.Result),
                        data.Pagination.TotalCount,
                        data.Pagination.TotalPage,
                        data.Pagination.PageSize,
                        data.Pagination.Skip
                        );

                    return result;
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"GetPaginationResult Error: {ex.Message}");
                return result;
            }
            finally
            {
                semaphore.Release();
            }
            
        }
        public async Task<List<AmazonJewelryDataFeedItem>> GetListPaginationResult()
        {
            try
            {
                List<AmazonJewelryDataFeedItem> allResults = new List<AmazonJewelryDataFeedItem>();
                var pageSize = 1000;
                var dataFirstCall = await GetPaginationResult(1, pageSize, new SemaphoreSlim(2));
                var totalPages = 0;
                List<AmazonJewelryDataFeedItem> firstData = new List<AmazonJewelryDataFeedItem>();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData);
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<AmazonJewelryDataFeedItem>>> tasks = new();
                int amountRunTask = 0;
                int maxConcurrency = 6;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                for (int i = 2; i <= totalPages; i++)
                {
                    // multi task, cho 6 task chạy cùng 1 lúc, nếu nhiều hơn sẽ chờ đợi 6 task xong và chạy tiếp
                    amountRunTask++;
                    tasks.Add(GetPaginationResult(i, pageSize, semaphore));
                    if (amountRunTask > maxConcurrency)
                    {
                        amountRunTask = 0;
                        while (tasks.Count > 0)
                        {
                            var result = await Task.WhenAll(tasks);
                            foreach (var item in result)
                            {
                                allResults.AddRange(item.Result);
                            }
                            tasks.RemoveAll(x => x.IsCompleted);
                        }
                    }
                }

                var result2 = await Task.WhenAll(tasks);
                foreach (var item in result2)
                {
                    allResults.AddRange(item.Result);
                }
                tasks.RemoveAll(x => x.IsCompleted);

                return allResults;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"GetListPaginationResult Error: {ex.Message}");
                return null;
            }
        }
    }
}
