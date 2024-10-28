using Quartz;
using StullerOrderProcessAPI.Controllers;

namespace StullerOrderProcessAPI.Jobs
{
    public class Stuller_GetOrder : IJob
    {
        private readonly TriggerController _triggerController;

        public Stuller_GetOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _triggerController.GetOrderStuller();
            return ;
        }
    }
}
