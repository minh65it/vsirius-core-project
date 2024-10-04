using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_GetNewAmazonOrder : IJob
    {
        private readonly TriggerOrderAmazonProcessorController _orderAmazonProcessorController;

        public Amazon_GetNewAmazonOrder(TriggerOrderAmazonProcessorController orderAmazonProcessorController)
        {
            _orderAmazonProcessorController = orderAmazonProcessorController;
        }

        public  Task Execute(IJobExecutionContext context)
        {
            _orderAmazonProcessorController.GetNewOrder().Wait();
            return Task.CompletedTask;
        }
    }
}
