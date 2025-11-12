using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Twilight.Samples.Common;

public sealed class AppHost(IRunner runner, ILogger<AppHost> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Started {AppHost}.", nameof(AppHost));
        }

        await runner.Run();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Stopped {AppHost}.", nameof(AppHost));
        }

        await Task.CompletedTask;
    }
}
