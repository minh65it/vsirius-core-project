using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.ViewModels;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.Dtos;

namespace PhillyLuxEcommerce.Controllers
{
    public class TopProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;
        private readonly ITopProductService _topService;

        public TopProductController(ITopProductService topService, IMapper mapper, IWishlistService wishlistService, ICartService cartService)
        {
            _mapper = mapper;
            _cartService = cartService;
            _wishlistService = wishlistService;
            _topService = topService;
        }

        public async Task<IActionResult> Index(string? filter)
        {
            int pageNumber = 1;

            var searchParam = new SearchTopProductParam()
            {
                PageSize = 24,
                PageNumber = pageNumber,
                Filter = filter
            };

            SearchProductDto? searchData = await _topService.SearchProductDataAsync(searchParam);

            //Mapper ViewModel
            List<CartDetailDto> listCart = new List<CartDetailDto>();
            PaginationVM pagination = _mapper.Map<PaginationVM>(searchData!.pagination);
            List<SummaryProductVM> products = _mapper.Map<List<SummaryProductVM>>(searchData.result);

            var resCart = await _cartService.ListCartDetail();
            var resWishlist = await _wishlistService.GetWishlistAllByUser();
            if (resWishlist.Success)
            {
                List<WishlistVM> listWishlist = (List<WishlistVM>)resWishlist.Data;

                if (resCart != null)
                {
                    listCart = resCart.ListCartDetailsDtos;
                }

                foreach (var product in products)
                {
                    var wishlistItem = listWishlist.FirstOrDefault(w => w.ProductId == product.ProductId);
                    if (wishlistItem != null)
                    {
                        product.CheckWishlist = true;
                        product.WishListId = wishlistItem.Id;
                    }

                    var cartItem = listCart.FirstOrDefault(c => c.ProductId == product.ProductId);
                    if (cartItem != null)
                    {
                        product.CheckCart = true;
                    }
                }
            }

            TopProductVM searchView = new()
            {
                IsLatest = !string.IsNullOrWhiteSpace(filter),
                lstSearchProduct = products,
                pagination = pagination
            };
            return View(searchView);
        }

        [HttpPost]
        public async Task<IActionResult> GetProducts([FromBody] SearchTopProductParam searchParam)
        {
            searchParam.PageSize = 24;
            SearchProductDto? searchData = await _topService.SearchProductDataAsync(searchParam);

            //Mapper ViewModel
            PaginationVM pagination = _mapper.Map<PaginationVM>(searchData!.pagination);
            List<SummaryProductVM> products = _mapper.Map<List<SummaryProductVM>>(searchData.result);

            var searchVm = new TopProductVM()
            {
                lstSearchProduct = products,
                pagination = pagination
            };

            return PartialView("_TopProducts", searchVm);
        }
    }
}
