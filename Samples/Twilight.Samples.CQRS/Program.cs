using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Data;
using Twilight.Samples.CQRS;

const string consoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
                                      .Enrich.WithProperty("ApplicationName", DiagnosticsConfig.ServiceName)
                                      .WriteTo.Console(outputTemplate: consoleOutputTemplate)
                                      .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
       .WithTracing(tracerProviderBuilder
            => tracerProviderBuilder.AddSource(DiagnosticsConfig.ActivitySource.Name)
                                    .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
                                    .AddAspNetCoreInstrumentation()
                                    .AddConsoleExporter())
       .WithMetrics(metricsProviderBuilder
            => metricsProviderBuilder.ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
                                     .AddAspNetCoreInstrumentation()
                                     .AddConsoleExporter());

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>(containerBuilder => { containerBuilder.RegisterModule<AutofacModule>(); })
       .UseSerilog((context, services, configuration)
           => configuration.ReadFrom.Configuration(context.Configuration)
                           .ReadFrom.Services(services)
                           .MinimumLevel.Verbose()
                           .Enrich.WithProperty("ApplicationName", DiagnosticsConfig.ServiceName)
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
    Log.Information("Starting {AppName}", DiagnosticsConfig.ServiceName);

    await builder.Build().RunAsync();

    Log.Information("Running {AppName}", DiagnosticsConfig.ServiceName);
}
catch (Exception ex)
{
    Log.Fatal(ex, "{AppName} terminated unexpectedly. Message: {ExceptionMessage}", DiagnosticsConfig.ServiceName, ex.Message);

    Environment.Exit(-1);
}
finally
{
    Log.Information("Stopping {AppName}", DiagnosticsConfig.ServiceName);
    Log.CloseAndFlush();

    Environment.Exit(0);
}