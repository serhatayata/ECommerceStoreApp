﻿using Autofac.Core;
using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.Settings;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

namespace IdentityServer.Api.Services.Base.Concrete
{
    public class InitializeCacheService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public InitializeCacheService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

            var policy = Polly.Policy.Handle<Exception>()
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                ex.Message, nameof(InitializeCacheService),
                                                MethodBase.GetCurrentMethod()?.Name);
                        });

            await policy.ExecuteAsync(async () =>
            {
                var values = new Dictionary<string, RedisValue>();

                var localizationSettings = _configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();
                var redisSettings = _configuration.GetSection("RedisSettings").Get<RedisSettings>();

                var localizationMemberKey = localizationSettings.MemberKey;
                var redisCacheDuration = localizationSettings.CacheDuration;

                var localizationSuffix1 = localizationSettings.MemoryCache.Suffix1;
                var localizationSuffix2 = localizationSettings.MemoryCache.Suffix2;

                var memoryCache1Prefix = $"{localizationMemberKey}-{localizationSuffix1}";
                var memoryCache2Prefix = $"{localizationMemberKey}-{localizationSuffix2}";

                int databaseId = redisSettings.LocalizationCacheDbId;

                if (!redisService.AnyKeyExistsByPrefix(localizationMemberKey, databaseId) ||
                     !memoryCache.TryGetValue(memoryCache1Prefix, out object cacheDummy1) ||
                     !memoryCache.TryGetValue(memoryCache2Prefix, out object cacheDummy2))
                {
                    var gatewayClient = httpClientFactory.CreateClient("gateway-specific");
                    var result = await gatewayClient.PostGetResponseAsync<DataResult<MemberDto>, StringModel>("localization/members/get-with-resources-by-memberkey-and-save", new StringModel() { Value = localizationMemberKey });

                    if (!result.Success)
                        throw new Exception("Localization data request not successful");

                    var resultData = result.Data;
                    if (resultData == null)
                        throw new Exception("Localization data is null from http request");

                    MemoryCacheExtensions.SaveLocalizationData(memoryCache, _configuration, resultData);

                    if (redisService.AnyKeyExistsByPrefix(localizationMemberKey, databaseId))
                        return;

                    foreach (var resource in resultData.Resources)
                    {
                        await redisService.SetAsync($"{localizationMemberKey}-{resource.LanguageCode}-{resource.Tag}", resource, redisCacheDuration, databaseId);
                    }
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}