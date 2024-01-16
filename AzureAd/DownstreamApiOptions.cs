namespace Dgmjr.AzureAd;

public class DownstreamApiOptions : Microsoft.Identity.Abstractions.DownstreamApiOptions
{
    public type ServiceInterface { get; set; }
    public type ServiceImplementation { get; set; }
}
