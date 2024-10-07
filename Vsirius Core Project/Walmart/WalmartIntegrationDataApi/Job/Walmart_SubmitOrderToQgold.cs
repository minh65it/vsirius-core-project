using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_SubmitOrderToQgold : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_SubmitOrderToQgold(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.SendWalmartOrderToQgold();
            return;
        }
    }
}
