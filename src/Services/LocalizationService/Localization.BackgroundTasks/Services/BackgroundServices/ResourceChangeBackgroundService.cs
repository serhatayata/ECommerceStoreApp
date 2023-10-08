﻿using Confluent.Kafka;
using Localization.BackgroundTasks.Models;
using Localization.BackgroundTasks.Models.Cdc;
using Localization.BackgroundTasks.Models.Settings;
using Localization.BackgroundTasks.Services.Cache.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Localization.BackgroundTasks.Services.BackgroundServices
{
    public class ResourceChangeBackgroundService : BackgroundService
    {
        private readonly IRedisService _redisService;

        private readonly QueueSettings _queueSettings;
        private readonly RedisSettings _redisSettings;

        public ResourceChangeBackgroundService(
            IRedisService redisService,
            IOptions<QueueSettings> queueSettings,
            IOptions<RedisSettings> redisSettings)
        {
            _redisService = redisService;

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
                            await _redisService.SetAsync($"{_redisSettings.Prefix}-{resourceAfter.LanguageCode}-{resourceAfter.Tag}",
                                                         resourceAfter,
                                                         _redisSettings.Duration,
                                                         _redisSettings.DatabaseId);
                        }
                        else
                        {
                            await _redisService.RemoveAsync($"{_redisSettings.Prefix}-{resourceBefore.LanguageCode}-{resourceBefore.Tag}", 
                                                            _redisSettings.DatabaseId);

                            await _redisService.SetAsync($"{_redisSettings.Prefix}-{resourceAfter.LanguageCode}-{resourceAfter.Tag}",
                                                         resourceAfter,
                                                         _redisSettings.Duration,
                                                         _redisSettings.DatabaseId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
