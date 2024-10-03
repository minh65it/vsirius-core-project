using LinqKit;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;
using System.Data.Entity;
using System.Linq.Expressions;
using VSiriusSystemLog.WriteLog;

namespace ProductApi._Services.Services
{
    public class VsiriusTopProductService : IVsiriusTopProductService
    {
        private readonly IVsiriusHomeProductRepository _vsiriusHomeProductRepository;
        private ILoggerCustom _logService;
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VsiriusTopProductService(
            IVsiriusHomeProductRepository vsiriusHomeProductRepository,
            ILoggerCustom logService,
            IVsiriusProductRepository vsiriusProductRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _vsiriusHomeProductRepository = vsiriusHomeProductRepository;
            _logService = logService;
            _vsiriusProductRepository = vsiriusProductRepository;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<PaginationUtility<VsiriusProductDto>> GetTopProduct(PaginationParam pagination, VsiriusTopParams param)
        {
            var predicate = PredicateBuilder.New<VsiriusHomeProduct>(x => x.IsActive == true);
            var predicateProduct = PredicateBuilder.New<VsiriusProduct>(x => x.IsDelete != true);
            if (param.Filter?.ToLower() == VsiriusHomeProductFilterParams.FilterPopular)
                predicate.And(x => x.IsPopular == true && x.IsDelete == false);
            if (param.Filter?.ToLower() == VsiriusHomeProductFilterParams.FilterLatest)
                predicate.And(x => x.IsLatest == true && x.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(param.feed_product_type))
            {
                List<string> feed_product_types = param.feed_product_type.Split('|').Select(x => x.Trim()).ToList();
                var lambda = CreateFieldCheckLambda<VsiriusProduct>("feed_product_type", feed_product_types);
                predicateProduct = predicateProduct.And(lambda);
            }

            try
            {
                IQueryable<VsiriusProductDto> data = (from home in _vsiriusHomeProductRepository.FindAll(predicate).OrderByDescending(o => o.UpdatedDate)
                        .Select(x => x.item_sku)
                        join products in _vsiriusProductRepository.FindAll(predicateProduct).Select(x => new VsiriusProduct()
                        {
                            item_sku = x.item_sku,
                            item_name = x.item_name,
                            bullet_point1 = x.bullet_point1,
                            standard_price = x.standard_price,
                            sale_price = x.sale_price,
                            color_name = x.color_name,
                            size_name = x.size_name,
                            main_image_url = x.main_image_url,
                            other_image_url1 = x.other_image_url1,
                            image_standard_1 = x.other_image_url2,
                            image_standard_2 = x.other_image_url3,
                            image_standard_3 = x.other_image_url4,
                            image_standard_4 = x.other_image_url5,
                            image_standard_5 = x.other_image_url6,
                            feed_product_type = x.feed_product_type,
                            item_type = x.item_type,
                            metal_type = x.metal_type,
                            model_name = x.model_name,
                            item_type_name = x.item_type_name,
                        })
                        on home equals products.item_sku
                        select new VsiriusProductDto()
                        {
                            item_sku = home,
                            item_name = products.item_name,
                            bullet_point1 = products.bullet_point1,
                            standard_price = products.standard_price.ToString("0.00"),
                            sale_price = products.sale_price,
                            color_name = products.color_name,
                            size_name = products.size_name,
                            main_image_url = products.main_image_url,
                            other_image_url1 = products.other_image_url1,
                            image_standard_1 = products.image_standard_1,
                            image_standard_2 = products.image_standard_2,
                            image_standard_3 = products.image_standard_3,
                            image_standard_4 = products.image_standard_4,
                            image_standard_5 = products.image_standard_5,
                            feed_product_type = products.feed_product_type,
                            item_type = products.item_type,
                            metal_type = products.metal_type,
                            model_name = products.model_name,
                            item_type_name = products.item_type_name,

                        });
                if (!string.IsNullOrWhiteSpace(param.keyword))
                {
                    var fullTextSearch = param.keyword.ToLower();
                    data = data.Where(x => x.item_name.ToLower().ToLower().Contains(fullTextSearch) || x.standard_price.ToLower().Contains(fullTextSearch)
                    || x.feed_product_type.ToLower().Contains(fullTextSearch) || x.item_sku.ToLower().Contains(fullTextSearch)
                    || x.item_type.ToLower().Contains(fullTextSearch) || x.metal_type.ToLower().Contains(fullTextSearch)
                    || x.model_name.ToLower().Contains(fullTextSearch) || x.color_name.ToLower().Contains(fullTextSearch)
                    || x.item_type_name.ToLower().Contains(fullTextSearch));
                }
                var result = await PaginationUtility<VsiriusProductDto>.CreateNolockAsync(data.AsNoTracking(), pagination.PageNumber, pagination.PageSize, true);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
