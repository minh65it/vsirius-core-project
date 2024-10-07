using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Product;

namespace WalmartIntegrationDataApi._Services.Interfaces
{
    public interface IProductWalmartService
    {
        Task<OperationResult> GetAll(PaginationParam param);
        Task<OperationResult> Create(WalmartDTO data);
        Task<OperationResult> Update(WalmartDTO data);
        Task<OperationResult> Delete(List<string> id);
        Task<OperationResult> Export(PaginationParam param);
        Task<OperationResult> ImportFile(IFormFile file);
        Task<OperationResult> GetAllId();
        Task<OperationResult> BulkInsertUpdate(List<WalmartDTO> lstData);
        Task<OperationResult> ExportQuantity(PaginationParam param);
        Task<PaginationUtility<WalmartPriceInventoryDTO>> GetPriceInventory(PaginationParam param);
        Task<OperationResult> ExportPrice(PaginationParam param);
    }
}
