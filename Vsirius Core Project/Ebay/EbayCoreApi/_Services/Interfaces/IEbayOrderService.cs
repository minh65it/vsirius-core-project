using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Dtos;

namespace EbayCoreApi._Services.Interfaces
{
    public interface IEbayOrderService
    {
        Task<OrderDto> GetOrder(string orderId);
        Task<PaginationUtil<OrderDto>> SearchOrders(PaginationParam pagination, SearchOrderParam search);
    }
}
