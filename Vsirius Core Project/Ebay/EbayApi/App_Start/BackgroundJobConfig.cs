using System.Configuration;
using System.Threading.Tasks;
using EbayApi.JobInfratructure.Jobs;
using EbayApi.JobInfratructure.Setups;
using EbayApi.Utils;
using Hangfire;
using Owin;

namespace EbayApi.App_Start
{
    public static class BackgroundJobConfig 
    {
        public static async Task Start(IAppBuilder app, Unity.UnityContainer container)
        {
            try
            {
                GlobalConfiguration.Configuration
               .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["EbayContext"].ConnectionString)
               .UseUnityActivator(container);


                // Khởi động Hangfire server và bảng điều khiển
                app.UseHangfireServer();
                app.UseHangfireDashboard("/hangfire");

                await BackgroundJobSetup<Ebay_UploadProductToSeller>.Configure();
                await BackgroundJobSetup<Ebay_MapperPimProductToEbayProduct>.Configure();
                await BackgroundJobSetup<Ebay_UpdatePriceQuantityMarketplace>.Configure();
                await BackgroundJobSetup<Ebay_GetOrderFromEbay>.Configure();
                await BackgroundJobSetup<Ebay_SubmitEbayOrder>.Configure();
                await BackgroundJobSetup<Ebay_UpdateTrackingOrder>.Configure();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }            
        }
    }
}