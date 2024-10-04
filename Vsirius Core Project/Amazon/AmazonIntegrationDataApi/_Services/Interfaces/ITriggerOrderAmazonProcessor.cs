using AmazonIntegrationDataApi.Helpers.Utilities;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface ITriggerOrderAmazonProcessor
    {
        Task<OperationResult> GetNewOrder();
        Task<OperationResult> GetOrderShipped();
        Task<OperationResult> UpdateTrackingOrder();
        Task<OperationResult> SubmitOrderToQgold();
        Task<OperationResult> SubmitOrderToStuller();
    }
}
