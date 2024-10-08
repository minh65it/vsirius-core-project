using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IListingInventoryService
    {
        Task<OperationResult> UpdateListingInventory(string authorization, long listing_id);
        Task<OperationResult> UpdateAllListingInventory();
    }
}
