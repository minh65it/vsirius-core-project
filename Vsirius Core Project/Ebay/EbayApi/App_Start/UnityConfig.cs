using EbayApi.Controllers;
using EbayApi.JobServices;
using EbayApi.JobServices.Interfaces;
using EbayApi.Services;
using EbayApi.Services.Interfaces;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace EbayApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            //Service
            container.RegisterType<IEbayProductService, EbayProductService>();
            container.RegisterType<IEbayOrderService, EbayOrderService>();

            //Job Service
            container.RegisterType<IMapPimToEbayProductService, MapPimToEbayProductService>();
            container.RegisterType<IUploadItemToEbayMarketplaceService, UploadItemToEbayMarketplaceService>();
            container.RegisterType<IUpdatePriceAndQuantiyToMarketplaceService, UpdatePriceAndQuantiyToMarketplaceService>();
            container.RegisterType<IGetOrderEbayService, GetOrderEbayService>();
            container.RegisterType<ISubmitOrderService, SubmitOrderService>();
            container.RegisterType<IUpdateTrackingOrderService, UpdateTrackingOrderService>();

            container.RegisterType<TriggerController>(new TransientLifetimeManager());

            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}