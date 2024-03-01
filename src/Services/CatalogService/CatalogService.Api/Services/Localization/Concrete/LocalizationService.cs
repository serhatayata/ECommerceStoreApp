using CatalogService.Api.Dtos.Localization;
using CatalogService.Api.Models.Settings;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Localization.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogService.Api.Services.Localization.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRedisService _redisService;
        private readonly ILogger<LocalizationService> _logger;
        private readonly IConfiguration _configuration;

        private string _localizationMemberKey;

        private int _databaseId;

        public LocalizationService(
                      IRedisService redisService,
                      IHttpContextAccessor httpContextAccessor,
                      IHttpClientFactory httpClientFactory,
                      IConfiguration configuration,
                      ILogger<LocalizationService> logger)
        {
            _redisService = redisService;
            _logger = logger;
            _configuration = configuration;

            var localizationSettings = _configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();

            _localizationMemberKey = localizationSettings.MemberKey;

            _databaseId = localizationSettings.DatabaseId;
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
            => GetLocalizationData(culture, resourceKey, args);

        public string GetStringResource(string culture, string resourceKey)
           => GetLocalizationData(culture, resourceKey);

        private string GetLocalizationData(string currentCulture, string resourceKey, params object[] args)
        {
            string redisKey = GetResourceCacheKey(_localizationMemberKey, currentCulture, resourceKey);
            if (args != null && args.Count() > 0)
                return GetLocalizedValue(redisKey, args) ?? string.Empty;

            var redisValue = _redisService.Get<ResourceDto>(redisKey, _databaseId);

            return redisValue?.Value ?? string.Empty;
        }

        private string? GetLocalizedValue(string key, params object[] args)
        {
            var model = _redisService.Get<ResourceDto>(key, _databaseId);
            if (string.IsNullOrWhiteSpace(model?.Value))
                return string.Empty;

            return (args == null || args.Length == 0) ?
                       model.Value :
                       string.Format(model.Value, args);
        }

        private static string GetResourceCacheKey(
        string memberKey,
        string language,
        string key)
        {
            var result = string.Join("-",
                             memberKey,
                             language,
                             key);

            return result;
        }
    }
}
