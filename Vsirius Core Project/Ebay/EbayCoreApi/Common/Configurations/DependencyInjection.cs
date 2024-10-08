using EbayApi.JobInfratructure.Jobs;
using EbayCoreApi._Services.Interfaces;
using EbayCoreApi._Services.Services;
using EbayCoreApi.Common.Middlewares;
using EbayCoreApi.Controllers;
using EbayCoreApi.JobServices.Interfaces;
using EbayCoreApi.JobServices.Jobs;
using EbayCoreApi.JobServices.Services;
using QuartzInfrastructure;
using VSiriusSystemLog.WriteLog;

namespace EbayCoreApi.Common.Configurations
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<IMapper>(sp =>
            //{
            //    return new Mapper(AutoMapperConfig.RegisterMappings());
            //});
            //services.AddSingleton(AutoMapperConfig.RegisterMappings());
            //services.AddTransient<ExceptionHandlingMiddleware>();
            
            //Middleware
            services.AddScoped<ILoggerCustom, LoggerCustom>();
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Service
            services.AddScoped<IEbayProductService, EbayProductService>();
            services.AddScoped<IEbayOrderService, EbayOrderService>();

            //Job Service
            services.AddScoped<IMapPimToEbayProductService, MapPimToEbayProductService>();
            services.AddScoped<IUploadItemToEbayMarketplaceService, UploadItemToEbayMarketplaceService>();
            services.AddScoped<IUpdatePriceAndQuantiyToMarketplaceService, UpdatePriceAndQuantiyToMarketplaceService>();
            services.AddScoped<IGetOrderEbayService, GetOrderEbayService>();
            services.AddScoped<ISubmitOrderService, SubmitOrderService>();
            services.AddScoped<IUpdateTrackingOrderService, UpdateTrackingOrderService>();

            services.AddTransient<TriggerController>();

            //Job Scheduler
            services.AddJob<Ebay_GetOrderFromEbay>();
            services.AddJob<Ebay_MapperPimProductToEbayProduct>();
            services.AddJob<Ebay_SubmitEbayOrder>();
            services.AddJob<Ebay_UpdatePriceQuantityMarketplace>();
            services.AddJob<Ebay_UpdateTrackingOrder>();
            services.AddJob<Ebay_UploadProductToSeller>();
        }
    }
}
