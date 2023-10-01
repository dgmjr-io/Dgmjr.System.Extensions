using System.Runtime.InteropServices;

namespace System.Text.Json;

internal static class InternalCaseChangingExtensions
{
    public static string ToCasing(this string input, char separator)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new StringBuilder();

        foreach (var c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append(separator);
            }

            result.Append(char.ToLower(c));
        }

        return result.ToString();
    }

    public static string FromCasing(this string input, char separator, bool capitalizeFirstLetter)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var result = new StringBuilder();
        var words = input.Split(separator);

        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                result.Append(char.ToUpper(word[0]));
                result.Append(word.Substring(1).ToLower());
            }
        }

        if (capitalizeFirstLetter)
        {
            return result.ToString();
        }
        else
        {
            return result.Replace(result[0], char.ToLower(result[0]), 0, 1).ToString();
        }
    }
}
