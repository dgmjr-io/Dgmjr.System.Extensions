/*
 * ObjectQueryStringExtensions.cs
 *     Created: 2024-04-27T19:45:11-04:00
 *    Modified: 2024-04-27T19:45:13-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Net.Http;

public static class ObjectQueryStringExtensions
{
    // public static string ToQueryString<T>(this T t)
    // {
    //     return (t as object).ToQueryString(typeof(T).GetRuntimeProperties().ToArray());
    // }

    public static string ToQueryString(this object o)
    {
        var properties = o.GetType().GetRuntimeProperties().ToArray();
        var sb = new StringBuilder();
        foreach (var property in properties)
        {
            var value = property.GetValue(o);

            // if (property.GetCustomAttributes<JConverterAttribute>().Any())
            // {
            //     var converter = property.GetCustomAttribute<JConverterAttribute>().ConverterType;
            //     var instance = Activator.CreateInstance(converter) as JConverter;
            //     var method = converter.GetMethod("Write");
            //     method.Invoke(instance, new object[] { sb, value, null });

            // }

            if (value is null)
            {
                continue;
            }
            else if (value is Enum e)
            {
                value = e.GetEnumMemberValue() ?? e.GetName() ?? e.ToString();
            }

            if (sb.Length > 0)
            {
                sb.Append('&');
            }

            var propName = (
                property.GetCustomAttribute<JPropAttribute>()?.Name
                ?? property.Name.FromCasing('\0', false)
            );

            sb.Append($"{propName}={value}");
        }

        return sb.ToString();
    }
}
