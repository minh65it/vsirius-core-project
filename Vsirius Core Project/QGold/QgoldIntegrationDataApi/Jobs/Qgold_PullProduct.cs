using QgoldIntegrationDataApi.Controllers;
using Quartz;

namespace QgoldIntegrationDataApi.Jobs
{
    public class Qgold_PullProduct : IJob
    {
        private readonly QgoldProcessTriggerController _controller;

        public Qgold_PullProduct(QgoldProcessTriggerController controller)
        {
            _controller = controller;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _controller.PullQgold().Wait();
            return Task.CompletedTask;
        }
    }
}
