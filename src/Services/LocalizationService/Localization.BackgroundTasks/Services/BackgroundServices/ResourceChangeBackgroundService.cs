using Confluent.Kafka;
using Localization.BackgroundTasks.Models;
using Localization.BackgroundTasks.Models.Cdc;
using Localization.BackgroundTasks.Models.Settings;
using Localization.BackgroundTasks.Services.Cache.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Localization.BackgroundTasks.Services.BackgroundServices;

public class ResourceChangeBackgroundService : BackgroundService
{
    private readonly IRedisService _redisService;
    private readonly ILogger<ResourceChangeBackgroundService> _logger;

    private readonly QueueSettings _queueSettings;
    private readonly CacheSettings _redisSettings;

    public ResourceChangeBackgroundService(
        IRedisService redisService,
        ILogger<ResourceChangeBackgroundService> logger,
        IOptions<QueueSettings> queueSettings,
        IOptions<CacheSettings> redisSettings)
    {
        _redisService = redisService;
        _logger = logger;

        _queueSettings = queueSettings.Value;
        _redisSettings = redisSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var configurationa = new ConsumerConfig()
            {
                GroupId = _queueSettings.Topic,
                BootstrapServers = _queueSettings.Server,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(configurationa).Build();
            consumer.Subscribe(_queueSettings.Topic);

            while (true)
            {
                try
                {
                    ConsumeResult<Ignore, string> result = consumer.Consume();

                    var value = JsonConvert.DeserializeObject<CdcBase<Resource>>(result.Message.Value);

                    if (value == null)
                        continue;

                    var resourceAfter = value.Payload.After;
                    var resourceBefore = value.Payload.Before;

                    if (resourceBefore == null)
                    {
                        _ = await _redisService.SetAsync($"{_redisSettings.Prefix}-{resourceAfter.LanguageCode}-{resourceAfter.Tag}",
                                                         resourceAfter,
                                                         _redisSettings.Duration,
                                                         _redisSettings.DatabaseId);
                    }
                    else
                    {
                        _ = await _redisService.RemoveAsync($"{_redisSettings.Prefix}-{resourceBefore.LanguageCode}-{resourceBefore.Tag}", 
                                                        _redisSettings.DatabaseId);

                        _ = await _redisService.SetAsync($"{_redisSettings.Prefix}-{resourceAfter.LanguageCode}-{resourceAfter.Tag}",
                                                     resourceAfter,
                                                     _redisSettings.Duration,
                                                     _redisSettings.DatabaseId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("LOOP Problem occured while executing resource change background service, " +
                                     "Message : {Message}", ex.Message);
                }
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError("Problem occured while executing resource change background service, " +
                             "Message : {Message}", ex.Message);
        }
    }
}
