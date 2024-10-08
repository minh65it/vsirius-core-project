using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_CreateListDraftAndPublishListing : IJob
    {
        private readonly ListingController _listingController;

        public Etsy_CreateListDraftAndPublishListing(ListingController listingController)
        {
            _listingController = listingController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _listingController.CreateListDraftAndPublishListing().Wait();
            return Task.CompletedTask;
        }
    }
}
