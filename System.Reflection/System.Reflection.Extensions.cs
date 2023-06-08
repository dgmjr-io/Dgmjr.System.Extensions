/*
 * System.Reflection.Extensions.cs
 *
 *   Created: 2023-05-18-01:27:54
 *   Modified: 2023-05-18-01:27:55
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Reflection;

public static class Extensions
{
    public static string ReadAssemblyResourceAllText(this Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static Task<string> ReadAssemblyResourceAllTextAsync(
        this Assembly assembly,
        string resourceName
    )
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        return reader.ReadToEndAsync();
    }
}
