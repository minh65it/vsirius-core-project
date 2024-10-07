using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_UpdatePrice : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_UpdatePrice(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.UpdatePrice();
            return ;
        }
    }
}
