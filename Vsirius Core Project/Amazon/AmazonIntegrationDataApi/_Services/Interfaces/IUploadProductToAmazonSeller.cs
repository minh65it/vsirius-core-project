using AmazonIntegrationDataApi.Helpers.Utilities;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface IUploadProductToAmazonSeller
    {
        Task<OperationResult> UploadProductToAmazon();
        Task<OperationResult> UpdateInventory();
        Task<OperationResult> UpdatePrice();
        Task<OperationResult> RemoveProduct();
    }
}
