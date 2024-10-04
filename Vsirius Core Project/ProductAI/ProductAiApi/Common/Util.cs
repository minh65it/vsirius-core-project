namespace ProductAiApi.Common
{
    public static class Util
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
    }
}
