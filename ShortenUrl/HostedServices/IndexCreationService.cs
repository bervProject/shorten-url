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
        try
        {
            _provider.Connection.GetIndexInfo(typeof(Urls));

            if (!_provider.Connection.IsIndexCurrent(typeof(Urls)))
            {
                await _provider.Connection.DropIndexAsync(typeof(Urls));
                await _provider.Connection.CreateIndexAsync(typeof(Urls));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating or updating index for Urls");
            await _provider.Connection.CreateIndexAsync(typeof(Urls));
        }


    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}