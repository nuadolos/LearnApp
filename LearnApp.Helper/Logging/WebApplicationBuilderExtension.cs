using Microsoft.AspNetCore.Builder;
using Serilog;
using Elastic.CommonSchema;
using Elastic.CommonSchema.Serilog;
using Serilog.Events;
using System.Reflection;
using System.Diagnostics;

namespace LearnApp.Helper.Logging
{
    static public class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddLoggingProvider(this WebApplicationBuilder builder)
        {
            string serviceName = Assembly.GetEntryAssembly()!.GetName().Name!;

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.FromLogContext();

            if (Debugger.IsAttached)
                loggerConfig.WriteTo.Console(LogEventLevel.Debug);

            loggerConfig.WriteTo.File(new EcsTextFormatter(),
                Path.Combine("logs", $"{serviceName}.json"),
                restrictedToMinimumLevel: LogEventLevel.Warning,
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                rollingInterval: RollingInterval.Day,
                shared: true,
                retainedFileCountLimit: null,
                flushToDiskInterval: TimeSpan.FromSeconds(1));

            var logger = loggerConfig.CreateLogger();
            Serilog.Log.Logger = logger;
            builder.Logging.AddSerilog(logger);
            builder.Host.UseSerilog();

            return builder;
        }

        public static WebApplication UseLoggingMiddleware(this WebApplication app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "RequestPath: {RequestPath}";

                options.GetLevel = (_, _, ex) =>
                    ex == null ? LogEventLevel.Debug : LogEventLevel.Error;

                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            return app;
        }
    }
}
