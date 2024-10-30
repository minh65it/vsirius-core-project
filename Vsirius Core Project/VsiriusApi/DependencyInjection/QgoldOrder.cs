using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Repositories.Repositories;
using OrderProcessApi._Services.IServices;
using OrderProcessApi._Services.IServices.Amazon;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Services.IServices.Stuller;
using OrderProcessApi._Services.IServices.Walmart;
using OrderProcessApi._Services.Services;
using OrderProcessApi._Services.Services.Amazon;
using OrderProcessApi._Services.Services.Qgold;
using OrderProcessApi._Services.Services.Stuller;
using OrderProcessApi._Services.Services.Walmart;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Amazon;

namespace VsiriusApi.DependencyInjection
{
    public static class QgoldOrder
    {
        public static void AddQgoldOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAmazonRepository, AmazonRepository>();
            services.AddTransient<IWalmartRepository, WalmartRepository>();
            services.AddTransient<IQgoldRepository, QgoldRepository>();
            services.AddTransient<IStullerRepository, StullerRepository>();
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IQgoldShippingInstruction, QgoldShippingInstruction>();
            services.AddScoped<IReOrderQgold, ReOrderQgold>();
            services.AddScoped<IUpdateFullFields, UpdateFullFields>();

            services.AddScoped<IKeyService, KeyService>();
            services.AddScoped<ISubmitOrderQgoldService, SubmitOrderQgoldService>();
            services.AddScoped<IQAService, QAService>();
            services.AddScoped<IPackageClaimService, PackageClaimService>();
            services.AddScoped<ISubmitOrderStullerService, SubmitOrderStullerService>();
            services.AddScoped<IQgoldProcessService, QgoldProcessService>();
            services.AddScoped<IStullerProcessService, StullerProcessService>();
            services.AddScoped<IAmazonProcessService, AmazonProcessService>();
            services.AddScoped<IWalmartProcessService, WalmartProcessService>();
            services.AddScoped<IQgoldOrderMarketplaceService, QgoldOrderMarketplaceService>();
            services.AddScoped<IReSubmitOrderService, ReSubmitOrderService>();
        }
    }
}
