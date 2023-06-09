﻿using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.Settings;
using IdentityServer.Api.Services.Localization.Abstract;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Serilog;
using System.Reflection;

namespace IdentityServer.Api.Services.Localization.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LocalizationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private string _localizationMemberKey;
        private int _redisCacheDuration;
        private int _databaseId;

        public LocalizationService(
                      IRedisService redisService,
                      IHttpContextAccessor httpContextAccessor,
                      IHttpClientFactory httpClientFactory,
                      IConfiguration configuration,
                      ILogger<LocalizationService> logger)
        {
            _redisService = redisService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            var localizationSettings = _configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();

            _localizationMemberKey = localizationSettings.MemberKey;
            _databaseId = localizationSettings.DatabaseId;
            _redisCacheDuration = localizationSettings.CacheDuration;
        }

        public string this[string culture, string key, params object[] args]
        {
            get
            {
                var value = GetLocalizationData(culture, key, args);
                return string.IsNullOrWhiteSpace(value) ? " " : value; 
            }
        }

        public string this[string culture, string key]
        {
            get
            {
                var value = GetLocalizationData(culture, key);
                return string.IsNullOrWhiteSpace(value) ? " " : value;
            }
        }

        public string GetStringResource(string culture, string resourceKey, params object[] args)
        {
            return GetLocalizationData(culture, resourceKey, args);
        }

        public string GetStringResource(string culture, string resourceKey)
        {
            return GetLocalizationData(culture, resourceKey);
        }

        private string GetLocalizationData(string currentCulture, string resourceKey, params object[] args)
        {
            #region Memory Cache
            ////Memory cache 1
            //var memoryCacheValue = this.GetLocalizedValue($"{_memoryCache1Prefix}-{currentCulture}-{resourceKey}", args);
            //if (!string.IsNullOrWhiteSpace(memoryCacheValue))
            //    return memoryCacheValue;

            //var memory1Any = _memoryCache.TryGetValue(_memoryCache1Prefix, out var mem1AnyValue);
            //if (memory1Any)
            //    return string.Empty;

            ////Memory cache 2
            //var memoryCacheValue2 = this.GetLocalizedValue($"{_memoryCache2Prefix}-{currentCulture}-{resourceKey}", args);
            //if (!string.IsNullOrWhiteSpace(memoryCacheValue2))
            //    return memoryCacheValue2;

            //var memory2Any = _memoryCache.TryGetValue(_memoryCache2Prefix, out var mem2AnyValue);
            //if (memory2Any)
            //    return string.Empty;
            #endregion
            string redisKey = $"{_localizationMemberKey}-{currentCulture}-{resourceKey}";
            var redisValue = _redisService.Get<ResourceDto>(redisKey, _databaseId);

            if (!_redisService.AnyKeyExistsByPrefix(_localizationMemberKey, _databaseId))
                this.SetCacheValues();

            if (redisValue != null)
                return redisValue?.Value ?? string.Empty;

            var result = _redisService.Get<ResourceDto>(redisKey, _databaseId);
            return result?.Value ?? string.Empty;
        }

        private void SetCacheValues()
        {
            Task.Run(async () =>
            {
                var policy = Polly.Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                        ex.Message, nameof(LocalizationService),
                                        MethodBase.GetCurrentMethod()?.Name);
                });

                await policy.ExecuteAsync(async () =>
                {
                    var gatewayClient = _httpClientFactory.CreateClient("gateway-specific");
                    var result = await gatewayClient.PostGetResponseAsync<DataResult<List<ResourceDto>>, StringModel>("localization/members/get-with-resources-by-memberkey-and-save", new StringModel() { Value = _localizationMemberKey });

                    if (result == null || (!result?.Success ?? false))
                        throw new Exception("Localization data request not successful");

                    foreach (var resource in result?.Data ?? new List<ResourceDto>())
                        await _redisService.SetAsync($"{_localizationMemberKey}-{resource.LanguageCode}-{resource.Tag}", resource, _redisCacheDuration, _databaseId);

                    //MemoryCacheExtensions.SaveLocalizationData(memoryCache: _memoryCache,
                    //                                           configuration: _configuration,
                    //                                           result.Data);
                });
            });
        }

        private string GetLocalizedValue(string key, params object[] args)
        {
            var value = _redisService.Get<string>(key, _databaseId);

            return (args == null || args.Length == 0) ?
                       value :
                       string.Format(value, args);
        }
    }
}
