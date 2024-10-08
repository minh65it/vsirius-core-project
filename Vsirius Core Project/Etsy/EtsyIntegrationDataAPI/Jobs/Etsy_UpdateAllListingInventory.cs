using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_UpdateAllListingInventory : IJob
    {
        private readonly ListingInventoryController _listingInventoryController;

        public Etsy_UpdateAllListingInventory(ListingInventoryController listingInventoryController)
        {
            _listingInventoryController = listingInventoryController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _listingInventoryController.UpdateAllListingInventory().Wait();
            return Task.CompletedTask;
        }
    }
}
