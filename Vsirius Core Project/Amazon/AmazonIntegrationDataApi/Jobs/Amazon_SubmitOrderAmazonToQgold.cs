using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_SubmitOrderAmazonToQgold : IJob
    {
        private readonly TriggerOrderAmazonProcessorController _orderAmazonProcessorController;

        public Amazon_SubmitOrderAmazonToQgold(TriggerOrderAmazonProcessorController orderAmazonProcessorController)
        {
            _orderAmazonProcessorController = orderAmazonProcessorController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _orderAmazonProcessorController.SubmitOrderToQgold().Wait();
            return Task.CompletedTask;
        }
    }
}
