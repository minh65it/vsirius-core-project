using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IQgoldProcessService
    {
        Task<PaginationResult<QgoldOrderDTO>> GetOrders(PaginationParam paginationParam, SearchQgoldProcessParam seachParam);
        Task<QgoldOrderDetailDTO?> GetDetailOrder(string id);
        Task<Result> DeleteOrders(List<string> ds);
        Task<bool> UpdateReOrderViaName(List<OrderMarketplaceParams> models);
        Task<PaginationResult<QgoldOrderDTO>> GetReOrders(PaginationParam paginationParam, SearchQgoldProcessParam seachParam);
    }
}
