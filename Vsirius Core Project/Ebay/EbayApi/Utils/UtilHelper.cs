using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EbayApi.Utils
{
    public static class UtilHelper
    {
        public static string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Logs");

        public static void WriteLog(string Message)
        {
            CreateFolder(_logPath);
            string filePath = Path.Combine(_logPath, $"Log_{DateTime.Now:yyyyMMdd}.txt");

            try
            {
                StreamWriter writer = File.AppendText(filePath);
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    writer.WriteLine($"[{DateTime.Now}] {Message}");
                    Console.WriteLine($"[{DateTime.Now}] {Message}");
                }
                writer.Flush();
                writer.Close();
            }
            catch (IOException)
            {
                Task.Delay(10).Wait();
                WriteLog(Message);
            }
        }

        public static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static List<List<T>> SplitListIntoBatches<T>(List<T> list, int batchSize)
        {
            List<List<T>> batches = new List<List<T>>();
            for (int i = 0; i < list.Count; i += batchSize)
            {
                List<T> batch = list.GetRange(i, Math.Min(batchSize, list.Count - i));
                batches.Add(batch);
            }
            return batches;
        }

        public static string GetOriginSKU(string inputString)
        {
            int indexOfDash = inputString.IndexOf('-');

            if (indexOfDash != -1 && indexOfDash < inputString.Length - 1)
            {
                return inputString.Substring(indexOfDash + 1);
            }
            return inputString;
        }
    }
}