using PimApi.Dtos;
using PimApi.Dtos.OrderDto;
using PimApi.Helpers.Utilities;

namespace PimApi._Services.Interfaces
{
    public interface IOrderService
    {
        Task<PaginationUtility<OrderDto>> GetOrder(PaginationParam pagination, SearchOrderParams searchParams);
        Task<PaginationUtility<ProductDto>> GetProductTop(PaginationParam pagination, SearchOrderParams searchParams);
    }
}
