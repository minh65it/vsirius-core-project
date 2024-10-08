using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_SubmitEbayOrder : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_SubmitEbayOrder(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public void Execute()
        {
            try
            {
                _triggerController.SubmitOrderEbay().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_SubmitEbayOrder" + ex.Message);
            }
            return;
        }
    }
}