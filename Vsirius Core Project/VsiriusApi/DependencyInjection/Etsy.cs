using EtsyIntegrationDataAPI._Etsy.Etsy;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Repositories.Repositories;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI._Services.Services;
using EtsyIntegrationDataAPI.Controllers;
using EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Helpers.OrderEtsy;
using EtsyIntegrationDataAPI.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.DependencyInjection
{
    public static class Etsy
    {
        public static void AddEtsyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenEtsyRepository, AuthenEtsyRepository>();
            services.AddTransient<IProductEtsyRepository, ProductEtsyRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IPropertiesByTaxonomyRepository, PropertiesByTaxonomyRepository>();
            services.AddTransient<IPropertiesProductRepository, PropertiesProductRepository>();
            services.AddScoped<IAuthenticationEtsyService, AuthenticationEtsyService>();
            services.AddScoped<IListingImageService, ListingImageService>();
            services.AddScoped<IListingService, ListingService>();
            services.AddScoped<IProductEtsyService, ProductEtsyService>();
            services.AddScoped<IMapPIMToEtsyDBService, MapPIMToEtsyDBService>();
            services.AddScoped<IPIMApiClient, PIMApiClient>();
            services.AddScoped<IExcludedProduct, ExcludedProduct>();
            services.AddScoped<IInsertProduct, InsertProduct>();
            services.AddScoped<IListingInventoryService, ListingInventoryService>();
            services.AddScoped<IInsertOrder, InsertOrder>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISubmitOrder, SubmitOrder>();
            services.AddScoped<IQgoldApiClient, QgoldApiClient>();
            services.AddScoped<IStullerApiClient, EtsyIntegrationDataAPI.Helpers.OrderEtsy.StullerApiClient>();
            services.AddScoped<ITopOrderService, TopOrderService>();
            services.AddScoped<IPropertiesProductService, PropertiesProductService>();

            services.AddUpdateJobs();
            services.AddTransient<MapPIMToEtsyDBController>();
            services.AddJob<Etsy_MapPIMToEtsyDB>();
            services.AddTransient<ListingController>();
            services.AddJob<Etsy_CreateListDraftAndPublishListing>();
            //services.AddTransient<ListingInventoryController>();
            //services.AddJob<Etsy_UpdateAllListingInventory>();
            //// update order
            //services.AddTransient<OrderController>();
            //services.AddJob<Etsy_GetNewOrderEtsy>();
            //services.AddJob<Etsy_SubmitOrderQgold>();
            //services.AddJob<Etsy_UpdateTrackingOrder>();
            //// update Properties
            //services.AddTransient<PropertiesProductController>();
            //services.AddJob<Etsy_UpdateProperties>();

        }
    }
}
