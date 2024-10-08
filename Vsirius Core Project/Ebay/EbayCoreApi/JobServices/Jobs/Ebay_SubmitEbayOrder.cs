using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_SubmitEbayOrder : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_SubmitEbayOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.SubmitOrderEbay();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_SubmitEbayOrder" + ex.Message);
            }
            return;
        }
    }
}