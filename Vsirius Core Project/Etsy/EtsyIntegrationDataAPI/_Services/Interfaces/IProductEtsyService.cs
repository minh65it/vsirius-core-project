using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using LinqKit;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IProductEtsyService
    {
        public Task<PaginationUtility<ProductEtsyDto>> GetData(PaginationParam pagination, string? keyword, bool isPaging);
        public Task<List<ProductEtsyDto>> GetData(ExpressionStarter<ProductEtsy> predicate, int pageNumber, int pageSize, SemaphoreSlim semaphore);
        public Task<List<ProductEtsyDto>> GetAllData(string? keyword, bool? checkQuantity, bool? isDraft);
        public Task<ProductEtsyDto> GetDataDetail(string item_sku, string? keyword);
        public Task<OperationResult> UpdateListingIdProduct(ProductEtsyDto dto);
    }
}
