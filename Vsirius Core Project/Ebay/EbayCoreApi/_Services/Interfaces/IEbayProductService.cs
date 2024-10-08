using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Dtos;

namespace EbayCoreApi._Services.Interfaces
{
    public interface IEbayProductService
    {
        Task<PaginationUtil<ProductDto>> SearchProducts(PaginationParam pagination, SearchParam searchParam);
        Task<ResultUtil<ProductDto>> AddProduct(ProductDto dto);
        Task<ResultUtil<string>> DeleteProduct(string Skus);
        Task<ResultUtil<ProductDto>> UpdateProduct(ProductDto dto);
        Task<ProductDto> GetProduct(string Sku);
    }
}
