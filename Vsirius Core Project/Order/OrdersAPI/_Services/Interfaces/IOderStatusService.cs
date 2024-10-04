using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Utilities;

namespace OrdersAPI._Services.Interfaces
{
    public interface IOderStatusService
    {
        Task<PaginationUtility<OrderStatusDto>> GetData(PaginationParam pagination, string keyword);
        Task<OperationResult> SeedDataStatus();
        Task<OperationResult> Create(OrderStatusDto dto);
        Task<OperationResult> Update(OrderStatusDto dto);
        Task<OperationResult> Delete(int id);
        Task<List<KeyValuePair<int,string>>> GetListStatus();
    }
}
