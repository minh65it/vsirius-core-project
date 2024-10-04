using ProductAiApi.Controllers;
using Quartz;

namespace ProductAiApi.Jobs
{
    public class ProductAI_CreateProductAiFromPim : IJob
    {
        private readonly TriggerController _controller;

        public ProductAI_CreateProductAiFromPim(TriggerController controller)
        {
            _controller = controller;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _controller.CreateProductAiFromPim();
            return;
        }
    }
}
