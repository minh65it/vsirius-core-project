using QgoldIntegrationDataApi.Controllers;
using Quartz;

namespace QgoldIntegrationDataApi.Jobs
{
    public class Qgold_MappingQgoldToPIM : IJob
    {
        private readonly QgoldProcessTriggerController _controller;

        public Qgold_MappingQgoldToPIM(QgoldProcessTriggerController controller)
        {
            _controller = controller;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _controller.MappingQgoldToVsirius().Wait();
            return Task.CompletedTask;
        }
    }
}
