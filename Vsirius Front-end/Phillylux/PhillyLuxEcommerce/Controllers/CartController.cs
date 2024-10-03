using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using PhillyLuxEcommerce.Helpers.Utilities;

namespace PhillyLuxEcommerce.Controllers
{
    [RequestInfo("Page Cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;           
        }

        public async Task<IActionResult> Index()
        {
            CartDto? cart = await _cartService.ListCartDetail();

            return View(cart);
        }

        [HttpPost]
        public async Task<bool> UpdateCart(List<UpdatedCartsVM> lstData)
        {
            return await _cartService.UpdateCart(lstData);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            await _cartService.AddToCart(productId, quantity);
            return PartialView("_Cart");
        }
    }
}
