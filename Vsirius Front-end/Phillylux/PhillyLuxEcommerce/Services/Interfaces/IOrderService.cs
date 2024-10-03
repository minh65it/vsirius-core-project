using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OperationResult> GetCartByEmail(string email);
        Task<OperationResult> GetInfoUserByEmail(string email);
        Task<OperationResult> AddOrder(OrderInformationVM orderInfo, OrderCartVM orderCart);
        Task<OperationResult> GetDetailOrder(string orderId);
        Task<OperationResult> GetListOrderHistory(string email, int? pageNumber);
        Task<OperationResult> CancelOrder(OrderInformationVM orderInfo);
        Task<OperationResult> GetProductByOrderIdMoq();
        Task<OperationResult> GetInfomationdMoq();
    }
}
