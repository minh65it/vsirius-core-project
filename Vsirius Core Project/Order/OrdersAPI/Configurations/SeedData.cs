using OrdersAPI._Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataModel;

namespace OrdersAPI.Configurations
{
    public static class SeedData
    {
        public static async Task Run(this WebApplication app, ConfigurationManager configuration)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var configJsonRepository = services.GetRequiredService<IConfigJsonRepository>();
                    var existData = await configJsonRepository.FindAll(x => x.ServiceName.Equals("Order")).ToListAsync();
                    configJsonRepository.RemoveMultiple(existData);

                    var lstData = new List<ConfigJson>()
                    {
                        new ConfigJson()
                        {
                            Key = "AuthorizeNetEnviroment",
                            Value = configuration["AuthorizeNet"],    
                            ServiceName = "Order",
                        },
                        new ConfigJson()
                        {
                            Key = "ApiLoginID",
                            Value = configuration["ApiLoginID"],
                            ServiceName = "Order",
                        },
                        new ConfigJson()
                        {
                            Key = "ApiTransactionKey",
                            Value = configuration["ApiTransactionKey"],
                            ServiceName = "Order",
                        },
                    };
                    configJsonRepository.AddMultiple(lstData);

                    await configJsonRepository.SaveChangesAsync();
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
