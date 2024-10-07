using System;
using System.Globalization;

public static class StringExtensions
{
    public static bool IsNumber(this string str)
    {
        try
        {
            decimal number = decimal.Parse(str);
            return true;
        }
        catch
        {
            return false;
        }

    }

    public static bool IsUrl(this string str)
    {
        Uri uriResult = null!;
        bool isValidUrl = Uri.TryCreate(str, UriKind.Absolute, out uriResult!) 
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (isValidUrl)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static bool IsDate(this string str)
    {
        try
        {
            var date = DateTime.ParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }

    }
}

