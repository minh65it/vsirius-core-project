using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;
namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_UpdatePriceQuantityMarketplace : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_UpdatePriceQuantityMarketplace(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public void Execute()
        {
            try
            {
                _triggerController.UpdatePriceQuantiyEbayProduct().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UpdatePriceQuantiyEbayProduct" + ex.Message);
            }
            return;
        }
    }
}