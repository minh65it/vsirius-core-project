using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_UpdateTrackingOrder : IJob
    {
        private readonly TriggerController _triggerController;
        public Ebay_UpdateTrackingOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }
        public void Execute()
        {
            try
            {
                _triggerController.UpdateTrackingOrder().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UpdateTrackingOrder" + ex.Message);
            }
            return;
        }
    }
}