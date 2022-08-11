namespace ShortenUrl.HostedServices;


using Microsoft.Extensions.Logging;

using Redis.OM;

using ShortenUrl.Models;

public class IndexCreationService : IHostedService
{
    private readonly RedisConnectionProvider _provider;
    private readonly ILogger<IndexCreationService> _logger;

    public IndexCreationService(RedisConnectionProvider provider, ILogger<IndexCreationService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create Index {}", typeof(Urls));
        var result = await _provider.Connection.CreateIndexAsync(typeof(Urls));
        _logger.LogDebug("Create Index {} Result: {}", typeof(Urls), result);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}