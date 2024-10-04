using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_UpdateTrackingOrderAmazon : IJob
    {
        private readonly TriggerOrderAmazonProcessorController _orderAmazonProcessorController;

        public Amazon_UpdateTrackingOrderAmazon(TriggerOrderAmazonProcessorController orderAmazonProcessorController)
        {
            _orderAmazonProcessorController = orderAmazonProcessorController;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _orderAmazonProcessorController.UpdateTrackingOrder().Wait();
            return Task.CompletedTask;
        }
    }
}
