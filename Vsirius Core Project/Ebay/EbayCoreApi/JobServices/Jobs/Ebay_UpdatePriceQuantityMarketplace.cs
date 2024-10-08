using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_UpdatePriceQuantityMarketplace : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_UpdatePriceQuantityMarketplace(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.UpdatePriceQuantiyEbayProduct();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UpdatePriceQuantiyEbayProduct" + ex.Message);
            }
            return;
        }
    }
}