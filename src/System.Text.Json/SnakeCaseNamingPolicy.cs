namespace System.Text.Json;
using System.Text.RegularExpressions;

using static System.Text.RegularExpressions.RegexOptions;

public class SnakeCaseNamingPolicy
#if NET6_0_OR_GREATER
    : JsonNamingPolicy
#endif
{
    public
#if NET6_0_OR_GREATER
    override
#endif
    string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}

public static partial class SnakeCaseExtensions
{
    //     public const string ToSnakeCaseRegexString = @"([a-zA-Z])([A-Z][a-zA-Z])";
    //     public const string SnakeCaseToCamelCaseRegexString = @"_([a-z])";
    //     public const string SnakeCaseToPascalCaseRegexString = @"(?:(?:^)|(?:_))([a-z])";
    //     public const RegexOptions RegexOptions = Singleline | Compiled;

    // #if NET7_0_OR_GREATER
    //     [GeneratedRegex(ToSnakeCaseRegexString, RegexOptions)]
    //     public static partial Regex ToSnakeCaseRegex();

    //     [GeneratedRegex(SnakeCaseToCamelCaseRegexString, RegexOptions)]
    //     public static partial Regex SnakeCaseToCamelCaseRegex();

    //     [GeneratedRegex(SnakeCaseToPascalCaseRegexString, RegexOptions)]
    //     public static partial Regex SnakeCaseToPascalCaseRegex();
    // #else
    //     private static Regex _toSnakeCaseRegex = new(ToSnakeCaseRegexString, RegexOptions);
    //     public static Regex ToSnakeCaseRegex() => _toSnakeCaseRegex;
    //     private static Regex _snakeCaseToCamelCaseRegex = new(SnakeCaseToCamelCaseRegexString, RegexOptions);
    //     public static Regex SnakeCaseToCamelCaseRegex() => _snakeCaseToCamelCaseRegex;
    //     private static Regex _snakeCaseToPascalCaseRegex = new(SnakeCaseToPascalCaseRegexString, RegexOptions);
    //     public static Regex SnakeCaseToPascalCaseRegex() => _snakeCaseToPascalCaseRegex;
    // #endif

    /// <summary>
    /// Converts the <paramref name="input" /> to snake_case
    /// </summary>
    /// <param name="input"></param>
    public static string ToSnakeCase(this string input) => input.ToCasing('_');

    /// <summary>
    /// Converts the <paramref name="input" /> to camelCase from snake_case
    /// </summary>
    /// <param name="input"></param>
    public static string SnakeCaseToCamelCase(this string input) => FromCasing(input, '_', false);

    /// <summary>
    /// Converts the <paramref name="input" /> to PascalCase from snake_case
    /// </summary>
    /// <param name="input"></param>
    public static string SnakeCaseToPascalCase(this string input) => FromCasing(input, '_', true);

    private static string FromCasing(
        this string input,
        char separator,
        bool shouldCapitalizeFirstLetter = false
    ) => InternalCaseChangingExtensions.FromCasing(input, separator, shouldCapitalizeFirstLetter);
}