using Quartz;
using StullerDataApi.Controllers;

namespace StullerDataApi.Jobs
{
    public class Stuller_MapStullerToPIM : IJob
    {
        private readonly MapStullerDBToVsiriusDBController _controller;

        public Stuller_MapStullerToPIM(MapStullerDBToVsiriusDBController controller)
        {
            _controller = controller;
        }
        public Task Execute(IJobExecutionContext context)
        {
             _controller.MapToVsiriusDB().Wait();
            return Task.CompletedTask;
        }
    }
}
