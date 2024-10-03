using ProductApi.Controllers;
using Quartz;

namespace ProductApi.Jobs
{
    public class VSiriusWeb_UpdateFilter : IJob
    {
        private readonly TriggerController _triggerController;

        public VSiriusWeb_UpdateFilter(TriggerController triggerController)   
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.UpdateFilter();
        }
    }
}
