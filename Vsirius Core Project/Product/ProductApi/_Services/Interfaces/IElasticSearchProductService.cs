using Nest;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace ProductApi._Services.Interfaces
{
    public interface IElasticSearchProductService
    {
        Task BulkInsertProductToElastic(List<VsiriusProduct> elasticProducts);
        Task<PaginationUtility<VsiriusProductSearchDto>> GetListSkuFromTextSearch(
            List<VsiriusFilterParamDto>? filterParam,
            PaginationParam pagination,
            SearchProductParam searchParam);
        Task DeleteAllProductInElastic();
        Task<PaginationUtility<VsiriusProductDto>> GetAllBackOffice(
            List<VsiriusFilterParamDto>? filterParam, 
            PaginationParam pagination, 
            SearchProductParam searchParam);

        ElasticClient client { get; set; }
    }
}
