
namespace System.Text.Json;
using System.Text.RegularExpressions;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}

public static partial class SnakeCaseExtensions
{
    public const string ToSnakeCaseRegexString = @"(.)([A-Z][a-z])";
    public const string SnakeCaseToCamelCaseRegexString = @"_([a-z])";
    public const string SnakeCaseToPascalCaseRegexString = @"^([a-z])";

#if NET7_0_OR_GREATER
    [GeneratedRegex(ToSnakeCaseRegexString)]
    public static partial Regex ToSnakeCaseRegex();

    [GeneratedRegex(SnakeCaseToCamelCaseRegexString)]
    public static partial Regex SnakeCaseToCamelCaseRegex();

    [GeneratedRegex(SnakeCaseToPascalCaseRegexString)]
    public static partial Regex SnakeCaseToPascalCaseRegex();
#else
    public static Regex ToSnakeCaseRegex() => new(ToSnakeCaseRegexString);

    public static Regex SnakeCaseToCamelCaseRegex() => new(SnakeCaseToCamelCaseRegexString);

    public static Regex SnakeCaseToPascalCaseRegex() => new(SnakeCaseToPascalCaseRegexString);
#endif

    public static string ToSnakeCase(this string str) =>
        ToSnakeCaseRegex().Replace(str, s => $"{s.Groups[1].Value}_{s.Groups[2].Value}").ToLower();

    public static string SnakeCaseToCamelCase(this string str) =>
        SnakeCaseToCamelCaseRegex().Replace(str, @"\U$1");

    public static string SnakeCaseToPascalCase(this string str) =>
        SnakeCaseToPascalCaseRegex().Replace(str, @"\U$1");
}
