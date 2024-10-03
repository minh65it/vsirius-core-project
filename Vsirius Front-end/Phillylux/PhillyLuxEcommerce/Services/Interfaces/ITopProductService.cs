using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Dtos;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface ITopProductService
    {
        Task<SearchProductDto?> SearchProductDataAsync(SearchTopProductParam searchParam);
    }
}
