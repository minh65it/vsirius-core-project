using System.Net.Http.Headers;
using System.Security.Claims;

namespace PhillyLuxEcommerce.Configure
{
    public static class ConfigAPI
    {
        static readonly HttpContextAccessor httpContext;
        private readonly static IConfiguration _configuration;
        static ConfigAPI()
        {
            httpContext = new HttpContextAccessor();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Routers"))
            .AddJsonFile("routers.json")
            .Build();
        }

        public static void AddAPIProduct(this IServiceCollection services, IConfiguration configuration)
        {
            //if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddHttpClient("ProductDetailData", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["ProductDetailService:URL_Detail"]!);
            });
            services.AddHttpClient("RelatedProductData", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["ProductDetailService:URL_Related"]!);
            });
        }
        public static void AddAPISearch(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("FilterData", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["SearchService:URL_Filter"]!);
            });
            services.AddHttpClient("Search", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["SearchService:URL_Search"]!);
            });
            services.AddHttpClient("SearchTopProduct", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["SearchService:URL_TopProduct"]!);
            });
        }
        public static void AddAPIHome(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("HomeFilterData", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["HomeService:APIURlVsiriusFilter"]!);
            });
            services.AddHttpClient("HomeProductData", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["HomeService:APIURlVsiriusHomeProduct"]!);
            });
        }
        public static void AddAPISystem(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Logging", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["VsiriusSystemApi"]!);
            });
        }
        public static void AddAPICart(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("CartDetails", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["CartService:URL_CartDetails"]!);
                httpClient.DefaultRequestHeaders.Add("clientId", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientIdOrder").Value}");
                httpClient.DefaultRequestHeaders.Add("clientSecret", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientSecretOrder").Value}");
            });
            services.AddHttpClient("AddToCart", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["CartService:URL_AddToCart"]!);
                httpClient.DefaultRequestHeaders.Add("User-Agent", $"{httpContext.HttpContext?.Request.Headers["User-Agent"].ToString()}");
                httpClient.DefaultRequestHeaders.Add("clientId", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientIdOrder").Value}");
                httpClient.DefaultRequestHeaders.Add("clientSecret", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientSecretOrder").Value}");
            });
        }
        public static void AddAPINotification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("MarkNotification", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["NotificationService:URL_MarkReadNotifications"]!);
            });
            services.AddHttpClient("AllNotification", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["NotificationService:URL_AllNotifications"]!);
            });
            services.AddHttpClient("TopNotificaion", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["NotificationService:URL_TopNotification"]!);
            });
            services.AddHttpClient("NotReadNotification", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["NotificationService:URL_NotReadNotification"]!);
            });
        }

        public static void AddAPIPayment(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Order", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["OrderService:URL_Order"]!);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
            services.AddHttpClient("OrderCart", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["OrderService:URL_Cart"]!);
                httpClient.DefaultRequestHeaders.Add("clientId", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientIdOrder").Value}");
                httpClient.DefaultRequestHeaders.Add("clientSecret", $"{_configuration.GetSection("ApiVsiriusCredentials:ClientSecretOrder").Value}");
            });
            services.AddHttpClient("OrderUser", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["OrderService:URL_User"]!);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");

            });
            services.AddHttpClient("OrderStatus", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["OrderService:URL_OrderStatus"]!);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
        }
        public static void AddAPISinglePage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Contact", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["PageService:URL_Contact"]!);
            });
            services.AddHttpClient("GetPage", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["PageService:URL_DetailPage"]!);
            });
            services.AddHttpClient("GetAllPages", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["PageService:URL_ListPage"]!);
            });
        }
        public static void AddAPIUser(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("CheckLogin", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["HomeService:URL_CheckLogin"]!);
            });
            services.AddHttpClient("RegisterAccount", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["HomeService:URL_RegisterAccount"]!);
            });
            services.AddHttpClient("UserInfo", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_UserInfo"]!);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value);
            });
            services.AddHttpClient("ConfirmEmail", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_ConfirmEmail"]!);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value);
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
            services.AddHttpClient("SendOTP", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_SendOTP"]!);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value);
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
            services.AddHttpClient("ConfirmPhone", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_ConfirmPhone"]!);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value);
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
            services.AddHttpClient("UpdateUser", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_UpdateUser"]!);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value);
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {httpContext.HttpContext?.User.FindFirst(ClaimTypes.Authentication)?.Value}");
            });
            services.AddHttpClient("UserWishlist", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["UserService:URL_UserWishlist"]!);
            });
        }

    }
}
