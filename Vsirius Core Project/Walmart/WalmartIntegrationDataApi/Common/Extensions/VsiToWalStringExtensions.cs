using System.Text.RegularExpressions;

namespace WalmartIntegrationDataApi.Common.Extensions
{
    public static class VsiToWalStringExtensions
    {
        public static string ToSimpleString(this string str)
        {
            string pattern = "[^A-Za-z0-9\\s-]";
            string output = Regex.Replace(str, pattern, "");
            return output.Replace(' ', '-');
        }

        public static bool IsNumber(this string str)
        {
            return double.TryParse(str, out _);
        }
    }
}
