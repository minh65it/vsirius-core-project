using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;


namespace PhillyLuxEcommerce.Controllers
{
    [RequestInfo("Page ProductDetail")]
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailService _productDetailService;
        private readonly IMapper _mapper;
        private readonly IWishlistService _wishlistService;
        private readonly ICartService _cartService;
        public ProductDetailController(IProductDetailService productDetailService, IMapper mapper, IWishlistService wishlistService, ICartService cartService)
        {
            _productDetailService = productDetailService;
            _mapper = mapper;
            _wishlistService = wishlistService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index(string productId)
        {
            VsiriusProductDto data = await _productDetailService.GetProduct(productId);
            if (data != null)
            {
                ProductDetailVM productDetailVM = _mapper.Map<ProductDetailVM>(data);

                List<VsiriusProductDto> lstData = await _productDetailService.GetRelatedProducts(productDetailVM.ItemType!);
                if (lstData != null)
                {
                    List<SummaryProductVM> lstRelated = _mapper.Map<List<SummaryProductVM>>(lstData);
                    List<CartDetailDto> listCart = new ();

                    productDetailVM.RelatedProducts = lstRelated;

                    var resWishlist = await _wishlistService.GetWishlistAllByUser();
                    var resCart = await _cartService.ListCartDetail();
                    if (resWishlist.Success)
                    {
                        List<WishlistVM> listWishlist = (List<WishlistVM>)resWishlist.Data;

                        if (resCart != null)
                        {
                            listCart = resCart.ListCartDetailsDtos;
                        }

                        var wishlistItem = listWishlist.FirstOrDefault(w => w.ProductId == productDetailVM.SKU);
                        if (wishlistItem != null)
                        {
                            productDetailVM.CheckWishlist = true;
                            productDetailVM.WishListId = wishlistItem.Id;
                        }

                        foreach (var product in lstRelated)
                        {
                            var wishlistItems = listWishlist.FirstOrDefault(w => w.ProductId == product.ProductId);
                            if (wishlistItems != null)
                            {
                                product.CheckWishlist = true;
                                product.WishListId = wishlistItems.Id;
                            }

                            var cartItem = listCart.FirstOrDefault(c => c.ProductId == product.ProductId);
                            if (cartItem != null)
                            {
                                product.CheckCart = true;
                            }
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Error");
                }

                return View(productDetailVM);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }


        public async Task<IActionResult> BuyItNow(string productId, int quantity)
        {
            await _cartService.AddToCart(productId, quantity);
            return Ok();
        }
    }
}
