namespace System;

public static class EnvironmentExtensions
{
    public static string GetEnvironmentVariable(
        this EnvironmentVariableTarget target,
        string name,
        string? defaultValue = null
    ) => Environment.GetEnvironmentVariable(name, target) ?? defaultValue ?? "";

    public static string GetEnvironmentVariable(
        this EnvironmentVariableTarget target,
        string name,
        Func<string> defaultConstructor
    ) => Environment.GetEnvironmentVariable(name, target) ?? defaultConstructor();

    public static string GetEnvironmentName(this EnvironmentVariableTarget target) =>
        target.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
}
