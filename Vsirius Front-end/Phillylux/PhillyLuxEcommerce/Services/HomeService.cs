using Moq;
using AutoMapper;
using System.Text;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using PhillyLuxEcommerce.Helpers.Utilities;

namespace PhillyLuxEcommerce.Services
{
    public class HomeService : IHomeService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWishlistService _wishlistService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        public HomeService(IHttpClientFactory httpClientFactory, IMapper mapper, IWishlistService wishlistService, ILogService logService,
            IHttpContextAccessor httpContextAccessor, ICartService cartService)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _wishlistService = wishlistService;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
        }

        public async Task<OperationResult> GetFilterByProductType()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("HomeFilterData");

                HttpResponseMessage response = await httpClient.GetAsync("GetFilterByProductType");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.data.ToObject<List<VsiriusFilterDto>>();

                    List<HomeVMFilter> listFilter = _mapper.Map<List<HomeVMFilter>>(result);

                    return new OperationResult { Success = true, Data = listFilter };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetAllHomeProduct(string filter, int takeProduct)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("HomeProductData");

                HttpResponseMessage response = await httpClient.GetAsync("GetAllHomeProduct?Filter=" + filter + "&TakeProduct=" + takeProduct);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.ToObject<List<VsiriusProductDto>>();

                    List<HomeVMProduct> listProduct = _mapper.Map<List<HomeVMProduct>>(result);
                    List<CartDetailDto> listCart = new List<CartDetailDto>();

                    var resWishlist = await _wishlistService.GetWishlistAllByUser();
                    var resCart = await _cartService.ListCartDetail();

                    if (resWishlist.Success)
                    {
                        List<WishlistVM> listWishlist = (List<WishlistVM>)resWishlist.Data;

                        if (resCart != null)
                        {
                            listCart = resCart.ListCartDetailsDtos ?? new List<CartDetailDto>();
                        }

                        foreach (var product in listProduct)
                        {
                            var wishlistItem = listWishlist.FirstOrDefault(w => w.ProductId == product.SKU);
                            if (wishlistItem != null)
                            {
                                product.CheckWishlist = true;
                                product.WishListId = wishlistItem.Id;
                            }

                            var cartItem = listCart.FirstOrDefault(c => c.ProductId == product.SKU);
                            if (cartItem != null)
                            {
                                product.CheckCart = true;
                            }
                        }
                    }

                    return new OperationResult { Success = true, Data = listProduct };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetFilterByProductTypeMoq()
        {
            var mockDataService = new Mock<IHomeService>();
            var mockDataList = new List<HomeVMFilter>
                {
                    new HomeVMFilter { Value = "Bracelet", ImageURL = "" },
                    new HomeVMFilter { Value = "Necklet", ImageURL = "" },
                };
            var operation = new OperationResult { Success = true, Data = mockDataList };
            mockDataService.Setup(x => x.GetFilterByProductType()).Returns(Task.FromResult(operation));

            var result = await mockDataService.Object.GetFilterByProductType();
            return result;
        }

        public async Task<OperationResult> GetProductMoq(string filter, int takeProduct)
        {
            var mockDataService = new Mock<IHomeService>();
            var mockDataList = new List<HomeVMProduct>
                {
                    new HomeVMProduct { SKU="1", ProductName = "Bracelet1", Price = "123.90", BulletPoint1 = "bullet1", main_image_url= "https://meteor.stullercloud.com/das/35321848?$xlarge$", CheckCart = true, CheckWishlist = true },
                    new HomeVMProduct { SKU="2",ProductName = "Bracelet2", Price = "234.90", BulletPoint1 = "bullet2", main_image_url= "https://meteor.stullercloud.com/das/35321848?$xlarge$", CheckCart = true, },
                    new HomeVMProduct { SKU="3",ProductName = "Bracelet3", Price = "345.90", BulletPoint1 = "bullet3", main_image_url= "https://meteor.stullercloud.com/das/35321848?$xlarge$",CheckWishlist = true, },

                };
            var operation = new OperationResult { Success = true, Data = mockDataList };
            mockDataService.Setup(x => x.GetAllHomeProduct(filter, takeProduct)).Returns(Task.FromResult(operation));

            var result = await mockDataService.Object.GetAllHomeProduct(filter, takeProduct);
            return result;
        }

        public async Task<OperationResult> LoginResult(LoginParam loginParams)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CheckLogin");

                var jsonContent = JsonConvert.SerializeObject(loginParams);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    return new OperationResult { Success = result.isLogin, Data = result.token };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> RegisterResult(RegisterParam registerParam)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RegisterAccount");
                var request = _httpContextAccessor.HttpContext?.Request;
                string url = $"{request?.Scheme}://{request?.Host}";
                httpClient.DefaultRequestHeaders.Add("OriginUrl", url);

                var jsonContent = JsonConvert.SerializeObject(registerParam);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    return new OperationResult { Success = result.success, Data = result.message };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetAllPages()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("GetAllPages");

                HttpResponseMessage response = await httpClient.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<PageInfoDto> result = JsonConvert.DeserializeObject<List<PageInfoDto>>(responseBody);

                    return new OperationResult { Success = true, Data = result };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
