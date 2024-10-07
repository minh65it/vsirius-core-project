using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_GetReturnOrder : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_GetReturnOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.GetReturnOrderWalmart();
            return ;
        }
    }
}
