using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Parameters.ListingsByShop;
using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IListingService
    {
        Task<ProductEtsyDto> CreateDraftListing(string authorization, ProductEtsyDto dto);
        Task<OperationResult> CreateListDraftAndPublishListing(int limits);
        Task<List<ProductEtsyDto>> GetListingsById(GetListingParams search);
        Task<List<ProductEtsyDto>> GetAllListingsByShop(string authorization);
        Task<ProductEtsyDto> UpdateListing(string authorization, ProductEtsyDto dto);
        Task<OperationResult> SyncEtsyAndDatabase();
    }
}
