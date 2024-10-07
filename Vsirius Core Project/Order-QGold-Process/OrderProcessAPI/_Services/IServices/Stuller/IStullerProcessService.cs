using OrderProcessApi.Data;
using OrderProcessApi.DTOs;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IStullerProcessService
    {
        Task<PaginationResult<StullerOrderDTO>> GetOrders(PaginationParam paginationParam, SearchStullerProcessParam seachParam);
        Task<StullerOrderDTO?> GetDetailOrder(string id);
        Task<Result> DeleteOrders(List<string> ds);
    }
}
