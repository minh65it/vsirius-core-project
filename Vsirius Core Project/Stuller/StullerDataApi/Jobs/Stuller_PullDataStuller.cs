using Quartz;
using StullerDataApi.Controllers;

namespace StullerDataApi.Jobs
{
    public class Stuller_PullDataStuller : IJob
    {
        private readonly PullStullerController _controllerice;

        public Stuller_PullDataStuller(PullStullerController controllerice)
        {
            _controllerice = controllerice;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _controllerice.PullNewProduct().Wait();
            return Task.CompletedTask;
        }
    }
}
