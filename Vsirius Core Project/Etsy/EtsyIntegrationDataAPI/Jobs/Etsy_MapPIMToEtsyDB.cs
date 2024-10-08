using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_MapPIMToEtsyDB : IJob
    {
        private readonly MapPIMToEtsyDBController _mapPIMToEstyDBController;

        public Etsy_MapPIMToEtsyDB(MapPIMToEtsyDBController mapPIMToEstyDBController)
        {
            _mapPIMToEstyDBController = mapPIMToEstyDBController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _mapPIMToEstyDBController.RequestAnAuthorizationCode().Wait();
            return Task.CompletedTask;
        }
    }
}
