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
using System.IO;

public static class Extensions
{
    public static string ReadAssemblyResourceAllText(this Assembly assembly, string resourceName)
        => assembly.GetManifestResourceStream(resourceName).ReadToEnd();


    public static Task<string> ReadAssemblyResourceAllTextAsync(
        this Assembly assembly,
        string resourceName
    )
    => assembly.GetManifestResourceStream(resourceName).ReadToEndAsync();
}
