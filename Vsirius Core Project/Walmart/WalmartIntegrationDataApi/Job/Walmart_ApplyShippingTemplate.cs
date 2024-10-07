using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_ApplyShippingTemplate : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_ApplyShippingTemplate(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.ApplyShippingTemplate();
            return;
        }
    }
}
