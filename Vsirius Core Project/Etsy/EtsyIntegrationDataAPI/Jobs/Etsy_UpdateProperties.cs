using EtsyIntegrationDataAPI.Controllers;
using Quartz;

namespace EtsyIntegrationDataAPI.Jobs
{
    public class Etsy_UpdateProperties : IJob
    {
        private readonly PropertiesProductController _propertiesProductController;

        public Etsy_UpdateProperties(PropertiesProductController propertiesProductController)
        {
            _propertiesProductController = propertiesProductController;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _propertiesProductController.UpdateListingPropertiesEtsy().Wait();
            return Task.CompletedTask;
        }
    }
}
