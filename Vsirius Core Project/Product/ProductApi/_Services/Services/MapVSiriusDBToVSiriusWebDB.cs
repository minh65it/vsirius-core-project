using AutoMapper;
using EFCore.BulkExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.httpClient;
using ProductApi.Helpers.Utilities;
using ProductApi.Jobs;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace ProductApi._Services.Services
{
    public class MapVSiriusDBToVSiriusWebDB : IMapVSiriusDBToVSiriusWebDB
    {
        private readonly IVSiriusProductApiClient _vSiriusProductApiClient;
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IVsiriusFilterRepository _vsiriusFilterRepository;
        private readonly IProductAIClient _productAIClient;
        private readonly IElasticSearchProductService _elasticSearchProductService;
        private readonly ValidationVsirius _valid;
        private readonly IMapper _mapper;
        private readonly VsiriusContext _context;

        public MapVSiriusDBToVSiriusWebDB(
            IVSiriusProductApiClient vSiriusProductApiClient,
            ValidationVsirius valid,
            IMapper mapper,
            VsiriusContext context,
            IProductAIClient productAIClient,
            IVsiriusProductRepository vsiriusProductRepository,
            IVsiriusFilterRepository vsiriusFilterRepository,
            IElasticSearchProductService elasticSearchProductService)
        {
            _vSiriusProductApiClient = vSiriusProductApiClient;
            _valid = valid;
            _mapper = mapper;
            _context = context;
            _productAIClient = productAIClient;
            _vsiriusProductRepository = vsiriusProductRepository;
            _vsiriusFilterRepository = vsiriusFilterRepository;
            _elasticSearchProductService = elasticSearchProductService;
        }

        public async Task<OperationResult> MapToVSiriusDB()
        {
            try
            {
                //await _elasticSearchProductService.DeleteAllProductInElastic();
                try
                {
                    await Common.WriteLogAsync("Map data VSiriusDB to VSiriusWeb START //======================================================");
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
                
                var pageSize = 500;
                var dataFirstCall = await _vSiriusProductApiClient.GetPaginationResult_RFT(1, pageSize, new SemaphoreSlim(2));
                var totalPages = 0;
                //call api AI
                var dataAI = await _productAIClient.GetProductAI();
                //first data
                if (dataFirstCall != null)
                {
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    if (dataAI != null)
                    {
                        dataFirstCall.Result = AddContentAI(dataFirstCall.Result, dataAI);
                    }
                    await Insert(dataFirstCall.Result);
                }
                else
                {
                    await Common.WriteLogAsync($"Get data first null");
                    return new OperationResult { Success = false };
                }

                //List rest data
                int maxConcurrency = 3;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
                bool checkNull = false;
                for (int i = 2; i <= totalPages; i++)
                {
                    // using async await
                    var result = await _vSiriusProductApiClient.GetPaginationResult_RFT(i, pageSize, semaphore);
                    if (result == null)
                    {
                        Thread.Sleep(5000);
                        result = await _vSiriusProductApiClient.GetPaginationResult_RFT(i, pageSize, semaphore);
                    }
                    if (result == null)
                    {
                        checkNull = true;
                    }
                    else
                    {
                        await Common.WriteLogAsync($"Get data page: {pageSize}");
                        if (dataAI != null)
                        {
                            result.Result = AddContentAI(result.Result, dataAI);
                        }
                        await Insert(result.Result);
                    }
                }
                //Delete or Update quantity
                DateTime updateTime = checkNull ? DateTime.Now.AddHours(-12) : DateTime.Now.AddHours(-2);
                var predicate = PredicateBuilder.New<VsiriusProduct>(x => x.UpdatedDate <= updateTime && x.IsDelete == false);
                var querySKUDelete = await _context.VsiriusProduct.Where(predicate).Select(x => x.item_sku).ToListAsync();
                if (querySKUDelete.Count >= 0)
                {
                    if (querySKUDelete.Count >= 1000)
                    {
                        querySKUDelete = querySKUDelete.Take(1000).ToList();
                    }
                    var curDate = DateTime.Now;
                    await _context.VsiriusProduct.Where(x => querySKUDelete.Contains(x.item_sku))
                         .ExecuteUpdateAsync(s => s
                         .SetProperty(i => i.IsDelete, i => true)
                         .SetProperty(i => i.UpdatedDate, i => curDate)
                         .SetProperty(i => i.quantity, i => "0")
                         );
                }
                await Common.WriteLogAsync("Map data VSiriusDB to VSiriusWeb DONE //======================================================== And run check Filter");
                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {

                await Common.WriteLogAsync($"Job VSiriusWeb_MapPIMToVSiriusWebDB");
                await Common.WriteLogAsync($"Get Data Failed with exception: {ex.Message} ");
                return new OperationResult { Success = false };
                throw new Exception(ex.ToString());
            }
        }
        public async Task<OperationResult> Insert(List<VsiriusProductDto> input)
        {
            if (input == null)
                return new OperationResult(true);

            var products = new List<VsiriusProductDto>();
            var notifications = new List<VsiriusProductDto>();

            for (int row = 0; row < input?.Count(); row++)
            {
                input[row].external_product_id = null;
                input[row].external_product_id_type = null;
                if (!string.IsNullOrWhiteSpace(input[row].brand_name))
                {
                    var msg = _valid.IsValid(input[row]);
                    if (IsAllPropertiesNull(msg))
                    {
                        products.Add(input[row]);
                    }
                    else
                    {
                        msg.item_sku = input[row].item_sku;
                        notifications.Add(msg);
                    }
                }
            };

            try
            {
                if (notifications.Count > 0)
                {
                    return new OperationResult { Success = false, Data = notifications };
                }

                var lstProduct = _mapper.Map<List<VsiriusProduct>>(products);

                await InsertSQLDatabase(lstProduct);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Insert page error: {ex.ToString()}");
                return new OperationResult(false, message: ex.Message);
            }
        }
        public bool IsAllPropertiesNull(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value != null)
                {
                    return false;
                }
            }

            return true;
        }
        public static List<VsiriusProductDto> AddContentAI(List<VsiriusProductDto> input, List<ProductAiDto> listAI)
        {
            foreach (var item in input)
            {
                var aiProduct = listAI.FirstOrDefault(x => x.SKU == item.item_sku);
                if (aiProduct != null)
                {
                    item.item_name = aiProduct.ProductName;
                    item.generic_keywords = aiProduct.Keywords;
                    item.bullet_point1 = aiProduct.BulletPoint1;
                    item.bullet_point2 = aiProduct.BulletPoint2;
                    item.bullet_point3 = aiProduct.BulletPoint3;
                    item.bullet_point4 = aiProduct.BulletPoint4;
                    item.bullet_point5 = aiProduct.BulletPoint5;
                    item.bullet_point6 = aiProduct.BulletPoint6;
                    item.bullet_point7 = aiProduct.BulletPoint7;
                    item.bullet_point8 = aiProduct.BulletPoint8;
                    item.bullet_point9 = aiProduct.BulletPoint9;
                    item.brand_name = aiProduct.Brand;
                }
            }
            return input;
        }

        public async Task<bool> InsertSQLDatabase(List<VsiriusProduct> lstProduct)
        {
            var curDate = DateTime.Now;
            lstProduct.ForEach(x => x.UpdatedDate = curDate);
            if (lstProduct.Count > 0)
            {
                int batchSize = 10000; // Số lượng tối đa trong mỗi danh sách con
                int numBatches = (int)Math.Ceiling((double)lstProduct.Count / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var batch = lstProduct
                        .Skip(i * batchSize)
                        .Take(batchSize)
                        .ToList();

                    await _context.BulkInsertOrUpdateAsync(batch);

                    await _elasticSearchProductService.BulkInsertProductToElastic(batch);
                }
                await Common.WriteLogAsync($"Insert {lstProduct.Count} to Database VSiriusWeb");
            }
            return true;
        }

        public static Expression<Func<T, bool>> CreateFieldCheckLambda<T>(string fieldName, List<string> l)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            var constant = Expression.Constant(l);
            var containsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(constant, containsMethod, property);
            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }
        public async Task<OperationResult> UpdateFilter()
        {
            try
            {
                //Check feed_product_type
                var checkFeedProductType = await _vsiriusFilterRepository
                    .FindAll(x => x.IsDelete == false && x.FieldName == "feed_product_type")
                    .GroupBy(x => x.FieldName)
                    .Select(x => new { Key = x.Key, List = x.ToList() }).FirstOrDefaultAsync();
                if (checkFeedProductType != null)
                {
                    await _context.VsiriusFilter.Where(x => x.FieldName == "feed_product_type")
                                        .ExecuteUpdateAsync(s => s
                                        .SetProperty(i => i.IsActive, i => false)
                                        );

                    foreach (var i in checkFeedProductType.List)
                    {
                        var pre = PredicateBuilder.New<VsiriusProduct>(x => x.quantity != "0" && x.feed_product_type == i.Value && x.IsDelete != true);
                        VsiriusProductDto dto = new();
                        var propertiesValid = dto.GetType().GetProperty(i.FieldName)!.Name;
                        var lambda = CreateFieldCheckLambda<VsiriusProduct>(propertiesValid, new List<string>() { i.Value });
                        pre = pre.And(lambda);
                        var checkValue = await _vsiriusProductRepository.AnyAsync(pre);

                        if (checkValue)
                        {
                            await _context.VsiriusFilter.Where(x => x.Value == i.Value && x.FieldName == checkFeedProductType.Key)
                                    .ExecuteUpdateAsync(s => s
                                    .SetProperty(i => i.IsActive, i => true)
                                    );
                        }
                    }
                }

                //Check Item_type and Metal_type (and...)
                var checkFilterName = await _vsiriusFilterRepository
                    .FindAll(x => x.IsDelete == false && (x.FieldName == "metal_type" || x.FieldName == "item_type"))
                    .GroupBy(x => x.ProductType)
                    .Select(x => new { Key = x.Key, List = x.ToList() }).ToListAsync();
                foreach (var item in checkFilterName)
                {
                    if (!string.IsNullOrWhiteSpace(item.Key))
                    {
                        await _context.VsiriusFilter.Where(x => x.ProductType == item.Key)
                                    .ExecuteUpdateAsync(s => s
                                    .SetProperty(i => i.IsActive, i => false)
                                    );

                        foreach (var i in item.List)
                        {
                            var pre = PredicateBuilder.New<VsiriusProduct>(x => x.quantity != "0" && x.feed_product_type == item.Key && x.IsDelete != true);
                            VsiriusProductDto dto = new();
                            var propertiesValid = dto.GetType().GetProperty(i.FieldName)!.Name;
                            var lambda = CreateFieldCheckLambda<VsiriusProduct>(propertiesValid, new List<string>() { i.Value });
                            pre = pre.And(lambda);
                            var checkValue = await _vsiriusProductRepository.AnyAsync(pre);

                            if (checkValue)
                            {
                                await _context.VsiriusFilter.Where(x => x.Value == i.Value && x.ProductType == item.Key && x.FieldName == i.FieldName)
                                        .ExecuteUpdateAsync(s => s
                                        .SetProperty(i => i.IsActive, i => true)
                                        );
                            }
                        }

                    }
                }

                return new OperationResult() { Success = true };
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Update Filter error: {ex.ToString()}");
                return new OperationResult() { Success = false };
            }

        }
    }
}
