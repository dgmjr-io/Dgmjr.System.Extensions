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
    public const string ToSnakeCaseRegexString = @"(.)([A-Z][a-z])";
    public const string SnakeCaseToCamelCaseRegexString = @"_([a-z])";
    public const string SnakeCaseToPascalCaseRegexString = @"^([a-z])";
    public const RegexOptions RegexOptions = Singleline | Compiled;

#if NET7_0_OR_GREATER
    [GeneratedRegex(ToSnakeCaseRegexString, RegexOptions)]
    public static partial Regex ToSnakeCaseRegex();

    [GeneratedRegex(SnakeCaseToCamelCaseRegexString, RegexOptions)]
    public static partial Regex SnakeCaseToCamelCaseRegex();

    [GeneratedRegex(SnakeCaseToPascalCaseRegexString, RegexOptions)]
    public static partial Regex SnakeCaseToPascalCaseRegex();
#else
    private static Regex _toSnakeCaseRegex = new(ToSnakeCaseRegexString, RegexOptions);
    public static Regex ToSnakeCaseRegex() => _toSnakeCaseRegex;
    private static Regex _snakeCaseToCamelCaseRegex = new(SnakeCaseToCamelCaseRegexString, RegexOptions);
    public static Regex SnakeCaseToCamelCaseRegex() => _snakeCaseToCamelCaseRegex;
    private static Regex _snakeCaseToPascalCaseRegex = new(SnakeCaseToPascalCaseRegexString, RegexOptions);
    public static Regex SnakeCaseToPascalCaseRegex() => _snakeCaseToPascalCaseRegex;
#endif

    public static string ToSnakeCase(this string str) =>
        ToSnakeCaseRegex().Replace(str, s => $"{s.Groups[1].Value}_{s.Groups[2].Value}").ToLowerInvariant();

    public static string SnakeCaseToCamelCase(this string str) =>
        SnakeCaseToCamelCaseRegex().Replace(str, @"\U$1");

    public static string SnakeCaseToPascalCase(this string str) =>
        SnakeCaseToPascalCaseRegex().Replace(str, @"\U$1");
}
