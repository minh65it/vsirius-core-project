using System.Text.RegularExpressions;

namespace OrderProcessApi.Utilities
{
    public static class Util
    {
        public static string RemoveFirstChar(string? sku)
        {
            string pattern = @"^(QU-|Q-|S-)";
            string newSku = sku is not null ? Regex.Replace(sku, pattern, "") : "";

            return newSku;
        }
    }
}
