using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Twilight.Samples.Common;

public sealed class AppHost : IHostedService
{
    private readonly ILogger<AppHost> _logger;
    private readonly IRunner _runner;

    public AppHost(IRunner runner, ILogger<AppHost> logger)
    {
        _runner = runner;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started {AppHost}.", nameof(AppHost));

        await _runner.Run();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopped {AppHost}.", nameof(AppHost));

        await Task.CompletedTask;
    }
}
