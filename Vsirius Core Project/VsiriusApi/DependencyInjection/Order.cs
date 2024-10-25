using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Repositories.Repositories;
using OrdersAPI._Services.Interfaces;
using OrdersAPI._Services.Services;
using OrdersAPI.Helpers.Utilities;

namespace VsiriusApi.DependencyInjection
{
    public static class Order
    {
        public static void AddOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVsiriusOrderRepository, VsiriusOrderRepository>();
            services.AddTransient<IVsiriusOrderDetailRepository, VsiriusOrderDetailRepository>();
            services.AddTransient<IVsiriusCartRepository, VsiriusCartRepository>();
            services.AddTransient<IVsiriusCartDetailsRepository, VsiriusCartDetailsRepository>();
            services.AddTransient<IVsiriusProductRepository, VsiriusProductRepository>();
            services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IConfigJsonRepository, ConfigJsonRepository>();

            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IVsiriusOrderService, VsiriusOrderService>();
            services.AddScoped<IOderStatusService, OderStatusService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IVsiriusCartService, VsiriusCartService>();
            services.AddScoped<IHttpHeaderService, HttpHeaderService>();
            services.AddScoped<IKeyService, KeyService>();
            services.AddScoped<IFunctionUtilities, FunctionUtilities>();
        }
    }
}
