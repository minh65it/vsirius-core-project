using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.DtosOrderStuller;


namespace WalmartIntegrationDataApi._Services.Interfaces
{
    public interface IOrderStullerService
    {
        Task<PaginationUtility<StullerOrderDTO>> GetOrders(PaginationParam paginationParam, SearchOrderStullerParam searchParam);
    }
}
