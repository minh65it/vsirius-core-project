using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_UpdateTrackingOrder : IJob
    {
        private readonly OrderController _orderController;

        public Etsy_UpdateTrackingOrder(OrderController orderController)
        {
            _orderController = orderController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _orderController.UpdateTrackingOrder().Wait();
            return Task.CompletedTask;
        }
    }
}
