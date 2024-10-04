using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_UpdateInventoryToAmazon : IJob
    {
        private readonly UploadProductToAmazonSellerController _uploadProductToAmazonSellerController;

        public Amazon_UpdateInventoryToAmazon(UploadProductToAmazonSellerController uploadProductToAmazonSellerController)
        {
            _uploadProductToAmazonSellerController = uploadProductToAmazonSellerController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _uploadProductToAmazonSellerController.UpdateInventory().Wait();
            return Task.CompletedTask;
        }
    }
}
