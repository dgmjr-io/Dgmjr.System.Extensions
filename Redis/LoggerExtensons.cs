namespace Dgmjr.Redis.Extensions;
using Microsoft.Extensions.Logging;
using static Microsoft.Extensions.Logging.LogLevel;
using System.Net.Security;

public static partial class LoggerExtensions
{
    [LoggerMessage(
    Trace,
    "Loading Redis client certificate from {Path} with password *******",
    EventName = "LoadingRedisClientCertificate"
    )]
    public static partial void LogLoadingRedisClientCertificate(this ILogger logger, string path);

    [LoggerMessage(
        Trace,
        "Validating Redis server certificate: ✅ SUCCESS! ✅ \n\tSubject: {Subject}\n\tIssuer: {Issuer}\n\tChain: {ChainInfo}",
        EventName = "ValidatingRedisServerCertificateCertificate"
    )]
    public static partial void LogValidatingRedisServerCertificateCertificate(this ILogger logger, string subject, string issuer, string chainInfo);

    [LoggerMessage(
        Trace,
        "Validating Redis server certificate: 🚫 !!FAILED!! 🚫: \n\tSubject: {Subject}\n\tIssuer: {Issuer}\n\tChain: {ChainInfo}\r\n\tSslPolicyErrors: {SslPolicyErrors}",
        EventName = "ValidatingRedisServerCertificateCertificate"
    )]
    public static partial void LogFailedValidatingRedisServerCertificateCertificate(this ILogger logger, string subject, string issuer, string chainInfo, SslPolicyErrors sslPolicyErrors);
}
