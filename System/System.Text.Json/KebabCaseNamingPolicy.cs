/*
 * KebabCaseNamingPolicy.cs
 *
 *   Created: 2023-08-11-06:12:55
 *   Modified: 2023-08-11-06:12:55
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Text.Json;

using System.Text.RegularExpressions;

using static System.Text.RegularExpressions.RegexOptions;

public class KebabCaseNamingPolicy
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
        return name.ToKebabCase();
    }
}

public static partial class KebabCaseExtensions
{
    //     public const string ToKebabCaseRegexString = @"([a-zA-Z])([A-Z][a-zA-Z])";
    //     public const string KebabCaseToCamelCaseRegexString = @"-([a-z])";
    //     public const string KebabCaseToPascalCaseRegexString = @"(?:(?:^)|(?:\-))([a-z])";
    //     public const RegexOptions RegexOptions = Singleline | Compiled;

    // #if NET7_0_OR_GREATER
    //     [GeneratedRegex(ToKebabCaseRegexString, RegexOptions)]
    //     public static partial Regex ToKebabCaseRegex();

    //     [GeneratedRegex(KebabCaseToCamelCaseRegexString, RegexOptions)]
    //     public static partial Regex KebabCaseToCamelCaseRegex();

    //     [GeneratedRegex(KebabCaseToPascalCaseRegexString, RegexOptions)]
    //     public static partial Regex KebabCaseToPascalCaseRegex();
    // #else
    //     private static Regex _toKebabCaseRegex = new(ToKebabCaseRegexString, RegexOptions);
    //     public static Regex ToKebabCaseRegex() => _toKebabCaseRegex;
    //     private static Regex _snakeCaseToCamelCaseRegex = new(KebabCaseToCamelCaseRegexString, RegexOptions);
    //     public static Regex KebabCaseToCamelCaseRegex() => _snakeCaseToCamelCaseRegex;
    //     private static Regex _snakeCaseToPascalCaseRegex = new(KebabCaseToPascalCaseRegexString, RegexOptions);
    //     public static Regex KebabCaseToPascalCaseRegex() => _snakeCaseToPascalCaseRegex;
    // #endif

    /// <summary>
    /// Converts the <paramref name="input" /> to kebab-case
    /// </summary>
    /// <param name="input"></param>
    public static string ToKebabCase(this string input) => input.ToCasing('-');

    /// <summary>
    /// Converts the <paramref name="input" /> to camelCase from kebab-case
    /// </summary>
    /// <param name="input"></param>
    public static string KebabCaseToCamelCase(this string input) => FromCasing(input, '-', false);

    /// <summary>
    /// Converts the <paramref name="input" /> to PascalCaaw from kebab-case
    /// </summary>
    /// <param name="input"></param>
    public static string KebabCaseToPascalCase(this string input) => FromCasing(input, '-', true);

    private static string FromCasing(
        this string input,
        char separator,
        bool shouldCapitalizeFirstLetter = false
    ) => InternalCaseChangingExtensions.FromCasing(input, separator, shouldCapitalizeFirstLetter);
}
