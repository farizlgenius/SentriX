using System;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Serilog.Events;

namespace Host.Logging;

public static class LoggingSetup
{
      public static IHostBuilder ConfigureEnterpriseLogging(this IHostBuilder host)
      {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)

                // ⭐ GLOBAL PROPERTIES
                .Enrich.WithProperty("Application", "DeviceManager")
                .Enrich.WithProperty("Environment", env)

                // ⭐ ENVIRONMENT INFO
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()

                // ⭐ REQUEST TRACING
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()

                // ⭐ CLASS + METHOD
                .Enrich.WithCallerInfo(includeFileInfo: true, assemblyPrefix: "SentriX")

                // ⭐ FILE SINK (main investigation source)
                .WriteTo.File(
                    "logs/app-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 60,           // keep 60 days
                    fileSizeLimitBytes: 50_000_000,       // 50MB per file
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " +
                    "[{Level:u3}] " +
                    "[{Environment}] " +
                    "[{MachineName}] " +
                    "[{CorrelationId}] " +
                    "[{SourceContext}.{Method}] " +
                    "{Message:lj} {Properties:j}{NewLine}{Exception}")

                // ⭐ DEBUG WINDOW
                .WriteTo.Debug()

                .CreateLogger();

            host.UseSerilog();

            return host;
      }
}
