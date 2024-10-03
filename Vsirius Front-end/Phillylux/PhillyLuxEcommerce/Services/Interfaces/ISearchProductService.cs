using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Dtos.Params;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface ISearchProductService
    {
        Task<SearchProductDto?> SearchProductDataAsync(SearchParam searchParam);
        Task<List<FilterDto>?> FilterDataAsync(string? feedProductType);
    }
}
