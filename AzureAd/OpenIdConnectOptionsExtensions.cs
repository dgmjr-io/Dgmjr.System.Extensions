namespace Microsoft.AspNetCore.Authentication.OpenIdConnect.Extensions;

public static class OpenIdConnectOptionsExtensions
{
    public static bool IsUsingClientSecret(this OpenIdConnectOptions options) =>
        !IsNullOrEmpty(options.ClientSecret);

    public static bool IsUsingClientSecret(this MicrosoftIdentityOptions options) =>
        options.ClientCredentials.Any(cc => cc.CredentialType == CredentialType.Secret);

    public static bool IsUsingClientCertificate(this MicrosoftIdentityOptions options) =>
        options.ClientCredentials.Any(cc => cc.CredentialType == CredentialType.Certificate);
}
