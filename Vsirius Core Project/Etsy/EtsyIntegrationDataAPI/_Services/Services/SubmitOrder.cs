using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class SubmitOrder : ISubmitOrder
    {
        public async Task<OperationResult> SubmitOrderToQgold()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> SubmitOrderToStuller()
        {
            throw new NotImplementedException();
        }
    }
}
