using PimApi.Controllers;
using Quartz;

namespace PimApi.Jobs
{
    public class PIM_ETLOrder : IJob
    {
        private readonly ETLOrderController _eTLOrderController;
        public PIM_ETLOrder(ETLOrderController eTLOrderController)
        {
            _eTLOrderController = eTLOrderController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _eTLOrderController.ConvertOrder().Wait();
            return Task.CompletedTask;
        }
    }
}
