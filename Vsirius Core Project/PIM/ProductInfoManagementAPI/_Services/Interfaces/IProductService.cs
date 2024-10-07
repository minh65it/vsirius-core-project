using PimApi.Dtos;
using PimApi.Helpers.Utilities;

namespace PimApi._Services.Interfaces
{
    public interface IProductService
    {
        Task<OperationResult> CreateProduct(ProductDto model);
        Task<OperationResult> ImportFile(IFormFile fileExcel);
        Task<PaginationUtility<VsiriusProductSearchDto>> GetAll(GetAllParmas filterParam, PaginationParam pagination);
        Task<ProductDto> GetProduct(string item_sku);
        Task<OperationResult> RemoveProduct(string item_sku);
        Task<OperationResult> UpdateProduct(ProductDto model);
        Task<List<string>> GetAllItemSKU(string item_sku_category);
        Task<PaginationUtility<ProductDto>> GetAllItemMartketplace(Marketplace martketplace, PaginationParam pagination);
        Task<OperationResult> UpdateMartketplace(Marketplace martketplace, List<string> listSKU, bool isValue);
        Task<OperationResult> RemoveMultipleProduct(List<string> listSKU);
        Task<PaginationUtility<ProductDto>> GetData(PaginationParam pagination, SearchParams searchParams, bool isPagging);
        Task<PaginationUtility<ProductDto>> GetData(PaginationParam pagination, string? keyword, bool v);
    }
}
