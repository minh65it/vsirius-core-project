using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_GetNewOrderEtsy : IJob
    {
        private readonly OrderController _orderController;

        public Etsy_GetNewOrderEtsy(OrderController orderController)
        {
            _orderController = orderController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _orderController.GetNewOrderEtsy().Wait();
            return Task.CompletedTask;
        }
    }
}
