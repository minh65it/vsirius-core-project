using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Interfaces
{
    public interface IVsiriusProductService
    {
        Task<OperationResult> CreateProduct(VsiriusProductDto model);
        Task<OperationResult> ImportFile(IFormFile fileExcel );
        Task<PaginationUtility<VsiriusProductDto>> GetAll(List<VsiriusFilterParamDto> filterParam, PaginationParam pagination, string fullTextSearch);
        Task<PaginationUtility<VsiriusProductDto>> GetAllLayer1(string layer1);
        Task<VsiriusProductDto> GetProduct(string item_sku);
        Task<OperationResult> RemoveProduct(string item_sku);
        Task<OperationResult> UpdateProduct(VsiriusProductDto model);
        Task<List<string>> GetAllItemSKU(string item_sku_category);
        Task<OperationResult> RemoveMultipleProduct(List<string> listSKU);
        //
        Task<List<VsiriusFilterParamDto>> GetFilter(string feed_product_type);
        Task<List<VsiriusFilterParamDto>> GetValueForAdd(string feed_product_type);
    }
}
