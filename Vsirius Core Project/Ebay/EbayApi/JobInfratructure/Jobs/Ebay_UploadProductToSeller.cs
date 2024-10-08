using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_UploadProductToSeller : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_UploadProductToSeller(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public void Execute()
        {
            try
            {
                _triggerController.UploadProductToEbayMarketplace().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_UploadProductToEbayMarketplace" + ex.Message);
            }
            return;
        }

    }
}