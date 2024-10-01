using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Params;
using OrdersAPI.Helpers.Utilities;

namespace OrdersAPI._Services.Interfaces
{
    public interface IVsiriusOrderService
    {
        Task<PaginationUtility<VsiriusOrderDto>> GetData(PaginationParam pagination, OrderParams param);
        Task<List<VsiriusOrderDto>> TopOrders();
        Task<VsiriusOrderDto> GetDetail(string orderId);
        Task<PaginationUtility<OrderHistory>> GetListOrderHistory(PaginationParam pagination, string email);
        Task<OperationResult> Create(VsiriusOrderDto dto);
        Task<OperationResult> Update(VsiriusOrderDto dto);
    }
}
