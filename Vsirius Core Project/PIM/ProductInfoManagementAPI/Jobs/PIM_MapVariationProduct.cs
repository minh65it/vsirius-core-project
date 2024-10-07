using PimApi.Controllers;
using Quartz;

namespace PimApi.Jobs
{
    public class PIM_MapVariationProduct : IJob
    {
        private readonly VariationController _variationController;
        public PIM_MapVariationProduct(VariationController variationController)
        {
            _variationController = variationController;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _variationController.TriggerMapVariation();
            return;
        }
    }
}
