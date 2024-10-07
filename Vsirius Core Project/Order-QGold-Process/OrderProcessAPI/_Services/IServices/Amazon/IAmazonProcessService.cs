using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;

namespace OrderProcessApi._Services.IServices.Amazon
{
    public interface IAmazonProcessService
    {
        Task<PaginationResult<AmazonOrderDTO>> GetOrders(PaginationParam paginationParam, AmazonProcessSearchParam seachParam);
        Task<AmazonOrderDetailDTO?> GetDetailOrder(string id);
        Task<Result> DeleteOrders(List<string> ds);
        Task<OperationResult> ReSubmit(QgoldFtpOrderObject order);
        Task<OperationResult> GetNewOrders();
    }
}
