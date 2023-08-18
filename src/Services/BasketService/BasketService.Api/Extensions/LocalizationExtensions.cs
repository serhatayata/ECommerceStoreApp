using BasketService.Api.Dtos.Localization;
using BasketService.Api.Models;
using BasketService.Api.Models.Settings;
using BasketService.Api.Services.Redis.Abstract;
using BasketService.Api.Utilities.Results;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Serilog;
using StackExchange.Redis;
using System.Globalization;
using System.Reflection;

namespace BasketService.Api.Extensions
{
    public static class LocalizationExtensions
    {
        public static async Task AddLocalizationSettingsAsync(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddLocalization();

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

            var policy = Polly.Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                    ex.Message, nameof(LocalizationExtensions),
                                    MethodBase.GetCurrentMethod()?.Name);
            });

            await policy.ExecuteAsync(async () =>
            {
                var gatewayClient = httpClientFactory.CreateClient("gateway");
                var languageResult = await gatewayClient.PostGetResponseAsync<DataResult<List<LanguageDto>>, string>("localization/languages/get-all-for-clients", string.Empty);

                if (languageResult == null || languageResult?.Data == null || !languageResult.Success)
                    throw new Exception($"Language result not found for {nameof(AddLocalizationSettingsAsync)}");

                services.Configure<RequestLocalizationOptions>(options =>
                {
                    var cultures = languageResult.Data.Select(x => new CultureInfo(x.Code)).ToArray();

                    var defaultCulture = cultures.FirstOrDefault(x => x.Name == "tr-TR");
                    options.DefaultRequestCulture = new RequestCulture(culture: defaultCulture?.Name ?? "tr-TR",
                                                                       uiCulture: defaultCulture?.Name ?? "tr-TR");

                    options.SupportedCultures = cultures;
                    options.SupportedUICultures = cultures;
                });
            });
        }

        public static async Task AddLocalizationDataAsync(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

            var policy = Polly.Policy.Handle<Exception>()
                        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                ex.Message, nameof(AddLocalizationDataAsync),
                                                MethodBase.GetCurrentMethod()?.Name);
                        });

            await policy.ExecuteAsync(async () =>
            {
                var values = new Dictionary<string, RedisValue>();

                var localizationSettings = configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();
                var redisSettings = configuration.GetSection("RedisOptions").Get<RedisOptions>();

                var localizationMemberKey = localizationSettings.MemberKey;
                var redisCacheDuration = localizationSettings.CacheDuration;

                int databaseId = localizationSettings.DatabaseId;

                if (!redisService.AnyKeyExistsByPrefix(localizationMemberKey, databaseId))
                {
                    var gatewayClient = httpClientFactory.CreateClient("gateway");
                    var result = await gatewayClient.PostGetResponseAsync<Result, StringModel>("localization/members/get-with-resources-by-memberkey-and-save-default", new StringModel() { Value = localizationMemberKey });

                    if (!result?.Success ?? true)
                        throw new Exception("Localization data request not successful");
                }
            });
        }
    }
}
