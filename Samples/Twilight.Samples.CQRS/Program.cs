using System.Diagnostics;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Data;

namespace Twilight.Samples.CQRS;

public static class Program
{
    private const string ConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
    private const string AppName = "Twilight.Samples.CQRS";

    static Program()
    {
        // Configure Open Telemetry
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
    }

    public static async Task Main()
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: ConsoleOutputTemplate)
                                              .MinimumLevel.Verbose()
                                              .Enrich.WithProperty("ApplicationName", AppName)
                                              .CreateBootstrapLogger();
        // Add Open Telemetry
        using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                                     .AddSource("Twilight.CQRS", "Twilight.Samples.CQRS", "Twilight.CQRS.Messaging.InMemory.Autofac")
                                     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("twilight-samples-cqrs"))
                                     .AddConsoleExporter()
                                     .Build();

        Log.Information("Running {AppName}", AppName);

        try
        {
            await CreateHostBuilder().RunConsoleAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{AppName} terminated unexpectedly. CqrsMessage: {ExceptionMessage}", AppName, ex.Message);

            Environment.Exit(-1);
        }
        finally
        {
            Log.Information("Stopped {AppName}", AppName);
            Log.CloseAndFlush();

            Environment.Exit(0);
        }
    }

    private static IHostBuilder CreateHostBuilder()
        => new HostBuilder().UseServiceProviderFactory(new AutofacServiceProviderFactory())
                            .UseSerilog((context, services, configuration)
                                            => configuration.ReadFrom.Configuration(context.Configuration)
                                                            .ReadFrom.Services(services)
                                                            .WriteTo.Console(outputTemplate: ConsoleOutputTemplate))
                            .ConfigureServices(services =>
                            {
                                services.AddDbContext<SampleDataContext>(builder =>
                                {
                                    builder.UseInMemoryDatabase("DataDb");
                                    builder.EnableSensitiveDataLogging();
                                });
                                services.AddDbContext<ViewDataContext>(builder =>
                                {
                                    builder.UseInMemoryDatabase("ViewDb");
                                    builder.EnableSensitiveDataLogging();
                                });
                                services.AddHostedService<AppHost>();
                            })
                            .ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule<AutofacModule>(); });
}
