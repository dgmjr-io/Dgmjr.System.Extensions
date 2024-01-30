namespace Microsoft.Extensions.DependencyInjection;

public class AutomaticAzureAdConfigurator
    : IConfigureIHostApplicationBuilder,
        IConfigureIApplicationBuilder
{
    public ConfigurationOrder Order => ConfigurationOrder.VeryEarly;

    public void Configure(WebApplicationBuilder builder)
    {
        builder.AddAzureAdB2CIdentity();
    }

    public void Configure(IApplicationBuilder options)
    {
        options.UseAzureAdB2CIdentity();
    }
}
