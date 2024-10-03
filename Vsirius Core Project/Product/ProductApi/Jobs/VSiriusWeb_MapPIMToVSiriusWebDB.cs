using ProductApi.Controllers;
using Quartz;

namespace ProductApi.Jobs
{
    public class VSiriusWeb_MapPIMToVSiriusWebDB : IJob
    {
        private readonly TriggerController _triggerController;

        public VSiriusWeb_MapPIMToVSiriusWebDB(TriggerController triggerController)   
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.MapToAmazonDB();
        }
    }
}
