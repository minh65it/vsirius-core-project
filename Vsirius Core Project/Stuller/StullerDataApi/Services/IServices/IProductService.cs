using StullerApiClient.Models;
using StullerDataApi.Params;
using StullerDataApi.Utilities;

namespace StullerDataApi.Services.IServices
{
    public interface IProductService
    {
        //Task<GetProductsResponse> GetProduct(ProductsRequestPostFilter filter);
        Task<PaginationResult<Product>> Filter(ProductParams param, PaginationParam page);
        Task<Product> DetailProduct(string SKU);
        Task<bool> UpdateProduct(string SKU, Product model);
        Task<bool> DeleteProduct(string SKU);
    }
}
