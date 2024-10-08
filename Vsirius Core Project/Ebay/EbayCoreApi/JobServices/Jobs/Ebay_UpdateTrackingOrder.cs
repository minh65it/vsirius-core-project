using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_UpdateTrackingOrder : IJob
    {
        private readonly TriggerController _triggerController;
        public Ebay_UpdateTrackingOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.UpdateTrackingOrder();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UpdateTrackingOrder" + ex.Message);
            }
            return;
        }
    }
}