using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface IOrderAmazonProcessor
    {
        Task<PaginationResult<AmazonOrderDto>> GetOrders(PaginationParam paginationParam, AmazonProcessSearchParam searchParam);
        Task<AmazonOrderDetailDTO?> GetDetailOrder(string sellerOrderId);
        Task<OperationResult> DeleteOrders(List<string> ds);
        Task<OperationResult> ReSubmit(QgoldFtpOrderObject order);
        Task<List<UnshipOrderDto>> GetUnshippedOrderIds(UnshippedOrderIdsParam param);
    }
}
