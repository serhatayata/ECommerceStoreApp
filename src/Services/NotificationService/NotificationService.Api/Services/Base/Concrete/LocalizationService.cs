using NotificationService.Api.Dtos.Localization;
using NotificationService.Api.Extensions;
using NotificationService.Api.Models.Base;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Base.Abstract;
using NotificationService.Api.Services.Cache.Abstract;
using NotificationService.Api.Utilities.Results;
using Polly;
using Serilog;
using System.Reflection;

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
    {
        return GetLocalizationData(culture, resourceKey, args);
    }

    public string GetStringResource(string culture, string resourceKey)
    {
        return GetLocalizationData(culture, resourceKey);
    }

    private string GetLocalizationData(string currentCulture, string resourceKey, params object[] args)
    {
        string redisKey = GetResourceCacheKey(_localizationMemberKey, currentCulture, resourceKey);
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
                _logger.LogError("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                 ex.Message, nameof(LocalizationService),
                                 MethodBase.GetCurrentMethod()?.Name);
            });

            await policy.ExecuteAsync(async () =>
            {
                var gatewayClient = _httpClientFactory.CreateClient("LocalizationService");
                var result = await gatewayClient.PostGetResponseAsync<Result, StringModel>("localization/members/get-with-resources-by-memberkey-and-save-default", new StringModel() { Value = _localizationMemberKey });

                if (result == null || (!result?.Success ?? false))
                    throw new Exception("Localization data request not successful");
            });
        });
    }

    private string? GetLocalizedValue(string key, params object[] args)
    {
        var value = _redisService.Get<string>(key, _databaseId);

        return (args == null || args.Length == 0) ?
                   value :
                   string.Format(value, args);
    }

    public static string GetResourceCacheKey(
    string prefix,
    string language,
    string key)
    {
        var result = string.Join("-",
                         prefix,
                         language,
                         key);

        return result;
    }
}
