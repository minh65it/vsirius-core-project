using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IPropertiesProductService
    {
        public Task<OperationResult> SeedDataPropertiesByTaxonomy(int taxonomy_id);
        public Task<OperationResult> UpdatePropertiesProduct(List<ProductPIMDto> products);
        public Task<OperationResult> UpdateListingPropertiesEtsy();
    }
}
