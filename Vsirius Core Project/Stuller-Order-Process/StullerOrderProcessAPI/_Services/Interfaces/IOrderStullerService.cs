using StullerOrderProcessAPI.Common.Helpers;
using StullerOrderProcessAPI.Dtos;

namespace StullerOrderProcessAPI._Services.Interfaces
{
    public interface IOrderStullerService
    {
        Task<PaginationUtility<StullerOrderDTO>> GetOrders(PaginationParam paginationParam, SearchOrderStullerParam searchParam);
        Task<StullerOrderDTO?> GetDetailOrder(string id);
        Task<bool> DeleteOrders(List<string> ds);
        Task<bool?> SubmitStuller(SubmitOrderParam order, bool isResubmit = false);
        Task<bool> MapOrder(List<MapOrderParam> param);
        Task<List<UnSubmittedStullerOrder>> GetOrderNeedToSubmit();
    }
}
