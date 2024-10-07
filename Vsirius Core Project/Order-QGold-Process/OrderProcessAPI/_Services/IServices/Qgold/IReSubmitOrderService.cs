using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IReSubmitOrderService
    {
        //Task<OperationResult> AddReSubmitOrder(OrderMarketplaceDto returnOrder);
        Task<OperationResult> AddReSubmitOrder(List<QgoldFtpOrderObject> models);
    }
}
