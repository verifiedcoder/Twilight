using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Twilight.Sample.Data;

namespace Twilight.Sample
{
    public static class Program
    {
        static Program()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                                                  .MinimumLevel.Verbose()
                                                  .CreateLogger();

            Log.Information("Starting {AppName}...", AppName);
        }

        private static string AppName => "Twilight.Sample";

        public static async Task<int> Main()
        {
            try
            {
                await CreateHostBuilder().RunConsoleAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{AppName} terminated unexpectedly. Message: {ExceptionMessage}", AppName, ex.Message);

                return 1;
            }
            finally
            {
                Log.Information("Stopped {AppName}...", AppName);
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder()
            => new HostBuilder().UseServiceProviderFactory(new AutofacServiceProviderFactory())
                                .ConfigureServices((hostContext, services) =>
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
                                .ConfigureLogging((hostContext, logging) => { logging.AddSerilog(); });
    }
}
