using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Order;
using WalmartIntegrationDataApi.Dtos.Return;
namespace WalmartIntegrationDataApi._Services.Interfaces
{
    public interface IOrderWalmartService
    {
        Task<PaginationUtility<ReturnOrderDTO>?> GetReturnOrders(PaginationParam paginationParam, SearchReturnParam search);
        Task<ReturnOrderDTO?> GetDetailReturnOrder(string id);
        Task<PaginationUtility<OrderDetailDTO>> GetOrders(PaginationParam paginationParam, WalmartOrderSearchParam seachParam);
        Task<OrderDetailDTO?> GetDetailOrder(string id);
        Task<OperationResult> DeleteOrders(List<string> ds);
        Task<List<UnshipOrderDto>> GetUnshippedOrderIds(UnshippedOrderIdsParam param);
    }
}
