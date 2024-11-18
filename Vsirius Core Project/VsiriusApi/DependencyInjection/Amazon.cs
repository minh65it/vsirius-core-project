using AmazonIntegrationDataApi._Repositories;
using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi._Repositories.Repositories;
using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi._Services.Services;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetNewOrder;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetOrderShipped;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Update;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon;
using DBDataContext._Repositories;
using FluentValidation;
using QuartzInfrastructure;
using OfficeOpenXml;
using AmazonIntegrationDataApi.Jobs;
using AmazonIntegrationDataApi.Controllers;

namespace VsiriusApi.DependencyInjection
{
    public static class Amazon
    {
        public static void AddAmazonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAmazonJewelryDataFeedItemRepository, AmazonJewelryDataFeedItemRepository>();
            services.AddTransient<IAmazonMongoRepository, AmazonMongoRepository>();

            services.AddScoped<IReturnOrderAmazon, ReturnOrderAmazon>();
            services.AddScoped<IUploadProductToAmazonSeller, UploadProductToAmazonSeller>();
            services.AddScoped<ValidationAmazon>();
            services.AddScoped<IValidator<AmazonJewelryDataFeedItem_Dto>, AmazonJewelryDataFeedValidator>();
            services.AddTransient<IRepositoryAccessor, RepositoryAccessor>();
            services.AddScoped<IAmazonJewelryDataFeedItemService, AmazonJewelryDataFeedItemService>();
            services.AddTransient<IGetDataAmazonLocal, GetDataAmazonLocal>();
            services.AddTransient<IGetDataAmazonLocalForUpdate, GetDataAmazonLocalForUpdate>();

            services.AddTransient<ITriggerOrderAmazonProcessor, TriggerOrderAmazonProcessor>();
            services.AddTransient<IOrderAmazonProcessor, OrderAmazonProcessor>();

            services.AddTransient<IUpdateFullFields, UpdateFullFields>();
            services.AddTransient<IOrderDB, OrderDB>();
            services.AddTransient<IMappingOrder, MappingOrder>();
            services.AddTransient<IQgoldApiClient, QgoldApiClient>();
            services.AddTransient<IStullerClientApi, StullerClientApi>();
            services.AddTransient<IOrderTracking, OrderTracking>();
            services.AddTransient<IGetOrderDB, GetOrderDB>();
            services.AddTransient<IOrderFulfillment, OrderFulfillment>();
            services.AddTransient<IUpdateFullFieldsShipped, UpdateFullFieldsShipped>();
            services.AddTransient<IMapVSiriusDBToAmazonDB, MapVSiriusDBToAmazonDB>();
            services.AddTransient<IVSiriusProductApiClient, VSiriusProductApiClient>();
            services.AddTransient<IInsertProduct, InsertProduct>();
            services.AddTransient<IExcludedProduct, ExcludedProduct>();
            services.AddTransient<IQgoldClientApi, QgoldClientApi>();

            services.AddTransient<IMappingOrderStuller, MappingOrderStuller>();
            services.AddTransient<IOrderDBForStuller, OrderDBForStuller>();
            services.AddTransient<IStullerApiClient, AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller.StullerApiClient>();
            services.AddLogging(builder => builder
                .AddConsole()
                .AddDebug()
                .AddEventSourceLogger());

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //services.AddUpdateJobs();
            //services.AddTransient<UploadProductToAmazonSellerController>();
            //services.AddJob<Amazon_UploadProductToAmazon>();
            //services.AddJob<Amazon_UpdateInventoryToAmazon>();
            //services.AddJob<Amazon_UpdatePriceToAmazon>();

            //services.AddTransient<TriggerOrderAmazonProcessorController>();
            //services.AddJob<Amazon_GetNewAmazonOrder>();
            //services.AddJob<Amazon_SubmitOrderAmazonToQgold>();
            //services.AddJob<Amazon_UpdateTrackingOrderAmazon>();
            //services.AddTransient<ReturnOrderController>();
            //services.AddJob<Amazon_ReturnAmazonOrder>();
            //services.AddTransient<MapVSiriusDBToAmazonDBController>();
            //services.AddJob<Amazon_MapPIMToAmazonDB>();

        }
    }
}
