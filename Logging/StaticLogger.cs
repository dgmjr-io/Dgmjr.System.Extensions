namespace Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

using Log = Serilog.Log;
using SeriloggerFactory = Serilog.Extensions.Logging.SerilogLoggerFactory;

public static class StaticLogger
{
    private static readonly IDictionary<string, ILogger> Loggers = new DefaultableDictionary<
        string,
        ILogger
    >(key => Factory.CreateLogger(key));

    private static readonly IDictionary<type, ILogger> LoggersByType = new DefaultableDictionary<
        type,
        ILogger
    >(
        key =>
            (
                Factory.CreateLogger(key.GetDisplayName() ?? key.Name)
                ?? throw new InvalidOperationException("Logger creation failed")
            )!
    );

    private static readonly ILoggerFactory Factory = new SeriloggerFactory(NewSerilogger());

    public static ILogger<T> GetLogger<T>() => new Logger<T>(Factory);

    public static ILogger GetLogger([CallerMemberName] string? name = default) => Loggers[name];

    public static ILogger GetLogger(type type) => LoggersByType[type];

    public static Serilog.ILogger NewSerilogger([CallerMemberName] string type = nameof(Serilog)) =>
        Log.Logger ??= new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

    //     // Copyright 2020 Serilog Contributors
    //     //
    //     // Licensed under the Apache License, Version 2.0 (the "License");
    //     // you may not use this file except in compliance with the License.
    //     // You may obtain a copy of the License at
    //     //
    //     //     http://www.apache.org/licenses/LICENSE-2.0
    //     //
    //     // Unless required by applicable law or agreed to in writing, software
    //     // distributed under the License is distributed on an "AS IS" BASIS,
    //     // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    //     // See the License for the specific language governing permissions and
    //     // limitations under the License.

    // #if !NO_RELOADABLE_LOGGER

    //     // ReSharper disable MemberCanBePrivate.Global

    //     internal interface IReloadableLogger
    //     {
    //        Serilog.ILogger ReloadLogger();
    //     }

    //     /// <summary>
    //     /// A Serilog <see cref=Serilog.ILogger"/> that can be reconfigured without invalidating existing <see cref=Serilog.ILogger"/>
    //     /// instances derived from it.
    //     /// </summary>
    //     public sealed class ReloadableLogger : Serilog.ILogger, IReloadableLogger, IDisposable
    //     {
    //         private readonly object _sync = new object();
    //         private Serilog.Core.Logger _logger;

    //         // One-way; if the value is `true` it can never again be made `false`, allowing "double-checked" reads. If
    //         // `true`, `_logger` is final and a memory barrier ensures the final value is seen by all threads.
    //         private bool _frozen;

    //         // Unsure whether this should be exposed; currently going for minimal API surface.
    //         internal ReloadableLogger(Logger initial)
    //         {
    //             _logger = initial ?? throw new ArgumentNullException(nameof(initial));
    //         }

    //         Serilog.ILogger IReloadableLogger.ReloadLogger()
    //         {
    //             return _logger;
    //         }

    //         /// <summary>
    //         /// Reload the logger using the supplied configuration delegate.
    //         /// </summary>
    //         /// <param name="configure">A callback in which the logger is reconfigured.</param>
    //         /// <exception cref="ArgumentNullException"><paramref name="configure"/> is null.</exception>
    //         public void Reload(Func<LoggerConfiguration, LoggerConfiguration> configure)
    //         {
    //             if (configure == null)
    //                 throw new ArgumentNullException(nameof(configure));

    //             lock (_sync)
    //             {
    //                 _logger.Dispose();
    //                 _logger = configure(new LoggerConfiguration()).CreateLogger();
    //             }
    //         }

    //         /// <summary>
    //         /// Freeze the logger, so that no further reconfiguration is possible. Once the logger is frozen, logging through
    //         /// new contextual loggers will have no additional cost, and logging directly through this logger will not require
    //         /// any synchronization.
    //         /// </summary>
    //         /// <returns>The <see cref="Logger"/> configured with the final settings.</returns>
    //         /// <exception cref="InvalidOperationException">The logger is already frozen.</exception>
    //         public Logger Freeze()
    //         {
    //             lock (_sync)
    //             {
    //                 if (_frozen)
    //                     throw new InvalidOperationException("The logger is already frozen.");

    //                 _frozen = true;

    //                 // https://github.com/dotnet/runtime/issues/20500#issuecomment-284774431
    //                 // Publish `_logger` and `_frozen`. This is useful here because it means that once the logger is frozen - which
    //                 // we always expect - reads don't require any synchronization/interlocked instructions.
    //                 Interlocked.MemoryBarrierProcessWide();

    //                 return _logger;
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Dispose()
    //         {
    //             lock (_sync)
    //                 _logger.Dispose();
    //         }

    //         /// <inheritdoc />
    //         public Serilog.ILogger ForContext(ILogEventEnricher enricher)
    //         {
    //             if (enricher == null)
    //                 return this;

    //             if (_frozen)
    //                 return _logger.ForContext(enricher);

    //             lock (_sync)
    //                 return new CachingReloadableLogger(
    //                     this,
    //                     _logger,
    //                     this,
    //                     p => p.ForContext(enricher)
    //                 );
    //         }

    //         /// <inheritdoc />
    //         public Serilog.ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
    //         {
    //             if (enrichers == null)
    //                 return this;

    //             if (_frozen)
    //                 return _logger.ForContext(enrichers);

    //             lock (_sync)
    //                 return new CachingReloadableLogger(
    //                     this,
    //                     _logger,
    //                     this,
    //                     p => p.ForContext(enrichers)
    //                 );
    //         }

    //         /// <inheritdoc />
    //         public Serilog.ILogger ForContext(
    //             string propertyName,
    //             object value,
    //             bool destructureObjects = false
    //         )
    //         {
    //             if (propertyName == null)
    //                 return this;

    //             if (_frozen)
    //                 return _logger.ForContext(propertyName, value, destructureObjects);

    //             lock (_sync)
    //                 return new CachingReloadableLogger(
    //                     this,
    //                     _logger,
    //                     this,
    //                     p => p.ForContext(propertyName, value, destructureObjects)
    //                 );
    //         }

    //         /// <inheritdoc />
    //         public Serilog.ILogger ForContext<TSource>()
    //         {
    //             if (_frozen)
    //                 return _logger.ForContext<TSource>();

    //             lock (_sync)
    //                 return new CachingReloadableLogger(
    //                     this,
    //                     _logger,
    //                     this,
    //                     p => p.ForContext<TSource>()
    //                 );
    //         }

    //         /// <inheritdoc />
    //         public Serilog.ILogger ForContext(Type source)
    //         {
    //             if (source == null)
    //                 return this;

    //             if (_frozen)
    //                 return _logger.ForContext(source);

    //             lock (_sync)
    //                 return new CachingReloadableLogger(this, _logger, this, p => p.ForContext(source));
    //         }

    //         /// <inheritdoc />
    //         public void Write(LogEvent logEvent)
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(logEvent);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(logEvent);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write(LogEventLevel level, string messageTemplate)
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, messageTemplate);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, messageTemplate);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValue);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValue);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T0, T1>(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T0, T1, T2>(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             params object[] propertyValues
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValues);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, messageTemplate, propertyValues);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write(LogEventLevel level, Exception exception, string messageTemplate)
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, exception, messageTemplate);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, exception, messageTemplate);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T propertyValue
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValue);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValue);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T0, T1>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write<T0, T1, T2>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //             }
    //         }

    //         /// <inheritdoc />
    //         public void Write(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             params object[] propertyValues
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValues);
    //                 return;
    //             }

    //             lock (_sync)
    //             {
    //                 _logger.Write(level, exception, messageTemplate, propertyValues);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public bool IsEnabled(LogEventLevel level)
    //         {
    //             if (_frozen)
    //             {
    //                 return _logger.IsEnabled(level);
    //             }

    //             lock (_sync)
    //             {
    //                 return _logger.IsEnabled(level);
    //             }
    //         }

    //         /// <inheritdoc />
    //         public bool BindMessageTemplate(
    //             string messageTemplate,
    //             object[] propertyValues,
    //             out MessageTemplate parsedTemplate,
    //             out IEnumerable<LogEventProperty> boundProperties
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 return _logger.BindMessageTemplate(
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties
    //                 );
    //             }

    //             lock (_sync)
    //             {
    //                 return _logger.BindMessageTemplate(
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties
    //                 );
    //             }
    //         }

    //         /// <inheritdoc />
    //         public bool BindProperty(
    //             string propertyName,
    //             object value,
    //             bool destructureObjects,
    //             out LogEventProperty property
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 return _logger.BindProperty(propertyName, value, destructureObjects, out property);
    //             }

    //             lock (_sync)
    //             {
    //                 return _logger.BindProperty(propertyName, value, destructureObjects, out property);
    //             }
    //         }

    //         [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //         (Serilog.ILogger, bool) UpdateForCaller(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             // Synchronization on `_sync` is not required in this method; it will be called without a lock
    //             // if `_frozen` and under a lock if not.

    //             if (_frozen)
    //             {
    //                 // If we're frozen, then the caller hasn't observed this yet and should update. We could optimize a little here
    //                 // and only signal an update if the cached logger is stale (as per the next condition below).
    //                 newRoot = _logger;
    //                 newCached = caller.ReloadLogger();
    //                 frozen = true;
    //                 return (newCached, true);
    //             }

    //             if (cached != null && root == _logger)
    //             {
    //                 newRoot = default;
    //                 newCached = default;
    //                 frozen = false;
    //                 return (cached, false);
    //             }

    //             newRoot = _logger;
    //             newCached = caller.ReloadLogger();
    //             frozen = false;
    //             return (newCached, true);
    //         }

    //         internal bool InvokeIsEnabled(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             out bool isEnabled,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 isEnabled = logger.IsEnabled(level);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 isEnabled = logger.IsEnabled(level);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeBindMessageTemplate(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             string messageTemplate,
    //             object[] propertyValues,
    //             out MessageTemplate parsedTemplate,
    //             out IEnumerable<LogEventProperty> boundProperties,
    //             out bool canBind,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 canBind = logger.BindMessageTemplate(
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties
    //                 );
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 canBind = logger.BindMessageTemplate(
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties
    //                 );
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeBindProperty(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             string propertyName,
    //             object propertyValue,
    //             bool destructureObjects,
    //             out LogEventProperty property,
    //             out bool canBind,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 canBind = logger.BindProperty(
    //                     propertyName,
    //                     propertyValue,
    //                     destructureObjects,
    //                     out property
    //                 );
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 canBind = logger.BindProperty(
    //                     propertyName,
    //                     propertyValue,
    //                     destructureObjects,
    //                     out property
    //                 );
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite(
    //             Serilog.ILogger root,
    //             Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEvent logEvent,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(logEvent);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(logEvent);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             string messageTemplate,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T propertyValue,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValue);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValue);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T0, T1>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T0, T1, T2>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             string messageTemplate,
    //             object[] propertyValues,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValues);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, messageTemplate, propertyValues);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T propertyValue,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValue);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValue);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T0, T1>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite<T0, T1, T2>(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return update;
    //             }
    //         }

    //         internal bool InvokeWrite(
    //            Serilog.ILogger root,
    //            Serilog.ILogger cached,
    //             IReloadableLogger caller,
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             object[] propertyValues,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValues);
    //                 return update;
    //             }

    //             lock (_sync)
    //             {
    //                 var (logger, update) = UpdateForCaller(
    //                     root,
    //                     cached,
    //                     caller,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 logger.Write(level, exception, messageTemplate, propertyValues);
    //                 return update;
    //             }
    //         }

    //         internal bool CreateChild(
    //            Serilog.ILogger root,
    //             IReloadableLogger parent,
    //            Serilog.ILogger cachedParent,
    //             Func<ILogger,Serilog.ILogger> configureChild,
    //             out Serilog.ILogger child,
    //             out Serilog.ILogger newRoot,
    //             out Serilog.ILogger newCached,
    //             out bool frozen
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 var (logger, _) = UpdateForCaller(
    //                     root,
    //                     cachedParent,
    //                     parent,
    //                     out newRoot,
    //                     out newCached,
    //                     out frozen
    //                 );
    //                 child = configureChild(logger);
    //                 return true; // Always an update, since the caller has not observed that the reloadable logger is frozen.
    //             }

    //             // No synchronization, here - a lot of loggers are created and thrown away again without ever being used,
    //             // so we just return a lazy wrapper.
    //             child = new CachingReloadableLogger(this, root, parent, configureChild);
    //             newRoot = default;
    //             newCached = default;
    //             frozen = default;
    //             return false;
    //         }
    //     }

    //     public class CachingReloadableLogger : Serilog.ILogger, IReloadableLogger
    //     {
    //         private readonly ReloadableLogger _reloadableLogger;
    //         private readonly Func<Serilog.ILogger, Serilog.ILogger> _configure;
    //         private readonly IReloadableLogger _parent;

    //         private Serilog.ILogger _root,
    //             _cached;
    //         private bool _frozen;

    //         public CachingReloadableLogger(
    //             ReloadableLogger reloadableLogger,
    //            Serilog.ILogger root,
    //             IReloadableLogger parent,
    //             Func<ILogger,Serilog.ILogger> configure
    //         )
    //         {
    //             _reloadableLogger = reloadableLogger;
    //             _parent = parent;
    //             _configure = configure;
    //             _root = root;
    //             _cached = null;
    //             _frozen = false;
    //         }

    //         public Serilog.ILogger ReloadLogger()
    //         {
    //             return _configure(_parent.ReloadLogger());
    //         }

    //         public Serilog.ILogger ForContext(ILogEventEnricher enricher)
    //         {
    //             if (enricher == null)
    //                 return this;

    //             if (_frozen)
    //                 return _cached.ForContext(enricher);

    //             if (
    //                 _reloadableLogger.CreateChild(
    //                     _root,
    //                     this,
    //                     _cached,
    //                     p => p.ForContext(enricher),
    //                     out var child,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return child;
    //         }

    //         public Serilog.ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
    //         {
    //             if (enrichers == null)
    //                 return this;

    //             if (_frozen)
    //                 return _cached.ForContext(enrichers);

    //             if (
    //                 _reloadableLogger.CreateChild(
    //                     _root,
    //                     this,
    //                     _cached,
    //                     p => p.ForContext(enrichers),
    //                     out var child,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return child;
    //         }

    //         public Serilog.ILogger ForContext(
    //             string propertyName,
    //             object value,
    //             bool destructureObjects = false
    //         )
    //         {
    //             if (propertyName == null)
    //                 return this;

    //             if (_frozen)
    //                 return _cached.ForContext(propertyName, value, destructureObjects);

    //            Serilog.ILogger child;
    //             if (
    //                 value == null
    //                 || value is string
    //                 || value.GetType().IsPrimitive
    //                 || value.GetType().IsEnum
    //             )
    //             {
    //                 // Safe to extend the lifetime of `value` by closing over it.
    //                 // This ensures `SourceContext` is passed through appropriately and triggers minimum level overrides.
    //                 if (
    //                     _reloadableLogger.CreateChild(
    //                         _root,
    //                         this,
    //                         _cached,
    //                         p => p.ForContext(propertyName, value, destructureObjects),
    //                         out child,
    //                         out var newRoot,
    //                         out var newCached,
    //                         out var frozen
    //                     )
    //                 )
    //                 {
    //                     Update(newRoot, newCached, frozen);
    //                 }
    //             }
    //             else
    //             {
    //                 // It's not safe to extend the lifetime of `value` or pass it unexpectedly between threads.
    //                 // Changes to destructuring configuration won't be picked up by the cached logger.
    //                 var eager = ReloadLogger();
    //                 if (!eager.BindProperty(propertyName, value, destructureObjects, out var property))
    //                     return this;

    //                 var enricher = new FixedPropertyEnricher(property);

    //                 if (
    //                     _reloadableLogger.CreateChild(
    //                         _root,
    //                         this,
    //                         _cached,
    //                         p => p.ForContext(enricher),
    //                         out child,
    //                         out var newRoot,
    //                         out var newCached,
    //                         out var frozen
    //                     )
    //                 )
    //                 {
    //                     Update(newRoot, newCached, frozen);
    //                 }
    //             }

    //             return child;
    //         }

    //         public Serilog.ILogger ForContext<TSource>()
    //         {
    //             if (_frozen)
    //                 return _cached.ForContext<TSource>();

    //             if (
    //                 _reloadableLogger.CreateChild(
    //                     _root,
    //                     this,
    //                     _cached,
    //                     p => p.ForContext<TSource>(),
    //                     out var child,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return child;
    //         }

    //         public Serilog.ILogger ForContext(Type source)
    //         {
    //             if (_frozen)
    //                 return _cached.ForContext(source);

    //             if (
    //                 _reloadableLogger.CreateChild(
    //                     _root,
    //                     this,
    //                     _cached,
    //                     p => p.ForContext(source),
    //                     out var child,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return child;
    //         }

    //         void UpdateSerilog.ILogger newRoot,Serilog.ILogger newCached, bool frozen)
    //         {
    //             _root = newRoot;
    //             _cached = newCached;

    //             // https://github.com/dotnet/runtime/issues/20500#issuecomment-284774431
    //             // Publish `_cached` and then `_frozen`. This is useful here because it means that once the logger is frozen - which
    //             // we always expect - reads don't require any synchronization/interlocked instructions.
    //             Interlocked.MemoryBarrierProcessWide();

    //             _frozen = frozen;

    //             Interlocked.MemoryBarrierProcessWide();
    //         }

    //         public void Write(LogEvent logEvent)
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(logEvent);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     logEvent,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write(LogEventLevel level, string messageTemplate)
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, messageTemplate);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     messageTemplate,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, messageTemplate, propertyValue);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     messageTemplate,
    //                     propertyValue,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T0, T1>(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, messageTemplate, propertyValue0, propertyValue1);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T0, T1, T2>(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write(
    //             LogEventLevel level,
    //             string messageTemplate,
    //             params object[] propertyValues
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, messageTemplate, propertyValues);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     messageTemplate,
    //                     propertyValues,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write(LogEventLevel level, Exception exception, string messageTemplate)
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, exception, messageTemplate);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T propertyValue
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, exception, messageTemplate, propertyValue);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T0, T1>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write<T0, T1, T2>(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             T0 propertyValue0,
    //             T1 propertyValue1,
    //             T2 propertyValue2
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2
    //                 );
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValue0,
    //                     propertyValue1,
    //                     propertyValue2,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public void Write(
    //             LogEventLevel level,
    //             Exception exception,
    //             string messageTemplate,
    //             params object[] propertyValues
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 _cached.Write(level, exception, messageTemplate, propertyValues);
    //                 return;
    //             }

    //             if (
    //                 _reloadableLogger.InvokeWrite(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     exception,
    //                     messageTemplate,
    //                     propertyValues,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }
    //         }

    //         public bool IsEnabled(LogEventLevel level)
    //         {
    //             if (_frozen)
    //             {
    //                 return _cached.IsEnabled(level);
    //             }

    //             if (
    //                 _reloadableLogger.InvokeIsEnabled(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     level,
    //                     out var isEnabled,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return isEnabled;
    //         }

    //         public bool BindMessageTemplate(
    //             string messageTemplate,
    //             object[] propertyValues,
    //             out MessageTemplate parsedTemplate,
    //             out IEnumerable<LogEventProperty> boundProperties
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 return _cached.BindMessageTemplate(
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties
    //                 );
    //             }

    //             if (
    //                 _reloadableLogger.InvokeBindMessageTemplate(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     messageTemplate,
    //                     propertyValues,
    //                     out parsedTemplate,
    //                     out boundProperties,
    //                     out var canBind,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return canBind;
    //         }

    //         public bool BindProperty(
    //             string propertyName,
    //             object value,
    //             bool destructureObjects,
    //             out LogEventProperty property
    //         )
    //         {
    //             if (_frozen)
    //             {
    //                 return _cached.BindProperty(propertyName, value, destructureObjects, out property);
    //             }

    //             if (
    //                 _reloadableLogger.InvokeBindProperty(
    //                     _root,
    //                     _cached,
    //                     this,
    //                     propertyName,
    //                     value,
    //                     destructureObjects,
    //                     out property,
    //                     out var canBind,
    //                     out var newRoot,
    //                     out var newCached,
    //                     out var frozen
    //                 )
    //             )
    //             {
    //                 Update(newRoot, newCached, frozen);
    //             }

    //             return canBind;
    //         }
    //     }

    //     class FixedPropertyEnricher : ILogEventEnricher
    //     {
    //         readonly LogEventProperty _property;

    //         public FixedPropertyEnricher(LogEventProperty property)
    //         {
    //             _property = property;
    //         }

    //         public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    //         {
    //             logEvent.AddPropertyIfAbsent(_property);
    //         }
    //     }

    // #endif
}
