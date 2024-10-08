using EbayApi.Dtos;
using EbayApi.Utils;
using System.Threading.Tasks;

namespace EbayApi.Services.Interfaces
{
    public interface IEbayOrderService
    {
        Task<OrderDto> GetOrder(string orderId);
        Task<PaginationUtil<OrderDto>> SearchOrders(PaginationParam pagination, SearchParam search);
    }
}
