using AutoMapper;
using AutoMapper.QueryableExtensions;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using LinqKit;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class ProductEtsyService : IProductEtsyService
    {
        private readonly DataContext _context;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IProductEtsyRepository _productEstyRepository;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public ProductEtsyService(DataContext context,
            MapperConfiguration mapperConfiguration,
            IProductEtsyRepository productEstyRepository,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _mapperConfiguration = mapperConfiguration;
            _productEstyRepository = productEstyRepository;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<ProductEtsyDto>> GetAllData(string? keyword, bool? checkQuantity = true, bool? isDraft = false)
        {
            //keyword = "Q-103E";
            var pageSize = 2000;
            var totalPages = 0;
            var allResults = new List<ProductEtsyDto>();
            var predicate = PredicateBuilder.New<ProductEtsy>(true);
            if (checkQuantity == true)
            {
                predicate.And(x => x.quantity > 0);
            }
            //isDraft = true thì lấy các sản phẩm đang có trong database nhưng chưa listing trên Etsy
            if (isDraft == true)
            {
                predicate.And(x => x.listing_id == 0);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                predicate.And(x => x.title.Contains(keyword) || x.description.Contains(keyword)
                    || x.who_made.Contains(keyword) || x.item_sku.Contains(keyword)
                    || x.when_made.Contains(keyword));
            }

            var query = _productEstyRepository.FindAll(predicate, x => x.materials, x => x.tags, x => x.styles, x => x.image_ids).ProjectTo<ProductEtsyDto>(_mapperConfiguration);
            var dataPage1 = await PaginationUtility<ProductEtsyDto>.CreateAsync(query, 1, pageSize, true);
            if (dataPage1 == null)
            {
                return null;
            }
            allResults.AddRange(dataPage1.Result);
            totalPages = dataPage1.Pagination.TotalPage;
            int amountRunTask = 0;
            int maxConcurrency = 1;
            SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
            List<Task<List<ProductEtsyDto>>> tasks = new();
            for (int i = 2; i <= totalPages; i++)
            {
                amountRunTask++;
                tasks.Add(GetData(predicate, i, pageSize, semaphore));

                if (amountRunTask > maxConcurrency)
                {
                    amountRunTask = 0;
                    while (tasks.Count > 0)
                    {
                        var result = await Task.WhenAll(tasks);
                        foreach (var item in result)
                        {
                            allResults.AddRange(item);
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }
            }

            var result2 = await Task.WhenAll(tasks);
            foreach (var item in result2)
            {
                allResults.AddRange(item);
            }
            tasks.RemoveAll(x => x.IsCompleted);

            return allResults;
        }

        public async Task<PaginationUtility<ProductEtsyDto>> GetData(PaginationParam pagination, string? keyword, bool isPaging)
        {
            try
            {

                var predicate = PredicateBuilder.New<ProductEtsy>(x => x.quantity > 0);
                if (!string.IsNullOrEmpty(keyword))
                {
                    predicate.And(x => x.title.Contains(keyword) || x.description.Contains(keyword)
                        || x.who_made.Contains(keyword) || x.item_sku.Contains(keyword)
                        || x.when_made.Contains(keyword));
                }

                var query = _productEstyRepository.FindAll(predicate, x => x.materials, x => x.tags, x => x.styles, x => x.image_ids).ProjectTo<ProductEtsyDto>(_mapperConfiguration);
                var result = await PaginationUtility<ProductEtsyDto>.CreateAsync(query, pagination.PageNumber, pagination.PageSize, isPaging);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ProductEtsyDto>> GetData(ExpressionStarter<ProductEtsy> predicate, int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IProductEtsyRepository>();
                    var queryTask = service.FindAll(predicate, x => x.materials, x => x.tags, x => x.styles, x => x.image_ids).ProjectTo<ProductEtsyDto>(_mapperConfiguration);
                    var data = await PaginationUtility<ProductEtsyDto>.CreateAsync(queryTask, pageNumber, pageSize, true);
                    return data.Result;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<ProductEtsyDto> GetDataDetail(string item_sku, string? keyword)
        {
            if (string.IsNullOrEmpty(item_sku))
                return null;
            var predicate = PredicateBuilder.New<ProductEtsy>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                predicate.And(x => x.title.Contains(keyword) || x.description.Contains(keyword)
                    || x.who_made.Contains(keyword) || x.item_sku.Contains(keyword)
                    || x.when_made.Contains(keyword));
            }
            var query = await _productEstyRepository.FirstOrDefaultAsync(predicate, x => x.materials, x => x.tags, x => x.styles, x => x.image_ids);
            return _mapper.Map<ProductEtsyDto>(query);
        }

        public async Task<OperationResult> UpdateListingIdProduct(ProductEtsyDto dto)
        {
            var checkData = _productEstyRepository.FirstOrDefault(x => x.item_sku == dto.item_sku);
            if (checkData != null)
            {
                checkData.listing_id = dto.listing_id;
                _productEstyRepository.Update(checkData);
                try
                {
                    await _productEstyRepository.SaveChangesAsync();
                    return new OperationResult { Success = true };
                }
                catch (Exception)
                {

                    return new OperationResult { Success = false };
                }
            }
            return new OperationResult { Success = false, Message = "No Data" };
        }
    }
}
