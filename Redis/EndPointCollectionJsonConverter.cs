namespace Dgmjr.Redis.Extensions;

using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using StackExchange.Redis;

public class EndPointCollectionJsonConverter : JsonConverter<EndPointCollection>
{
    private const int StandardPort = 6379;

    private const string Localhost = "localhost";

    public override bool CanConvert(type typeToConvert)
    {
        return typeof(EndPointCollection).IsAssignableFrom(typeToConvert);
    }

    public override EndPointCollection Read(
        ref Utf8JsonReader reader,
        type typeToConvert,
        Jso options
    )
    {
        var endpoints = new EndPointCollection();

        while (reader.Read())
        {
            if (reader.TokenType == JTokenType.EndObject)
            {
                return endpoints;
            }
            else if (reader.TokenType == JTokenType.StartArray)
            {
                while (reader.Read())
                {
                    var endpoint = default(EndPoint);

                    if (reader.TokenType == JTokenType.EndArray)
                    {
                        break;
                    }
                    else if (reader.TokenType == JTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString();
                        reader.Read();
                        switch (propertyName)
                        {
                            case nameof(DnsEndPoint.Host):
                                var host = reader.GetString();
                                if (IPAddress.TryParse(host, out var address))
                                {
                                    endpoint ??= new IPEndPoint(address, StandardPort);
                                }
                                else
                                {
                                    endpoint ??= new DnsEndPoint(host, StandardPort);
                                }
                                break;
                            case nameof(DnsEndPoint.Port):
                                var port = reader.GetInt32();
                                if (endpoint is IPEndPoint ipEndPoint)
                                {
                                    endpoint = new IPEndPoint(ipEndPoint.Address, port);
                                }
                                else
                                {
                                    endpoint = endpoint is DnsEndPoint dnsEndPoint
                                        ? new DnsEndPoint(dnsEndPoint.Host, port)
                                        : (EndPoint)new DnsEndPoint(Localhost, port);
                                }
                                break;
                            case nameof(IPEndPoint.Address):
                                var addressString = reader.GetString();
                                if (IPAddress.TryParse(addressString, out address))
                                {
                                    endpoint ??= new IPEndPoint(address, StandardPort);
                                }
                                else
                                {
                                    endpoint ??= new DnsEndPoint(addressString, StandardPort);
                                }
                                if (endpoint is IPEndPoint ipEndPoint2)
                                {
                                    endpoint = new IPEndPoint(
                                        ipEndPoint2.Address,
                                        (endpoint as IPEndPoint)?.Port
                                            ?? (endpoint as DnsEndPoint)?.Port
                                            ?? StandardPort
                                    );
                                }
                                else if (endpoint is DnsEndPoint dnsEndPoint)
                                {
                                    endpoint = new DnsEndPoint(
                                        dnsEndPoint.Host,
                                        (endpoint as IPEndPoint)?.Port
                                            ?? (endpoint as DnsEndPoint)?.Port
                                            ?? StandardPort
                                    );
                                }
                                break;
#if NETSTANDARD2_1_OR_GREATER
                            case nameof(System.Net.Sockets.UnixDomainSocketEndPoint):
                                endpoint ??= new System.Net.Sockets.UnixDomainSocketEndPoint(
                                    reader.GetString()
                                );
                                break;
#endif
                            default:
                                // ignore
                                break;
                        }
                        endpoints.Add(endpoint);
                    }
                    else if (reader.TokenType == JTokenType.String)
                    {
                        endpoints.Add(reader.GetString());
                    }
                }
            }
        }

        return endpoints;

        // var config = new StackExchange.Redis.ConfigurationOptions();

        // while(reader.Read())
        // {
        //     if (reader.TokenType == JsonTokenType.EndObject)
        //     {
        //         return config;
        //     }
        //     if (reader.TokenType == JsonTokenType.PropertyName)
        //     {
        //         var propertyName = reader.GetString();
        //         reader.Read();
        //         switch (propertyName)
        //         {
        //             case "AbortOnConnectFail":
        //                 config.AbortOnConnectFail = reader.GetBoolean();
        //                 break;
        //             case "AllowAdmin":
        //                 config.AllowAdmin = reader.GetBoolean();
        //                 break;
        //             case "ChannelPrefix":
        //                 config.ChannelPrefix = reader.GetString();
        //                 break;
        //             case "ClientName":
        //                 config.ClientName = reader.GetString();
        //                 break;
        //             case "ConnectRetry":
        //                 config.ConnectRetry = reader.GetInt32();
        //                 break;
        //             case "ConnectTimeout":
        //                 config.ConnectTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                 break;
        //             case "ConfigCheckSeconds":
        //                 config.ConfigCheckSeconds = reader.GetInt32();
        //                 break;
        //             case "DefaultDatabase":
        //                 config.DefaultDatabase = reader.GetInt32();
        //                 break;
        //             case "KeepAlive":
        //                 config.KeepAlive = reader.GetInt32();
        //                 break;
        //             case "NameResolver":
        //                 config.NameResolver = Deserialize<StackExchange.Redis.IRedisNameResolver>(ref reader, options);
        //                 break;
        //             case "Password":
        //                 config.Password = reader.GetString();
        //                 break;
        //             case "Proxy":
        //                 config.Proxy = Deserialize<StackExchange.Redis.RedisProxy>(ref reader, options);
        //                 break;
        //             case "ResolveDns":
        //                 config.ResolveDns = reader.GetBoolean();
        //                 break;
        //             case "ResponseTimeout":
        //                 config.ResponseTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                 break;
        //             case "ServiceName":
        //                 config.ServiceName = reader.GetString();
        //                 break;
        //             case "Ssl":
        //                 config.Ssl = reader.GetBoolean();
        //                 break;
        //             case "SslHost":
        //                 config.SslHost = reader.GetString();
        //                 break;
        //             case "SyncTimeout":
        //                 config.SyncTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                 break;
        //             case "TieBreaker":
        //                 config.TieBreaker = reader.GetString();
        //                 break;
        //             case "Version":
        //                 config.Version = reader.GetInt32();
        //                 break;
        //             case "WriteBuffer":
        //                 config.WriteBuffer = reader.GetInt32();
        //                 break;
        //             default:
        //                 throw new NotImplementedException();
        //         }
        //     }
        // }

        //     var options = new Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions();
        //     while (reader.Read())
        //     {
        //         if (reader.TokenType == JsonTokenType.EndObject)
        //         {
        //             return options;
        //         }
        //         if (reader.TokenType == JsonTokenType.PropertyName)
        //         {
        //             var propertyName = reader.GetString();
        //             reader.Read();
        //             switch (propertyName)
        //             {
        //                 case "Configuration":
        //                     options.Configuration = reader.GetString();
        //                     break;
        //                 case "InstanceName":
        //                     options.InstanceName = reader.GetString();
        //                     break;
        //                 case "ConfigurationOptions":
        //                     options.ConfigurationOptions = Deserialize<Microsoft.Extensions.Caching.StackExchangeRedis.ConfigurationOptions>(ref reader, options);
        //                     break;
        //                 case "ConnectTimeout":
        //                     options.ConnectTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                     break;
        //                 case "ResponseTimeout":
        //                     options.ResponseTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                     break;
        //                 case "Serializer":
        //                     options.Serializer = Deserialize<Microsoft.Extensions.Caching.StackExchangeRedis.IRedisSerializer>(ref reader, options);
        //                     break;
        //                 case "AbortOnConnectFail":
        //                     options.AbortOnConnectFail = reader.GetBoolean();
        //                     break;
        //                 case "AllowAdmin":
        //                     options.AllowAdmin = reader.GetBoolean();
        //                     break;
        //                 case "ChannelPrefix":
        //                     options.ChannelPrefix = reader.GetString();
        //                     break;
        //                 case "ClientName":
        //                     options.ClientName = reader.GetString();
        //                     break;
        //                 case "ConnectRetry":
        //                     options.ConnectRetry = reader.GetInt32();
        //                     break;
        //                 case "ConnectTimeout":
        //                     options.ConnectTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                     break;
        //                 case "ConfigCheckSeconds":
        //                     options.ConfigCheckSeconds = reader.GetInt32();
        //                     break;
        //                 case "DefaultDatabase":
        //                     options.DefaultDatabase = reader.GetInt32();
        //                     break;
        //                 case "KeepAlive":
        //                     options.KeepAlive = reader.GetInt32();
        //                     break;
        //                 case "NameResolver":
        //                     options.NameResolver = Deserialize<Microsoft.Extensions.Caching.StackExchangeRedis.IRedisNameResolver>(ref reader, options);
        //                     break;
        //                 case "Password":
        //                     options.Password = reader.GetString();
        //                     break;
        //                 case "Proxy":
        //                     options.Proxy = Deserialize<Microsoft.Extensions.Caching.StackExchangeRedis.RedisProxy>(ref reader, options);
        //                     break;
        //                 case "ResolveDns":
        //                     options.ResolveDns = reader.GetBoolean();
        //                     break;
        //                 case "ResponseTimeout":
        //                     options.ResponseTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                     break;
        //                 case "ServiceName":
        //                     options.ServiceName = reader.GetString();
        //                     break;
        //                 case "Ssl":
        //                     options.Ssl = reader.GetBoolean();
        //                     break;
        //                 case "SslHost":
        //                     options.SslHost = reader.GetString();
        //                     break;
        //                 case "SyncTimeout":
        //                     options.SyncTimeout = Deserialize<TimeSpan>(ref reader, options);
        //                     break;
        //                 case "TieBreaker":
        //                     options.TieBreaker = reader.GetString();
        //                     break;
        //                 case "Version":
        //                     options.Version = reader.GetInt32();
        //                     break;
        //                 case "WriteBuffer":
        //                     options.WriteBuffer = reader.GetInt32();
        //                     break;
        //                 default:
        //                     throw new NotImplementedException();
        //             }
        //         }
    }

    public override void Write(Utf8JsonWriter writer, EndPointCollection value, Jso options)
    {
        throw new NotImplementedException();
    }

    public class EndPointSerializerContext(Jso options) : JsonSerializerContext(options)
    {
        protected override Jso GeneratedSerializerOptions => Options;

    public override JsonTypeInfo? GetTypeInfo(type type)
    {
        return type switch
        {
            { Name: nameof(EndPoint) } => CreateJsonTypeInfo(Options),
            _ => throw new NotImplementedException()
        };
    }
}

protected static JsonTypeInfo CreateJsonTypeInfo(Jso jso)
{
    var typeInfo = JsonTypeInfo.CreateJsonTypeInfo<EndPoint>(jso);
    typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(DnsEndPoint)));
    typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(IPEndPoint)));
    return typeInfo;
}
}
