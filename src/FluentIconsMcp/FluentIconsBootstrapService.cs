using FluentIconsMcp.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentIconsMcp;

internal sealed class FluentIconsBootstrapService(FluentIconsDataService iconDataService, IHostApplicationLifetime hostApplicationLifetime, ILogger<FluentIconsBootstrapService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await iconDataService.LoadIconsAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Loaded {IconCount} fluent icons", iconDataService.Icons.Count);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to load fluent icons data");
            hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
