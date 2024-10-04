using AmazonIntegrationDataApi.Controllers;
using Quartz;

namespace AmazonIntegrationDataApi.Jobs
{
    public class Amazon_UploadProductToAmazon : IJob
    {
        private readonly UploadProductToAmazonSellerController _uploadProductToAmazonSellerController;

        public Amazon_UploadProductToAmazon(UploadProductToAmazonSellerController uploadProductToAmazonSellerController)
        {
            _uploadProductToAmazonSellerController = uploadProductToAmazonSellerController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _uploadProductToAmazonSellerController.UploadProductToAmazon().Wait();
            return Task.CompletedTask;
        }
    }
}
