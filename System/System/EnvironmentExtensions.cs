/*
 * EnvironmentExtensions.cs
 *     Created: 2024-36-09T06:36:02-05:00
 *    Modified: 2024-32-19T13:32:03-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

public static class EnvironmentExtensions
{
    public static string GetEnvironmentVariable(
        this EnvironmentVariableTarget target,
        string name,
        string? defaultValue = null
    ) => env.GetEnvironmentVariable(name, target) ?? defaultValue ?? "";

    public static string GetEnvironmentVariable(
        this EnvironmentVariableTarget target,
        string name,
        Func<string> defaultConstructor
    ) => env.GetEnvironmentVariable(name, target) ?? defaultConstructor();

    public static string GetEnvironmentName(this EnvironmentVariableTarget target) =>
        target.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
}
