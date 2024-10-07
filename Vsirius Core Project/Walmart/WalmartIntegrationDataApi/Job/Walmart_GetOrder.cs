using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_GetOrder : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_GetOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.GetOrderWalmart();
            return ;
        }
    }
}
