using EbayApi.Controllers;
using EbayApi.JobInfratructure.Dtos;
using EbayApi.Utils;

namespace EbayApi.JobInfratructure.Jobs
{
    public class Ebay_MapperPimProductToEbayProduct :IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_MapperPimProductToEbayProduct(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public void Execute()
        {
            try
            {
                _triggerController.MapPimToEbayProduct().Wait();
            }
            catch (System.Exception ex)
            {
                UtilHelper.WriteLog("Ebay_MapPimToEbayProduct" + ex.Message);
            }
            return;
        }
    }
}