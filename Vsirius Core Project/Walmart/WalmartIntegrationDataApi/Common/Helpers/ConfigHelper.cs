namespace WalmartIntegrationDataApi.Common.Helpers
{
    //public class ConfigHelper
    //{

    //    private static ConfigHelper? _appSettings;

    //    public dynamic AppSettingValue { get; set; }

    //    public static string AppSetting(string Key)
    //    {
    //        _appSettings = GetCurrentSettings(Key);
    //        return _appSettings.AppSettingValue!;
    //    }

    //    public ConfigHelper(IConfiguration config, string Key)
    //    {
    //        AppSettingValue = config.GetValue<dynamic>(Key)!;
    //    }

    //    public static ConfigHelper GetCurrentSettings(string Key)
    //    {
    //        var builder = new ConfigurationBuilder()
    //                        .SetBasePath(Directory.GetCurrentDirectory())
    //                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //                        .AddEnvironmentVariables();

    //        IConfigurationRoot configuration = builder.Build();

    //        var settings = new ConfigHelper(configuration, Key);

    //        return settings;
    //    }
    //}
}
