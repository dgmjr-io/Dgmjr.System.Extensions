namespace Microsoft.Extensions.DependencyInjection;

public static class AzureAdApplicationBuilderIdentityExtensions
{
    public static IApplicationBuilder UseAzureAdB2CIdentity(
        this IApplicationBuilder app
    )
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
