using EtsyIntegrationDataAPI.Dtos;
using EtsyIntegrationDataAPI.Dtos.OrderEtsy;
using EtsyIntegrationDataAPI.Helpers;
using EtsyIntegrationDataAPI.Parameters.OrderParmas;
using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderDetail(long orderId);
        PaginationUtil<OrderDto> GetAllOrderEtsy(SearchOrderParam search, PaginationParam pagination);
        Task<List<OrderDto>> GetShopReceipts(GetShopReceiptsParams? queries, int? pageNumber);
        Task<List<OrderDto>> GetNewOrderEtsy();
        Task<OperationResult> SubmitOrderQgold();
        Task<OperationResult> UpdateTrackingOrder();
        Task<OperationResult> SubmitOrderStuller();
        Task<OrderDto> CreateReceiptShipment(string authorization, ReceiptShipment dto);
    }
}
