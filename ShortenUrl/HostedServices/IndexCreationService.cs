﻿namespace ShortenUrl.HostedServices;


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
        if (!_provider.Connection.IsIndexCurrent(typeof(Urls)))
{
        _logger.LogDebug("Creating Index {}", typeof(Urls));
        var resultDrop = await _provider.Connection.DropIndexAsync(typeof(Urls));
        _logger.LogDebug("Drop Index {} Result: {}", typeof(Urls), resultDrop);
        var resultCreate = await _provider.Connection.CreateIndexAsync(typeof(Urls));
        _logger.LogDebug("Create Index {} Result: {}", typeof(Urls), resultCreate);
}
        

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}