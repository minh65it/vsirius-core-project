using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_UpdatePriceToAmazon : IJob
    {
        private readonly UploadProductToAmazonSellerController _uploadProductToAmazonSellerController;

        public Amazon_UpdatePriceToAmazon(UploadProductToAmazonSellerController uploadProductToAmazonSellerController)
        {
            _uploadProductToAmazonSellerController = uploadProductToAmazonSellerController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _uploadProductToAmazonSellerController.UpdatePrice().Wait();
            return Task.CompletedTask;
        }
    }
}
