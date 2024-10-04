using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_MapPIMToAmazonDB : IJob
    {
        private readonly MapVSiriusDBToAmazonDBController _mapVSiriusDBToAmazonDBController;

        public Amazon_MapPIMToAmazonDB(MapVSiriusDBToAmazonDBController mapVSiriusDBToAmazonDBController)
        {
            _mapVSiriusDBToAmazonDBController = mapVSiriusDBToAmazonDBController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _mapVSiriusDBToAmazonDBController.MapToAmazonDB().Wait();
            return Task.CompletedTask;
        }
    }
}
