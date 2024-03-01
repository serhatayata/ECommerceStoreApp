using IdentityServer.Api.Dtos.Localization;
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
            var redisKey = GetResourceCacheKey(_localizationMemberKey, currentCulture, resourceKey);
            if (args != null && args.Count() > 0)
                return GetLocalizedValue(redisKey, args) ?? string.Empty;

            var redisValue = _redisService.Get<ResourceDto>(redisKey, _databaseId);

            return redisValue?.Value ?? string.Empty;
        }

        private string GetLocalizedValue(string key, params object[] args)
        {
            var value = _redisService.Get<string>(key, _databaseId);

            return (args == null || args.Length == 0) ?
                       value :
                       string.Format(value, args);
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
