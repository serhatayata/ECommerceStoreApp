﻿using LocalizationService.Api.Attributes;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Models.ResourceModels;
using LocalizationService.Api.Services.Redis.Abstract;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 6)]
public class DataLoadServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<ILocalizationDbContext>();
        var redisService = serviceProvider.GetRequiredService<IRedisService>();

        var databaseId = configuration.GetValue<int>("LocalizationCacheSettings:DatabaseId");
        var database = redisService.GetDatabase(databaseId);

        var resources = await dbContext.Resources.ToListAsync();
        var batchData = resources.Batch(1000);
        foreach (var data in batchData)
        {
            var dataList = new Dictionary<RedisKey, RedisValue>();
            foreach (var resource in data)
            {
                var key = CacheExtensions.GetResourceCacheKey(
                                resource.MemberId.ToString(),
                                resource.LanguageId?.ToString() ?? string.Empty,
                                resource.Tag);

                var resourceModel = new ResourceCacheModel()
                {
                    Tag = resource.Tag,
                    Value = resource.Value,
                    ResourceCode = resource.ResourceCode,
                    LanguageCode = resource.LanguageCode,
                    MemberId = resource.MemberId
                };

                dataList.Add(key, resourceModel.ToJson());
            }

            await database.StringSetAsync(dataList.ToArray());
        }
    }
}
