using AutoMapper;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Security.Claims;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;

        public CartService(IHttpClientFactory httpClientFactory, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<bool> AddToCart(string sku, int quantity)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AddToCart");
                var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
                var userIdCookie = _httpContextAccessor.HttpContext?.Request.Cookies["CartUserVsirius"];

                var content =
                    new StringContent($"userId={userIdCookie}&email={email}&Quantity={quantity}&ProductId={sku}"
                    , Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage response = await httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);

                    return result.success;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }

        public async Task<CartDto?> GetCart(string? email, string userId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CartDetails");
                HttpResponseMessage response = await httpClient.GetAsync($"?userId={userId}&email={email}");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var cart = JsonConvert.DeserializeObject<CartDto>(responseBody);
                    return cart;
                }
                return null;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
        }

        public async Task<CartDto?> ListCartDetail()
        {
            CartDto? cart = null;
            try
            {
                string? userIdCookie = _httpContextAccessor?.HttpContext?.Request?.Cookies["CartUserVsirius"]?.ToString();
                string? email = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
                var httpClient = _httpClientFactory.CreateClient("CartDetails");
                HttpResponseMessage response = await httpClient.GetAsync($"?userId={userIdCookie}&email={email}");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<CartDto>(responseBody);
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return cart;
        }

        public async Task<bool> UpdateCart(List<UpdatedCartsVM> data)
        {
            try
            {
                var oldCart = await ListCartDetail();
                if (oldCart != null)
                {
                    foreach (var oldItem in oldCart.ListCartDetailsDtos)
                    {
                        var input = data.FirstOrDefault(x => x.ProductId.Equals(oldItem.ProductId));
                        if (input == null)
                        {
                            await AddToCart(oldItem.ProductId, -(oldItem.Quantity ?? 0));
                        }
                        else
                        {
                            await AddToCart(oldItem.ProductId, input.Quantity - (oldItem.Quantity ?? 0));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }
    }
}
