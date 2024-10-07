using Quartz;
using WalmartIntegrationDataApi.Controllers;

namespace WalmartIntegrationDataApi.Job
{
    public class Walmart_UploadProductToSeller : IJob
    {
        private readonly TriggerController _triggerController;

        public Walmart_UploadProductToSeller(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.UploadProductToSeller();
            return ;
        }
    }
}
