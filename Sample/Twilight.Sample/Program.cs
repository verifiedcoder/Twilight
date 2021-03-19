using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Twilight.Sample.Data;

namespace Twilight.Sample
{
    public static class Program
    {
        static Program()
        {
            // Configure Open Telemetry
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                                                  .MinimumLevel.Verbose()
                                                  .CreateLogger();

            Log.Information("Starting {AppName}...", AppName);
        }

        private static string AppName => "Twilight.Sample";

        public static async Task Main()
        {
            try
            {
                // Add Open Telemetry
                using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                                             .AddSource("Twilight.CQRS", "Twilight.CQRS.Messaging.InMemory.Autofac")
                                             .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("twilight-sample"))
                                             .AddConsoleExporter()
                                             .Build();

                await CreateHostBuilder().RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{AppName} terminated unexpectedly. Message: {ExceptionMessage}", AppName, ex.Message);

                Environment.Exit(-1);
            }
            finally
            {
                Log.Information("Stopped {AppName}...", AppName);
                Log.CloseAndFlush();

                Environment.Exit(0);
            }
        }

        private static IHostBuilder CreateHostBuilder() => new HostBuilder().UseServiceProviderFactory(new AutofacServiceProviderFactory())
                                                                            .ConfigureServices(services =>
                                                                                               {
                                                                                                   services.AddDbContext<UsersContext>(o =>
                                                                                                                                       {
                                                                                                                                           o.UseInMemoryDatabase("UsersView");
                                                                                                                                           o.EnableSensitiveDataLogging();
                                                                                                                                       });
                                                                                                   services.AddDbContext<UsersViewContext>(o =>
                                                                                                                                           {
                                                                                                                                               o.UseInMemoryDatabase("UsersView");
                                                                                                                                               o.EnableSensitiveDataLogging();
                                                                                                                                           });
                                                                                                   services.AddHostedService<AppHost>();
                                                                                               })
                                                                            .ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule<AutofacModule>(); })
                                                                            .ConfigureLogging(logging => { logging.AddSerilog(); });
    }
}
