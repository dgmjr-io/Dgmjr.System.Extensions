/*
 * ObjectExtensions.cs
 *     Created: 2024-06-23T03:06:18-05:00
 *    Modified: 2024-30-19T13:30:07-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

public static class ObjectExtensions
{
    public static Stream GetManifestResourceStream<T>(this T _, string resourceName) =>
        typeof(T).Assembly.GetManifestResourceStream(resourceName)
        ?? throw new FileNotFoundException(
            $"Resource {resourceName} not found in assembly {typeof(T).Assembly.FullName}"
        );

    public static byte[] GetManifestResourceBytes<T>(this T _, string resourceName) =>
        typeof(T).Assembly.GetManifestResourceStream(resourceName)?.ReadAllBytes()
        ?? throw new FileNotFoundException(
            $"Resource {resourceName} not found in assembly {typeof(T).Assembly.FullName}"
        );

    public static async Task<byte[]> GetManifestResourceBytesAsync<T>(
        this T _,
        string resourceName
    ) =>
        (
            await (
                typeof(T).Assembly.GetManifestResourceStream(resourceName)?.ReadAllBytesAsync()
                ?? Task.FromResult(default(byte[]))
            )
        )
        ?? throw new FileNotFoundException(
            $"Resource {resourceName} not found in assembly {typeof(T).Assembly.FullName}"
        );

    public static string ReadAssemblyResourceAllText<T>(this T _, string resourceName) =>
        Extensions.ReadAssemblyResourceAllText(typeof(T).Assembly, resourceName);

    public static async Task<string> ReadAssemblyResourceAllTextAsync<T>(
        this T _,
        string resourceName
    ) => await Extensions.ReadAssemblyResourceAllTextAsync(typeof(T).Assembly, resourceName);

    public static T To<T>(this object value) => (T)Convert.ChangeType(value, typeof(T));

    public static object? GetPropertyValue(this object obj, string propertyName)
    {
        return obj.GetType()
            .GetRuntimeProperties()
            .FirstOrDefault(pi => pi.Name.Equals(propertyName, OrdinalIgnoreCase))
            ?.GetValue(obj);
    }

    public static T? GetPropertyValue<T>(this object obj, string propertyName)
    {
        return (T?)
            obj.GetType()
                .GetRuntimeProperties()
                .FirstOrDefault(pi => pi.Name.Equals(propertyName, OrdinalIgnoreCase))
                ?.GetValue(obj);
    }
}
