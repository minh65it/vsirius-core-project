using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;

namespace OrderProcessApi._Services.IServices.Walmart
{
    public interface IWalmartProcessService
    {
        Task<PaginationResult<WalmartOrderDetailDTO>> GetOrders(PaginationParam paginationParam, WalmartProcessSearchParam seachParam);
        Task<WalmartOrderDetailDTO?> GetDetailOrder(string id);
        Task<Result> DeleteOrders(List<string> ds);
        Task<bool> LastestSellerOrder();
        Task<OperationResult> ReSubmit(QgoldFtpOrderObject order);
    }
}
