using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Services.Localization.Abstract;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Serilog;
using System.Net.Http;
using System.Reflection;

namespace IdentityServer.Api.Services.Localization.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LocalizationService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private string _localizationMemberKey;
        private string _localizationSuffix1;
        private string _localizationSuffix2;

        private int _redisCacheDuration;
        private int _localizationMemoryDuration1;
        private int _localizationMemoryDuration2;

        private string _memoryCache1Prefix;
        private string _memoryCache2Prefix;

        private int _databaseId;

        public LocalizationService(
                      IRedisService redisService,
                      IHttpContextAccessor httpContextAccessor,
                      IMemoryCache memoryCache,
                      IHttpClientFactory httpClientFactory,
                      IConfiguration configuration,
                      ILogger<LocalizationService> logger)
        {
            _redisService = redisService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            _localizationMemberKey = _configuration.GetSection("LocalizationSettings:MemberKey").Value;
            _localizationSuffix1 = _configuration.GetSection("LocalizationSettings:MemoryCache:Suffix1").Value;
            _localizationSuffix2 = _configuration.GetSection("LocalizationSettings:MemoryCache:Suffix2").Value;

            _databaseId = _configuration.GetSection("RedisSettings:LocalizationCacheDbId").Get<int>();

            _localizationMemoryDuration1 = _configuration.GetSection("LocalizationSettings:MemoryCache:Duration1").Get<int>();
            _localizationMemoryDuration2 = _configuration.GetSection("LocalizationSettings:MemoryCache:Duration2").Get<int>();

            _redisCacheDuration = _configuration.GetSection("LocalizationSettings:CacheDuration").Get<int>();

            _memoryCache1Prefix = $"{_localizationMemberKey}-{_localizationSuffix1}";
            _memoryCache2Prefix = $"{_localizationMemberKey}-{_localizationSuffix2}";
        }

        public async Task<string> GetStringResource(string culture, string resourceKey, params object[] args)
        {
            return await GetLocalizationData(culture, resourceKey, args);
        }

        private async Task<string> GetLocalizationData(string currentCulture, string resourceKey, params object[] args)
        {
            //Memory cache 1
            var memoryCacheValue = this.GetLocalizedValue($"{_memoryCache1Prefix}-{currentCulture}-{resourceKey}", args);
            if (!string.IsNullOrWhiteSpace(memoryCacheValue))
                return memoryCacheValue;

            string dummyDataValue = this.GetLocalizedValue($"{_localizationMemberKey}") ?? string.Empty;
            var memory1Any = _memoryCache.TryGetValue(dummyDataValue, out var mem1AnyValue);
            if (memory1Any)
                return string.Empty;

            //Memory cache 2
            var memoryCacheValue2 = this.GetLocalizedValue($"{_memoryCache2Prefix}-{currentCulture}-{resourceKey}", args);
            if (!string.IsNullOrWhiteSpace(memoryCacheValue2))
                return memoryCacheValue2;

            string dummyData2Value = this.GetLocalizedValue($"{_memoryCache2Prefix}") ?? string.Empty;
            var memory2Any = _memoryCache.TryGetValue(_memoryCache2Prefix, out var mem2AnyValue);
            if (memory2Any)
                return string.Empty;

            string redisKey = $"{_localizationMemberKey}-{currentCulture}-{resourceKey}";
            var redisValue = _redisService.Get(redisKey);

            if (!_redisService.AnyKeyExistsByPrefix(_localizationMemberKey, _databaseId))
                await this.SetCacheValues();

            if (!string.IsNullOrWhiteSpace(redisValue))
                return redisValue;

            var result = _redisService.Get(redisValue);
            return result;
        }

        private async Task SetCacheValues()
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
                var result = await gatewayClient.PostGetResponseAsync<DataResult<MemberDto>, StringModel>("localization/members/get-with-resources-by-memberkey-and-save", new StringModel() { Value = _localizationMemberKey });

                if (result == null || (!result?.Success ?? false))
                    throw new Exception("Localization data request not successful");

                foreach (var resource in result?.Data?.Resources ?? new List<ResourceDto>())
                    await _redisService.SetAsync($"{_localizationMemberKey}-{resource.LanguageCode}-{resource.Tag}", resource, _redisCacheDuration, _databaseId);

                MemoryCacheExtensions.SaveLocalizationData(memoryCache: _memoryCache,
                                                           configuration: _configuration,
                                                           result.Data);
            });
        }

        private string GetLocalizedValue(string key, params object[] args)
        {
            var value = _memoryCache.Get<string>(key);

            return (args == null || args.Length == 0) ?
                       value :
                       string.Format(value, args);
        }
    }
}
