using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Models;
using MongoDB.Driver;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
{
    public static class Utilities2
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
        public static void WriteLog(string textContent, string title = "")
        {
            try
            {
                var _fileLogName = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                string file = Path.Combine(_logPath, _fileLogName);

                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        writer.WriteLine($"\n{title}");
                        Console.WriteLine($"\n{title}");
                    }
                    writer.WriteLine(textContent);
                    writer.Flush();
                }
            }
            catch (Exception)
            {
                Task.Delay(10).Wait();
                WriteLog(textContent, title); // Thử ghi log lại nếu gặp lỗi
            }

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
        public static void SaveFileTxt(string fileName, List<AmazonJewelryDataFeedItem> dataList)
        {
            //save file txt
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFilePath = baseDirectory + @"\Resources\DataAmazonMapping.txt";
            if (!Directory.Exists(baseDirectory + @"\Storage\Data"))
            {
                Directory.CreateDirectory(baseDirectory + @"\Storage\Data");
            }
            string filePath = baseDirectory + @$"\Storage\Data\\{fileName}";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Copy(sourceFilePath, filePath);
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("\n");
                foreach (var data in dataList)
                {
                    if (data != null)
                    {
                        string line = "";
                        foreach (var property in typeof(AmazonJewelryDataFeedItem).GetProperties())
                        {
                            var value = property.GetValue(data, null);
                            line += $"{value}\t";
                        }
                        writer.WriteLine($"{line}\n");
                    }
                }
                writer.Flush();
            }
        }
        public static PaginationResult<T> CreatePagination<T>(List<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
        {
            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? source.Skip(skip).Take(pageSize).ToList() : source.ToList();
            var pageCount = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResult<T>() { Data = items, TotalCount = count, PageCount = pageCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public static long ConvertDateTimeToEpoch(DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeMilliseconds();
        }

        public static DateTime ConvertEpochToDateTime(long epoch)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return dateTimeOffset.DateTime;
        }

        public static long ConvertDateTimeToEpoch(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                date = DateTime.UtcNow.ToString();
            }
            var dateParse = DateTime.Parse(date);
            return new DateTimeOffset(dateParse).ToUnixTimeMilliseconds();
        }

        public static void AddOrFilter<T>(
            ref FilterDefinition<T>? filter,
            FilterDefinition<T> filterDefinition)
        {
            if (filter == null)
            {
                filter = filterDefinition;
            }
            else
            {
                filter |= filterDefinition;
            }
        }

        public static void AddAndFilter<T>(
            ref FilterDefinition<T>? filter,
            FilterDefinition<T>? filterDefinition)
        {
            if (filter == null)
            {
                filter = filterDefinition;
            }
            else if (filterDefinition != null)
            {
                filter &= filterDefinition;
            }
        }
    }
}
