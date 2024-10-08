public static class StringExtensions
{
    public static string ToCapitalizedCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";
        string[] words = input.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].ToTitleCase();
        }

        return string.Join(" ", words);
    }

    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
