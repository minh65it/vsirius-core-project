using Microsoft.Owin;
using Owin;
using Unity;

[assembly: OwinStartup(typeof(EbayApi.App_Start.Startup))]
namespace EbayApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);
            BackgroundJobConfig.Start(app, container).Wait();
        }
    }
}