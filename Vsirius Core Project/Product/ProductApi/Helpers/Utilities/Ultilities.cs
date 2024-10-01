using System.Globalization;
using System.Text.RegularExpressions;

namespace ProductApi.Helpers.Utilities
{
    public class Ultilities
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
            string pattern = @"^\d{1,18}$";
            var ddd = Regex.IsMatch(input, pattern);
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
            foreach (char c in input.Trim())
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

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
    }
}
