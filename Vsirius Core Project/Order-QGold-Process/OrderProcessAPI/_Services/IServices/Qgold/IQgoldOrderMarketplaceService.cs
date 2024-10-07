using OrderProcessApi.DTOs;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IQgoldOrderMarketplaceService
    {
        Task<PaginationResult<OrderMarketplaceDto>> GetOrder(OrderMarketplaceParams seachParam, PaginationParam page);
        Task<List<OrderMarketplace>> GetRecentOrder(int day);
        Task<OrderMarketplaceDto> DetailOrderBySellerId(string orderSellerId);
        Task<OrderMarketplaceDto> DetailOrderById(string orderId);
        Task<bool> UpdateOrder(List<OrderMarketplace> models);
        Task<bool> UpdateOrderViaName(List<OrderMarketplaceParams> models);
        Task<bool> AddOrder(List<QgoldFtpOrderObject> models);
        Task<bool> DeleteOrder(List<string> orderid);
    }
}
