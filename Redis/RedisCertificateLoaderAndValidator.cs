
namespace Dgmjr.Redis;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using global::Dgmjr.Abstractions;

public class RedisCertificateLoaderAndValidator(IConfiguration configuration, ILogger<RedisCertificateLoaderAndValidator> logger) : IPostConfigureOptions<RedisCacheOptions>, ILog
{
    private const string RedisCertificatePath = "Redis:ClientCertificatePath";
private const string RedisClientCertificatePassword = "Redis:ClientCertificatePassword";

private readonly IConfiguration _configuration = configuration;

public ILogger Logger => logger;

private string ClientCertificatePath => _configuration[RedisCertificatePath]!;
private string ClientCertificatePassword => _configuration[RedisClientCertificatePassword]!;

private bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
{
    var subject = certificate?.Subject ?? "null";
    var issuer = certificate?.Issuer ?? "null";
    var chainInfo = Join(" => ", chain?.ChainElements.Cast<X509ChainElement>().Select(x => x.Certificate.Subject).ToArray());

    if (sslPolicyErrors == SslPolicyErrors.None)
    {
        Logger.LogValidatingRedisServerCertificateCertificate(subject, issuer, chainInfo);
        return true;
    }
    else
    {
        Logger.LogFailedValidatingRedisServerCertificateCertificate(subject, issuer, chainInfo, sslPolicyErrors);
        return true;
    }
}

private X509Certificate2 GetClientCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate? remoteCertificate, string[] acceptableIssuers)
{
    Logger.LogLoadingRedisClientCertificate(ClientCertificatePath);
    var certificate = new X509Certificate2(ClientCertificatePath, ClientCertificatePassword);
    return certificate;
}

public void PostConfigure(string? name, RedisCacheOptions options)
{
    options.ConfigurationOptions.Ssl = true;
    options.ConfigurationOptions.CertificateSelection += GetClientCertificate;
    options.ConfigurationOptions.CertificateValidation += ValidateServerCertificate;
}
}
