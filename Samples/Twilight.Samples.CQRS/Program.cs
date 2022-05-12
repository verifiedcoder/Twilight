using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Data;
using Twilight.Samples.CQRS;

const string consoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
const string appName = "Twilight.Samples.CQRS";

var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: consoleOutputTemplate)
                                      .MinimumLevel.Verbose()
                                      .Enrich.WithProperty("ApplicationName", appName)
                                      .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder
     => tracerProviderBuilder.AddSqlClientInstrumentation(options =>
                             {
                                 options.EnableConnectionLevelAttributes = true;
                                 options.SetDbStatementForStoredProcedure = true;
                                 options.SetDbStatementForText = true;
                                 options.RecordException = true;
                                 options.Enrich = (activity, x, y) => activity.SetTag("db.type", "sql");
                             })
                             .AddSource("Twilight.CQRS", appName, "Twilight.CQRS.Messaging.InMemory.Autofac")
                             .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("twilight-samples-cqrs")
                                                                .AddAttributes(new[] { new KeyValuePair<string, object>("service.version", serviceVersion) }))
                             .SetSampler(new AlwaysOnSampler())
                             .AddConsoleExporter());

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>(containerBuilder => { containerBuilder.RegisterModule<AutofacModule>(); })
       .UseSerilog((context, services, configuration)
           => configuration.ReadFrom.Configuration(context.Configuration)
                           .ReadFrom.Services(services)
                           .MinimumLevel.Verbose()
                           .Enrich.WithProperty("ApplicationName", appName)
                           .WriteTo.Console(outputTemplate: consoleOutputTemplate))
       .ConfigureServices(services =>
       {
           services.AddDbContext<SampleDataContext>(dbContextOptions =>
           {
               dbContextOptions.UseInMemoryDatabase("DataDb");
               dbContextOptions.EnableSensitiveDataLogging();
           });
           services.AddDbContext<ViewDataContext>(dbContextOptions =>
           {
               dbContextOptions.UseInMemoryDatabase("ViewDb");
               dbContextOptions.EnableSensitiveDataLogging();
           });
           services.AddHostedService<AppHost>();
       });

try
{
    Log.Information("Starting {AppName}", appName);

    await builder.Build().RunAsync();

    Log.Information("Running {AppName}", appName);
}
catch (Exception ex)
{
    Log.Fatal(ex, "{AppName} terminated unexpectedly. CqrsMessage: {ExceptionMessage}", appName, ex.Message);

    Environment.Exit(-1);
}
finally
{
    Log.Information("Stopping {AppName}", appName);
    Log.CloseAndFlush();

    Environment.Exit(0);
}
