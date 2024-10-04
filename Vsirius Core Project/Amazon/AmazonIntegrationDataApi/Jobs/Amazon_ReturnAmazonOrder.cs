using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_ReturnAmazonOrder : IJob
    {
        private readonly ReturnOrderController _returnOrderController;

        public Amazon_ReturnAmazonOrder(ReturnOrderController returnOrderController)
        {
            _returnOrderController = returnOrderController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _returnOrderController.GetNewReturnOrders().Wait();
            return Task.CompletedTask;
        }
    }
}
