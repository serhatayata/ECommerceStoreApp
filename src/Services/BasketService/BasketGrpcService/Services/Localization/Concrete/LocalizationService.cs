using BasketGrpcService.Dtos.Localization;
using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Models.Settings;
using BasketGrpcService.Services.Localization.Abstract;
using BasketGrpcService.Services.Redis.Abstract;
using BasketGrpcService.Utilities.Results;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Serilog;
using System.Reflection;

namespace BasketGrpcService.Services.Localization.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LocalizationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private string _localizationMemberKey;
        private string _localizationSuffix1;
        private string _localizationSuffix2;

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

        private string? GetLocalizedValue(string key, params object[] args)
        {
            var value = _memoryCache.Get<string>(key);

            return (args == null || args.Length == 0) ?
                       value :
                       string.Format(value, args);
        }
    }
}
