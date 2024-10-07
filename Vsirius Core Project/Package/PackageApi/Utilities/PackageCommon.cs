using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace PackageApi.Utilities
{
    public class PackageCommon
    {
        public static string _logPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
        public static string _fileLogName = string.Empty;
        public static async Task WriteLogAsync(string logContent, string title = "")
        {
            if (string.IsNullOrEmpty(_fileLogName))
            {
                _fileLogName = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }

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
        public static string RenameFile(IFormFile dto, string id)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm" };
            string[] imgExtensions = { ".jpeg", ".jpg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".svg" };



            string fileExtension = Path.GetExtension(dto.FileName).ToLower();

            if (videoExtensions.Contains(fileExtension)) fileExtension = ".mp4";
            if (imgExtensions.Contains(fileExtension)) fileExtension = ".jpg";

            var fileNameNew = id + fileExtension;

            return fileNameNew;
        }

        public static string? GetMarketplaceOrderId(string textImg)
        {
            if (textImg.Contains("Walmart"))
            {
                return Regex.Match(textImg, @"PO:\s*(\d+)")?.Groups[1]?.Value;
            }
            if (textImg.Contains("Amazon"))
            {
                return Regex.Match(textImg, @"Order ID:\s*(\d{3}-\d{7}-\d{7})")?.Groups[1]?.Value;
            }
            return null;
        }

        public static string? GetRMA(string textImg)
        {
            if (textImg.Contains("Walmart"))
            {
                return Regex.Match(textImg, @"RMA:\s*(\d+)")?.Groups[1]?.Value;
            }
            if (textImg.Contains("Amazon"))
            {
                return Regex.Match(textImg, @"Amazon Return ID:\s*(\w+)")?.Groups[1]?.Value;
            }
            return null;
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
