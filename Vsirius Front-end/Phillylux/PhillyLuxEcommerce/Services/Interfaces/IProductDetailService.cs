using PhillyLuxEcommerce.Dtos;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IProductDetailService
    {
        Task<VsiriusProductDto> GetProduct(string productId);
        Task<List<VsiriusProductDto>> GetRelatedProducts(string filter);
    }
}
