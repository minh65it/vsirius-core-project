using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Services.Interfaces;
using System.Security.Claims;

namespace PhillyLuxEcommerce.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<ActionResult> AddWishlist(string productId)
        {
            var res = await _wishlistService.AddWishlist(productId);
            if (res.Success)
            {
                return Json(res.Data);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWishlist(string idWishlist)
        {
            var res = await _wishlistService.DeleteWishlist(idWishlist);
            if (res.Success)
            {
                return Json(res.Data);
            }
            return Json(res.Data);
        }


        [HttpGet]
        public async Task<ActionResult> CountWishlist()
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var res = await _wishlistService.GetCountWishlistUser();
            if (res.Success)
            {
                return Json(res.Data);
            }
            return Json(null);
        }
    }
}
