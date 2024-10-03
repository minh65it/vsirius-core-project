using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Helpers.Utilities;
using Microsoft.CodeAnalysis;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.ViewModels;
using PhillyLuxEcommerce.Services.Interfaces;

namespace PhillyLuxEcommerce.Controllers
{
    [RequestInfo("Page Search")]
    public class SearchProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;
        public const string TempFisrtSearch = "_ProductType";
        private readonly ISearchProductService _searchProductService;

        public SearchProductController(ISearchProductService searchProductService, IMapper mapper, IWishlistService wishlistService, ICartService cartService)
        {
            _mapper = mapper;
            _cartService = cartService;
            _wishlistService = wishlistService;
            _searchProductService = searchProductService;
        }
        //[ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new string[] { "*" })]
        public async Task<IActionResult> Index(string keySearch, string productType)
        {
            int pageNumber = 1;
            ViewBag.Category = productType;
            //Process API
            List<FilterDto>? lstFilter = await _searchProductService.FilterDataAsync(productType);
            if (lstFilter == null)
            {
                return RedirectToAction("Index", "Error");
            }

            //first search
            var searchProductType = new FilterDto()
            {
                fieldName = "feed_product_type",
                labelName = "Product Type",
                value = new List<string>() { productType },
            };
            string json = JsonConvert.SerializeObject(searchProductType);

            if (TempData.ContainsKey(TempFisrtSearch))
            {
                // Key tồn tại trong TempData
                TempData.Remove(TempFisrtSearch);
            }
            TempData[TempFisrtSearch] = json;
            //HttpContext.Session.SetString(TempFisrtSearch, json);

            var searchParam = new SearchParam()
            {
                keyWord = keySearch,
                PageSize = 24,
                PageNumber = pageNumber,
                filters = new List<FilterDto>() { searchProductType }
            };

            SearchProductDto? searchData = await _searchProductService.SearchProductDataAsync(searchParam);

            //Mapper ViewModel
            PaginationVM pagination = _mapper.Map<PaginationVM>(searchData!.pagination);
            List<FilterVM> filters = _mapper.Map<List<FilterVM>>(lstFilter);
            List<SummaryProductVM> products = _mapper.Map<List<SummaryProductVM>>(searchData.result);
            List<CartDetailDto> listCart = new ();

            var resWishlist = await _wishlistService.GetWishlistAllByUser();
            var resCart = await _cartService.ListCartDetail();
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

            SearchVM searchView = new()
            {
                lstFilter = filters,
                lstSearchProduct = products,
                pagination = pagination
            };
            return View(searchView);
        }

        [HttpPost]
        //[ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new string[] { "*" })]
        public async Task<IActionResult> GetProducts([FromBody] SearchParam searchParam)
        {
            var json = TempData.Peek(TempFisrtSearch) as string;

            FilterDto? tempObjSearch = JsonConvert.DeserializeObject<FilterDto>(json!);
            searchParam.PageSize = 24;

            if (searchParam.filters != null)
            {
                searchParam.filters.Add(tempObjSearch!);
            }
            else
            {
                searchParam.filters = new() { tempObjSearch! };
            }
            SearchProductDto? searchData = await _searchProductService.SearchProductDataAsync(searchParam);

            //Mapper ViewModel
            PaginationVM pagination = _mapper.Map<PaginationVM>(searchData!.pagination);
            List<SummaryProductVM> products = _mapper.Map<List<SummaryProductVM>>(searchData.result);

            var searchVm = new SearchVM()
            {
                lstSearchProduct = products,
                pagination = pagination
            };

            return PartialView("_SearchProducts", searchVm);
        }
    }
}
