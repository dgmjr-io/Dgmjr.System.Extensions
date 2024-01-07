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
    internal static partial void LoadingRedisClientCertificate(this ILogger logger, string path);

    [LoggerMessage(
        Trace,
        "Validating Redis server certificate: ✅ SUCCESS! ✅ \n\tSubject: {Subject}\n\tIssuer: {Issuer}\n\tChain: {ChainInfo}",
        EventName = "ValidatingRedisServerCertificateCertificate"
    )]
    internal static partial void ValidatingRedisServerCertificateCertificate(this ILogger logger, string subject, string issuer, string chainInfo);

    [LoggerMessage(
        Trace,
        "Validating Redis server certificate: 🚫 !!FAILED!! 🚫: \n\tSubject: {Subject}\n\tIssuer: {Issuer}\n\tChain: {ChainInfo}\r\n\tSslPolicyErrors: {SslPolicyErrors}",
        EventName = "ValidatingRedisServerCertificateCertificate"
    )]
    internal static partial void FailedValidatingRedisServerCertificateCertificate(this ILogger logger, string subject, string issuer, string chainInfo, SslPolicyErrors sslPolicyErrors);
}
