using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_MapVsiriusToWalmart : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_MapVsiriusToWalmart(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.MapProduct();
            return;
        }
    }
}
