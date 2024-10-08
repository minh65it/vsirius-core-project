using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface ISubmitOrder
    {
        public Task<OperationResult> SubmitOrderToQgold();
        public Task<OperationResult> SubmitOrderToStuller();

    }
}
