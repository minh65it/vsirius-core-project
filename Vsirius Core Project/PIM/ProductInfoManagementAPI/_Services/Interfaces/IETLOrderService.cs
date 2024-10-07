using PimApi.Helpers.Utilities;

namespace PimApi._Services.Interfaces
{
    public interface IETLOrderService
    {
        Task<OperationResult> ConvertOrder();
    }
}
