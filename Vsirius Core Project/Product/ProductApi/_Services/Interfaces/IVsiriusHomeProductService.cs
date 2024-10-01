using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Interfaces
{
    public interface IVsiriusHomeProductService
    {
        Task<OperationResult> CreateUpdateHomeProduct(VsiriusHomeProductDto dto);
        Task<List<VsiriusProductDto>> GetAllHomeProduct(VsiriusHomeProductParams param);
        Task<List<VsiriusHomeProductDto>> GetAllHomeProductBackend();
        Task<List<VsiriusProductDto>> GetRelatedProduct(VsiriusHomeProductParams param);
        Task<OperationResult> RemoveHomeProduct(string item_sku);
    }
}
