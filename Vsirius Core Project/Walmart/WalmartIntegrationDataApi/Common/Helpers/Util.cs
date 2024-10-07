using Aspose.Cells;
using MongoDB.Driver;
using OfficeOpenXml;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WalmartIntegrationDataApi.Common.Helpers
{
    public static class Util
    {
        public static string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Logs");
        public static void ActiveAsposeCell()
        {
            try
            {
                //License cellLicense = new License();
                //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //string filePath = baseDirectory + @"\Resources\Aspose.Total.lic";
                //cellLicense.SetLicense(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void ActiveEPPLUS()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Dictionary<string, object[]>? dataValidValueInExcel = GetDataValidExcel();

        #region use aspose cell
        private static Dictionary<string, object[]>? GetDataValidExcel()
        {
            if (dataValidValueInExcel == null)
            {
                dataValidValueInExcel = new Dictionary<string, object[]>();
                //string pathToTemplate = _env.ContentRootPath + "Files/WalmartProductTemplate.xlsx";
                string pathToTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Files\\WalmartProductTemplate.xlsx");
                Workbook workbook = new Workbook(pathToTemplate);
                Worksheet worksheet = workbook.Worksheets["Jewelry"];

                int finalCol = worksheet.Cells.MaxDataColumn;
                //3: colStart in excel
                for (int col = 3; col <= finalCol; ++col)
                {
                    //4: row attribute domain = key name directory
                    Cell cellKey = worksheet.Cells[4, col];
                    if (cellKey.Value == null)
                    {
                        break;
                    }
                    else
                    {
                        var key = cellKey.Value == null ? "" : cellKey.Value.ToString();

                        //Check duplicate key or special key and create new key base on model
                        if (dataValidValueInExcel.ContainsKey(key!) || key == "unit")
                        {

                            //Case: header exist => key + header                            
                            Cell cellHeader = worksheet.Cells[2, col];
                            if (cellHeader.IsMerged)
                            {
                                Aspose.Cells.Range mergedRange = cellHeader.GetMergedRange();

                                key += worksheet.Cells[mergedRange.FirstRow, mergedRange.FirstColumn].Value.ToString();
                                key = key.Split(' ')[0];
                            }


                        }

                        //6: row data domain = value key directory
                        Cell cellData = worksheet.Cells[6, col];
                        if (cellData.GetValidation() != null && cellData.GetValidation().Type == ValidationType.List)
                        {
                            dataValidValueInExcel.Add(key!, (object[])cellData.GetValidation().Value1);
                        }
                    }
                }
            }
            return dataValidValueInExcel;
        }
        #endregion
        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string rs = $"{input[0].ToString().ToLower()}{input[1..]}";

            return rs;
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

        public static string SKUQgold(string sku)
        {
            return Regex.Match(sku, @"Q.*?-(.*)").Groups[1].Value;
        }
        public static string SKUStuller(string sku)
        {
            string pattern = @"S-(\d+:)+[^-\s]+";

            Match match = Regex.Match(sku, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static long ConvertDateTimeToEpoch(DateTime date)
        {
            var dateWithOffset = new DateTimeOffset(date).ToUniversalTime();
            return dateWithOffset.ToUnixTimeMilliseconds();
        }

        public static long ConvertDateTimeToEpoch(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                date = DateTime.UtcNow.ToString();
            }
            var dateParse = DateTime.Parse(date);
            var dateWithOffset = new DateTimeOffset(dateParse).ToUniversalTime();
            return dateWithOffset.ToUnixTimeMilliseconds();
        }

        public static DateTime? ConvertEpochUTCToDateTime(long estimatedDeliveryDate)
        {
            if (estimatedDeliveryDate <= 0)
            {
                return null;
            }

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(estimatedDeliveryDate);
            DateTime dateTimeUTC = dateTimeOffset.UtcDateTime;

            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUTC, TimeZoneInfo.Local);
        }

        public static long ConvertDateTimeToEpochUTC(DateTime dateTime)
        {
            DateTime currentDateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            return ConvertDateTimeToEpoch(currentDateTimeUTC);
        }

        public static long ConvertDateTimeToEpochUTC(string date)
        {
            var dateTime = DateTime.Parse(date);
            DateTime currentDateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            return ConvertDateTimeToEpoch(currentDateTimeUTC);
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

        public static bool IsSimilar(string? s1, string? s2)
        {
            if (s1.Equals(s2, StringComparison.OrdinalIgnoreCase)) return true;

            var wordArr1 = s1.Split(' ').ToList();
            var wordArr2 = s2.Split(' ').ToList();

            decimal pointSimilar = 0;
            decimal percentSimilar1, percentSimilar2, percentSimilar;

            foreach (var word1 in wordArr1)
            {
                if (s2.Contains(word1, StringComparison.OrdinalIgnoreCase))
                {
                    pointSimilar++;
                }
            }
            percentSimilar1 = pointSimilar / wordArr1.Count;

            pointSimilar = 0;
            foreach (var word2 in wordArr2)
            {
                if (s1.Contains(word2, StringComparison.OrdinalIgnoreCase))
                {
                    pointSimilar++;
                }
            }
            percentSimilar2 = pointSimilar / wordArr2.Count;

            percentSimilar = (percentSimilar1 + percentSimilar2) / 2;

            if (percentSimilar > 0.8m)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetOriginSKU(string inputString)
        {
            int indexOfDash = inputString.IndexOf('-');

            // Kiểm tra xem có dấu '-' không và có ký tự đằng sau hay không
            if (indexOfDash != -1 && indexOfDash < inputString.Length - 1)
            {
                // Lấy chuỗi bắt đầu từ vị trí sau dấu '-' đầu tiên
                return inputString[(indexOfDash + 1)..];
            }
            return inputString;
        }

        public static T CloneObject<T>(T original) where T : class
        {
            var clone = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(clone, property.GetValue(original));
            }

            return clone;
        }

        public static string GetProviderName(string sku)
        {
            if (sku.StartsWith("S"))
            {
                return "Stuller";
            }
            if (sku.StartsWith("Q"))
            {
                return "Qgold";
            }
            return string.Empty;
        }

        public static string GetCarrier(string? input)
        {
            string result = string.Empty;
            int startIndex = input.IndexOf('_') + 1;

            // Tìm vị trí của dấu _ tiếp theo từ startIndex
            int endIndex = input.IndexOf('_', startIndex);

            // Lấy phần chuỗi từ startIndex đến endIndex
            result = input[startIndex..endIndex];

            return result;
        }
    }
}
