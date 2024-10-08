using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_GetOrderFromEbay : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_GetOrderFromEbay(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public void Execute()
        {
            try
            {
                _triggerController.GetOrderEbay().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_GetOrderEbay" + ex.Message);
            }
            return;
        }
    }
}