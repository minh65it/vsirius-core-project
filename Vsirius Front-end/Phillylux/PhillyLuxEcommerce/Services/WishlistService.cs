using AutoMapper;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Text;

namespace PhillyLuxEcommerce.Services
{

    public class WishlistService : IWishlistService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        public WishlistService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IUserService userService, IMapper mapper, ILogService logService)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<OperationResult> AddWishlist(string productId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("UserWishlist");
                var user = await _userService.GetUserByEmail();

                WishlistDto wishlist = new()
                {
                    Id = Guid.Empty,
                    UserId = user.Id,
                    ProductId = productId,
                    CreatedDate = DateTime.Now,
                };

                var json = JsonConvert.SerializeObject(wishlist);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("Create", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.data.ToObject<WishlistDto>();
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


        public async Task<OperationResult> GetWishlistByUserId(int? pageNumber)
        {
            try
            {
                var user = await _userService.GetUserByEmail();

                if (user != null)
                {
                    var httpClient = _httpClientFactory.CreateClient("UserWishlist");
                    int pageNumberParam = (int)(pageNumber != null ? pageNumber : 1);
                    HttpResponseMessage response = await httpClient.GetAsync("GetData?userId=" + user.Id +
                            "&PageNumber=" + pageNumberParam + "&PageSize=" + 10);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                        var result = jsonObject.result.ToObject<List<WishlistDto>>();

                        WishlistPagingMV wishlistPagingMV = new()
                        {
                            Wishlists = _mapper.Map<List<WishlistVM>>(result),
                            Pagination = _mapper.Map<PaginationVM>(jsonObject.pagination)
                        };

                        return new OperationResult { Success = true, Data = wishlistPagingMV };
                    }
                }

                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetWishlistAllByUser()
        {
            try
            {
                var user = await _userService.GetUserByEmail();

                if (user != null)
                {
                    var httpClient = _httpClientFactory.CreateClient("UserWishlist");
                    HttpResponseMessage response = await httpClient.GetAsync("GetAll?userId=" + user.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                        var result = jsonObject.ToObject<List<WishlistDto>>();
                        var listWishlish = _mapper.Map<List<WishlistVM>>(result);

                        return new OperationResult { Success = true, Data = listWishlish };
                    }
                }

                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> DeleteWishlist(string idWishlist)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("UserWishlist");

                HttpResponseMessage response = await httpClient.DeleteAsync("Delete?id=" + idWishlist);
                if (response.IsSuccessStatusCode)
                {
                    return new OperationResult { Success = true };
                }

                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetCountWishlistUser()
        {
            try
            {
                var user = await _userService.GetUserByEmail();

                if (user != null)
                {
                    var httpClient = _httpClientFactory.CreateClient("UserWishlist");
                    HttpResponseMessage response = await httpClient.GetAsync("GetAll?userId=" + user.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                        var result = jsonObject.ToObject<List<WishlistDto>>();

                        return new OperationResult { Success = true, Data = result.Count };
                    }
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
