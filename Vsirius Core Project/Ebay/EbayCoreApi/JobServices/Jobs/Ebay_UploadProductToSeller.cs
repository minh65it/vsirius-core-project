using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayCoreApi.JobServices.Jobs
{
    public class Ebay_UploadProductToSeller : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_UploadProductToSeller(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.UploadProductToEbayMarketplace();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UploadProductToEbayMarketplace" + ex.Message);
            }
            return;
        }
    }
}