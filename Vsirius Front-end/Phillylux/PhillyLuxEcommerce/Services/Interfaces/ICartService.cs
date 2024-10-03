using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCart(string? email, string userId);
        Task<bool> AddToCart(string sku, int quantity);
        Task<bool> UpdateCart(List<UpdatedCartsVM> data);
        Task<CartDto?> ListCartDetail();
    }
}
