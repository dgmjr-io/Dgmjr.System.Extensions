namespace Microsoft.Extensions.DependencyInjection;

using Dgmjr.Identity.Web;

public static class AzureAdApplicationBuilderIdentityExtensions
{
    public static IApplicationBuilder UseAzureAdB2CIdentity(this IApplicationBuilder app)
    {
        var mvcOptions = app.ApplicationServices
            .GetService<IOptions<Dgmjr.AspNetCore.Mvc.MvcOptions>>()
            ?.Value;
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            if (mvcOptions?.AddControllers == true)
                endpoints.MapControllers();
            if (mvcOptions?.AddRazorPages == true)
                endpoints.MapRazorPages();
        });
        return app;
    }
}
