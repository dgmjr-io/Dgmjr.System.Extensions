namespace Microsoft.AspNetCore.Authentication.OpenIdConnect.Extensions;

public static class OpenIdConnectOptionsExtensions
{
    public static bool IsUsingClientSecret(this OpenIdConnectOptions options) =>
        !IsNullOrEmpty(options.ClientSecret);
}
