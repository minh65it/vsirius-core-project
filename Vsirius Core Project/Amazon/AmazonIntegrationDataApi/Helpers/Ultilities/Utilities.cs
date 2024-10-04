using AmazonIntegrationDataApi.Models;
using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
{
    public static class Utilities
    {
        public static bool IsValidGCID(string gcid)
        {
            // GCID là chuỗi 16 ký tự
            return !string.IsNullOrEmpty(gcid) && gcid.Length == 16 && IsNumeric(gcid);
        }

        public static bool IsValidUPC(string upc)
        {
            // UPC là chuỗi 12 ký tự
            return !string.IsNullOrEmpty(upc) && upc.Length == 12 && IsNumeric(upc);
        }

        public static bool IsValidEAN(string ean)
        {
            // EAN là chuỗi 13 ký tự
            return !string.IsNullOrEmpty(ean) && ean.Length == 13 && IsNumeric(ean);
        }

        public static bool IsNumeric(string value)
        {
            // Kiểm tra chuỗi có chứa toàn ký tự số hay không
            return value.All(char.IsDigit);
        }

        public static bool IsValidInteger(string input)
        {
            int value;

            // Kiểm tra chuỗi có hợp lệ là số nguyên hay không
            return int.TryParse(input, out value);
        }
        public static bool isValidNotUseCurrency(string input)
        {
            string pattern = @"^(?:\d{1,18}|\d{1,18}\.\d{1,2})$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool isValidBulletPoint(string input)
        {
            if (input.Length > 1000 && Regex.IsMatch(input, @"[^a-zA-Z0-9\s]"))
            {
                return false;
            }

            return true;
        }
        public static bool IsValidCurrency(string input)
        {
            decimal value;

            // Kiểm tra chuỗi có hợp lệ là số tiền tệ hay không
            if (decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out value))
            {
                // Kiểm tra giá trị của số tiền tệ có lớn hơn 0 hay không
                return value > 0;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidImage(string imageUrl)
        {
            // Kiểm tra URL của hình ảnh có hợp lệ hay không
            if (string.IsNullOrWhiteSpace(imageUrl) || !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                return false;
            }

            return true;
        }
        //    public static bool IsValidImage(string imageUrl)
        //{
        //    // Kiểm tra URL của hình ảnh có hợp lệ hay không
        //    if (string.IsNullOrWhiteSpace(imageUrl) || !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
        //    {
        //        return false;
        //    }

        //    // Kiểm tra định dạng file của hình ảnh
        //    var extension = Path.GetExtension(imageUrl).ToLowerInvariant();
        //    if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif")
        //    {
        //        return false;
        //    }

        //    // Tải hình ảnh từ URL
        //    byte[] imageData;
        //    using (var wc = new WebClient())
        //    {
        //        try
        //        {
        //            imageData = wc.DownloadData(imageUrl);
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }

        //    // Kiểm tra độ phân giải và độ dài của hình ảnh
        //    using (var stream = new MemoryStream(imageData))
        //    {
        //        using (var image = Image.FromStream(stream))
        //        {
        //            //if (image.HorizontalResolution != 72 || image.VerticalResolution != 72)
        //            //{
        //            //    return false;
        //            //}

        //            var longestSide = Math.Max(image.Width, image.Height);
        //            if (longestSide < 500)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    // Kiểm tra tên file hình ảnh
        //    var fileName = Path.GetFileName(imageUrl);
        //    if (string.IsNullOrWhiteSpace(fileName) || fileName.Any(c => char.IsWhiteSpace(c) || c > 127))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public static string GetKeyNameRegex(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            string pattern = @"^(.*?)\s*-\s*\[";
            Match match = Regex.Match(key, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return key;
        }
        public static string GetKeyRegex(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            string pattern = @"\[\s*(.*?)\s*\]";
            Match match = Regex.Match(key, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
        public static bool IsValidUrl(string url)
        {
            // Kiểm tra URL có hợp lệ hay không
            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isValidUrl;
        }
        public static bool IsValidLength(string input, int length)
        {
            return input.Length > 0 && input.Length <= length;
        }

        public static bool IsValidAlphanumericString(string input, int maxLength)
        {
            // Kiểm tra độ dài của chuỗi
            if (string.IsNullOrEmpty(input) || input.Length < 1 || input.Length > maxLength)
            {
                return false;
            }

            // Kiểm tra các ký tự trong chuỗi
            //foreach (char c in input.Trim())
            //{
            //    if (!char.IsLetterOrDigit(c))
            //    {
            //        return false;
            //    }
            //}

            return true;
        }

        public static bool IsValidTextString(string input)
        {
            // Kiểm tra độ dài của chuỗi
            if (string.IsNullOrEmpty(input) || input.Length > 2000)
            {
                return false;
            }

            // Kiểm tra các ký tự đặc biệt
            foreach (char c in input)
            {
                if ((int)c > 127)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateSearchTerms(string[] searchTerms, string[] competitorBrands = null, string[] competitorAsins = null)
        {
            // Kiểm tra lặp lại từ khóa
            if (searchTerms.Distinct().Count() != searchTerms.Count())
            {
                return false;
            }

            // Kiểm tra sử dụng tên thương hiệu của đối thủ
            if (competitorBrands != null)
            {
                foreach (string brand in competitorBrands)
                {
                    foreach (string term in searchTerms)
                    {
                        if (Regex.IsMatch(term, @"\b" + Regex.Escape(brand) + @"\b", RegexOptions.IgnoreCase))
                        {
                            return false;
                        }
                    }
                }
            }

            // Kiểm tra sử dụng mã ASIN của sản phẩm cạnh tranh
            if (competitorAsins != null)
            {
                foreach (string asin in competitorAsins)
                {
                    if (searchTerms.Contains(asin))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        //A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point.Please do not use commas.
        public static bool ValidateNumber(string number, int numMax, int maxRightIsNumber)
        {
            string pattern = $@"^\d{{1,{numMax}}}(\.\d{{1,{maxRightIsNumber}}})?$";
            return Regex.IsMatch(number, pattern);
        }

        //check repetition
        public static bool ValidateSearchTerms(string searchTerms)
        {
            string[] terms = searchTerms.Split(',');
            HashSet<string> uniqueTerms = new HashSet<string>(terms);
            return uniqueTerms.Count == terms.Length;
        }

        public static bool CheckTwoletter(string countryCode)
        {
            Regex regex = new Regex("[A-Z]{2}"); // create the regular expression pattern
            return regex.IsMatch(countryCode);
        }

        public static bool HazmatUnitedNationals(string hazmatId)
        {
            Regex regex = new Regex(@"^\d{4}UN$"); // create the regular expression pattern
            return regex.IsMatch(hazmatId); // check if the string matches the pattern
        }

        public static bool IsFormatDatetime(string dateString)
        {
            DateTime date;
            return DateTime.TryParseExact(dateString, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out date);
        }
        public static bool IsValidDecimal(string str)
        {
            Regex regex = new Regex(@"^\d+(\.\d+)?$"); // create the regular expression pattern
            return regex.IsMatch(str); // check if the string matches the pattern
        }
        public static bool IsNATOCode(string natoCode)
        {
            Regex regex = new Regex(@"^[A-Z0-9]{13}$"); // create the regular expression pattern
            return regex.IsMatch(natoCode); // check if the string matches the pattern
        }

        public static Dictionary<string, List<object>> listModelfromExcel = GetListModelFromExcel();

        private static Dictionary<string, List<object>> GetListModelFromExcel()
        {
            if(listModelfromExcel == null)
            {
                var lst = new Dictionary<string, List<object>>();
                Dictionary<string, List<object>> item = new();
                Dictionary<string, string> dictDataDefinitions = new();
                List<ValidValueVsiriusModel> lstUpdate = new();

                var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\VsiriusTemplate1.0.xlsm");

                // mở file Excel và đọc dữ liệu
                //var workbook = new Workbook(filePath);

                var package = new ExcelPackage(fileName);

                var worksheet = package.Workbook.Worksheets["Valid Values"];

                var worksheetDataDefinitions = package.Workbook.Worksheets["Data Definitions"];

                for (int row = 4; row <= worksheetDataDefinitions.Dimension.End.Row; row++)
                {
                    if (worksheetDataDefinitions.Cells[row, 3].Value == null && worksheetDataDefinitions.Cells[row, 1].Value != null)
                    {
                        worksheetDataDefinitions.Cells[row, 3].Value = worksheetDataDefinitions.Cells[row, 1].Value;
                    }
                    if (worksheetDataDefinitions.Cells[row, 2].Value == null)
                    {
                        worksheetDataDefinitions.Cells[row, 2].Value = string.Empty;
                    }
                    if (dictDataDefinitions.ContainsKey($"{worksheetDataDefinitions.Cells[row, 3].Value}"))
                    {
                        worksheetDataDefinitions.Cells[row, 3].Value += $"{row}";
                    }

                    dictDataDefinitions.Add(worksheetDataDefinitions.Cells[row, 3].Value.ToString(), worksheetDataDefinitions.Cells[row, 2].Value.ToString());
                }

                // lấy tên cột từ file Excel
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (worksheet.Cells[row, 2].Value == null && worksheet.Cells[row, 1].Value == null) { break; }

                    if (worksheet.Cells[row, 2].Value == null)
                    {
                        continue;
                    }

                    List<object> lstValue = new();
                    List<string> lstValueUpdate = new();

                    for (int col = 3; col < worksheet.Dimension.End.Column; col++)
                    {
                        if (worksheet.Cells[row, col].Value == null) { break; }

                        lstValue.Add(worksheet.Cells[row, col].Value);
                        lstValueUpdate.Add(worksheet.Cells[row, col].Value.ToString());
                    }

                    string cell = worksheet.Cells[row, 2].Value.ToString();

                    if (Utilities.GetKeyNameRegex(cell).Contains("Additional Chemical Name"))
                    {
                        continue;
                    }

                    string cellMapModel = dictDataDefinitions.GetValueOrDefault(Utilities.GetKeyNameRegex(cell));

                    List<string> lstKey = new();
                    ValidValueVsiriusModel model;

                    //tìm value chứa item giống nhau nhưng có stt ví dụ matarial_type1-matarial_type3
                    if (cellMapModel.Contains("-"))
                    {
                        var str1 = cellMapModel.Split('-')[0].Trim();
                        var str2 = cellMapModel.Split('-')[1].Trim();
                        lstKey.Add(str1);
                        lstKey.Add(str2);

                        for (int i = 2; i < int.Parse(str2[str2.Length - 1].ToString()); i++)
                        {
                            lstKey.Add(str1.Substring(0, str1.Length - 1) + i);
                        }

                    }

                    if (lstKey.Count > 0)
                    {
                        foreach (string str in lstKey)
                        {
                            model = new();
                            if (Utilities.GetKeyRegex(cell) == string.Empty)
                            {
                                item.Add(str, lstValue);
                                //update db
                                model.Key = str;
                                model.Value = string.Join("|", lstValueUpdate);
                                lstUpdate.Add(model);

                            }
                            else
                            {
                                item.Add(str + "_" + Utilities.GetKeyRegex(cell), lstValue);
                                //update db
                                model.Key = str + "|" + Utilities.GetKeyRegex(cell);
                                model.Value = string.Join("|", lstValueUpdate);
                                lstUpdate.Add(model);

                            }
                        }
                        continue;
                    }
                    model = new();
                    if (Utilities.GetKeyRegex(cell) == string.Empty)
                    {
                        item.Add(cellMapModel, lstValue);
                        //update db
                        model.Key = cellMapModel;
                        model.Value = string.Join("|", lstValueUpdate);
                        lstUpdate.Add(model);

                    }
                    else
                    {
                        item.Add(cellMapModel + "_" + Utilities.GetKeyRegex(cell), lstValue);
                        //update db
                        model.Key = cellMapModel + "|" + Utilities.GetKeyRegex(cell);
                        model.Value = string.Join("|", lstValueUpdate);
                        lstUpdate.Add(model);

                    }

                }
                //await UpdateDb(lstUpdate);

                return item;
            }
            return listModelfromExcel!;
        }
    }
}
