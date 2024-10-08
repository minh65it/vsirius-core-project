using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_SubmitOrderQgold : IJob
    {
        private readonly OrderController _orderController;

        public Etsy_SubmitOrderQgold(OrderController orderController)
        {
            _orderController = orderController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _orderController.SubmitOrderQgold().Wait();
            return Task.CompletedTask;
        }
    }
}
