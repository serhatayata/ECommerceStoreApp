using NotificationService.Api.Dtos.Localization;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Base.Abstract;
using NotificationService.Api.Services.Cache.Abstract;

namespace NotificationService.Api.Services.Base.Concrete;

public class LocalizationService : ILocalizationService
{
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LocalizationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

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
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

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
            return this.GetLocalizedValue(redisKey, args) ?? string.Empty;

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
