using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_UpdateInventory : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_UpdateInventory(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.UpdateInventory();
            return;
        }
    }
}
