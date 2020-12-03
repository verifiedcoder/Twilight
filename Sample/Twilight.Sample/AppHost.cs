using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Twilight.Sample
{
    internal sealed class AppHost : IHostedService
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
            _logger.LogInformation("Started app host.");

            await _runner.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopped app host.");

            return Task.CompletedTask;
        }
    }
}
