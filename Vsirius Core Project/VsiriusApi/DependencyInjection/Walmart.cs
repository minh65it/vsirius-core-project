using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi._Repositories;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi._Services;
using WalmartIntegrationDataApi.Controllers;
using QuartzInfrastructure;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._JobServices.JobService;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Job;

namespace VsiriusApi.DependencyInjection
{
    public static class Walmart
    {
        public static void AddWalmartServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductWalmartService, ProductWalmartService>();
            services.AddScoped<IOrderWalmartService, OrderWalmartService>();

            services.AddTransient<IOrderWalmartRepository, OrderWalmartRepository>();
            services.AddTransient<IOrderQgoldRepository, OrderQgoldRepository>();
            services.AddTransient<OperationResult>();

            services.AddSingleton<IValidationWithMasterExcel, ValidationWithMasterExcel>();
            
            //Update Jobs
            //services.AddUpdateJobs();
            //Add Job
            services.AddScoped<IGetOrderWalmartService, GetOrderWalmartService>();
            services.AddScoped<IGetReturnOrderService, GetReturnOrderService>();
            services.AddScoped<IInteractWalmartSellerCommonService, InteractWalmartSellerCommonService>();
            services.AddScoped<ITrackingWalmartOrderService, TrackingWalmartOrderService>();
            services.AddScoped<ISubmitOrderToQgoldService, SubmitOrderToQgoldService>();
            services.AddScoped<IApplyShippingTemplateService, ApplyShippingTemplateService>();
            services.AddScoped<IUploadProductToSellerService, UploadProductToSellerService>();
            services.AddScoped<IUpdateInventoryService, UpdateInventoryService>();
            services.AddScoped<IUpdatePriceService, UpdatePriceService>();
            services.AddScoped<IMapVsiriusToWalmartService, MapVsiriusToWalmartService>();
            /*
            services.AddTransient<TriggerController>();
            
            services.AddJob<Walmart_UpdateTrackingOrder>();
            services.AddJob<Walmart_SubmitOrderToQgold>();
            services.AddJob<Walmart_GetReturnOrder>();
            services.AddJob<Walmart_GetOrder>();

            services.AddJob<Walmart_ApplyShippingTemplate>();
            services.AddJob<Walmart_UpdateInventory>();
            services.AddJob<Walmart_UpdatePrice>();
            services.AddJob<Walmart_UploadProductToSeller>();
            services.AddJob<Walmart_MapVsiriusToWalmart>();
            */
        }
    }
}
