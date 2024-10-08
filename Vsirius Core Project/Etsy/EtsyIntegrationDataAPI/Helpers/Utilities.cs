namespace EtsyIntegrationDataAPI.Helpers
{
    public class Utilities
    {
        public static string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Logs");
        public static IConfiguration configJson = GetJsonFile();
        private static IConfiguration GetJsonFile()
        {
            if (configJson == null)
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),
                    $"appsettings.{(string.IsNullOrWhiteSpace(env) ? "Production" : env)}.json"));

                var root = builder.Build();
                return root;
            }
            return configJson;
        }
        public static async Task WriteLogAsync(string logContent, string title = "")
        {
            var _fileLogName = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            string file = Path.Combine(_logPath, _fileLogName);

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            try
            {
                using (StreamWriter writer = File.AppendText(file))
                {
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        await writer.WriteLineAsync($"\n{title}");
                        Console.WriteLine($"\n{title}");
                    }
                    if (!string.IsNullOrWhiteSpace(logContent))
                    {
                        await writer.WriteLineAsync($"[{DateTime.Now}] {logContent}");
                        Console.WriteLine($"[{DateTime.Now}] {logContent}");
                    }
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (IOException)
            {
                await Task.Delay(10);
                await WriteLogAsync(title, logContent); // Thử ghi log lại nếu gặp lỗi
            }
        }
    }
}
