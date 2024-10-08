using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayCoreApi.JobServices.Jobs
{
    public class Ebay_GetOrderFromEbay : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_GetOrderFromEbay(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.GetOrderEbay();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_GetOrderEbay" + ex.Message);
            }
            return;
        }
    }
}