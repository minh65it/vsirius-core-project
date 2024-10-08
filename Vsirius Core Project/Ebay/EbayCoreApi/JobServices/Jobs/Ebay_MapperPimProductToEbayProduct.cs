using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Controllers;
using Quartz;

namespace EbayCoreApi.JobServices.Jobs
{
    public class Ebay_MapperPimProductToEbayProduct : IJob
    {
        private readonly TriggerController _triggerController;

        public Ebay_MapperPimProductToEbayProduct(TriggerController triggerController)
        {
            _triggerController = triggerController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _triggerController.MapPimToEbayProduct();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Ebay_MapPimToEbayProduct" + ex.Message);
            }
            return;
        }
    }
}