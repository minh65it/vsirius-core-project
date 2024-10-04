using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Models;
using AutoMapper;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Update
{
    public interface IGetDataAmazonLocalForUpdate
    {
        public Task<List<AmazonJewelryDataForUpdate>> GetListPaginationResultForUpdate();
        public Task<List<AmazonJewelryDataForUpdate>> GetListPaginationResultForDel();
    }
    public class GetDataAmazonLocalForUpdate : IGetDataAmazonLocalForUpdate
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public GetDataAmazonLocalForUpdate(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task<PaginationUtility<AmazonJewelryDataForUpdate>> GetPaginationResultForUpdate(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<AmazonJewelryDataForUpdate> result = null;
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
                    var data = await service.GetDataForAmazon(pagination, null, true, false);
                    result = new PaginationUtility<AmazonJewelryDataForUpdate>(
                        _mapper.Map<List<AmazonJewelryDataForUpdate>>(data.Result),
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

        public async Task<PaginationUtility<AmazonJewelryDataForUpdate>> GetPaginationResultForDel(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<AmazonJewelryDataForUpdate> result = null;
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
                    var data = await service.GetDataForAmazon(pagination, null, true, true);
                    result = new PaginationUtility<AmazonJewelryDataForUpdate>(
                        _mapper.Map<List<AmazonJewelryDataForUpdate>>(data.Result),
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

        public async Task<List<AmazonJewelryDataForUpdate>> GetListPaginationResultForUpdate()
        {
            try
            {
                List<AmazonJewelryDataForUpdate> allResults = new List<AmazonJewelryDataForUpdate>();
                var pageSize = 1000;
                var dataFirstCall = await GetPaginationResultForUpdate(1, pageSize, new SemaphoreSlim(1));
                var totalPages = 0;
                List<AmazonJewelryDataForUpdate> firstData = new List<AmazonJewelryDataForUpdate>();
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
                List<Task<PaginationUtility<AmazonJewelryDataForUpdate>>> tasks = new();
                int amountRunTask = 0;
                int maxConcurrency = 6;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                for (int i = 2; i <= totalPages; i++)
                {
                    // multi task, cho 6 task chạy cùng 1 lúc, nếu nhiều hơn sẽ chờ đợi 6 task xong và chạy tiếp
                    amountRunTask++;
                    tasks.Add(GetPaginationResultForUpdate(i, pageSize, semaphore));
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
                await Utilities2.WriteLogAsync($"GetListPaginationResultForUpdate Error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<AmazonJewelryDataForUpdate>> GetListPaginationResultForDel()
        {
            try
            {
                List<AmazonJewelryDataForUpdate> allResults = new List<AmazonJewelryDataForUpdate>();
                var pageSize = 1000;
                var dataFirstCall = await GetPaginationResultForDel(1, pageSize, new SemaphoreSlim(1));
                var totalPages = 0;
                List<AmazonJewelryDataForUpdate> firstData = new List<AmazonJewelryDataForUpdate>();
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
                List<Task<PaginationUtility<AmazonJewelryDataForUpdate>>> tasks = new();
                int amountRunTask = 0;
                int maxConcurrency = 6;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                for (int i = 2; i <= totalPages; i++)
                {
                    // multi task, cho 6 task chạy cùng 1 lúc, nếu nhiều hơn sẽ chờ đợi 6 task xong và chạy tiếp
                    amountRunTask++;
                    tasks.Add(GetPaginationResultForDel(i, pageSize, semaphore));
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
                await Utilities2.WriteLogAsync($"GetListPaginationResultForUpdate Error: {ex.Message}");
                return null;
            }
        }
    }
}
