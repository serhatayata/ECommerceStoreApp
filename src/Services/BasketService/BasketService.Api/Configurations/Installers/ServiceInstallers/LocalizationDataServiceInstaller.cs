using BasketService.Api.Extensions;
using BasketService.Api.Models;
using BasketService.Api.Models.Settings;
using BasketService.Api.Services.Redis.Abstract;
using BasketService.Api.Utilities.Results;
using Polly;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class LocalizationDataServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
        var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        var policy = Polly.Policy.Handle<Exception>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                            ex.Message, nameof(LocalizationDataServiceInstaller),
                                            MethodBase.GetCurrentMethod()?.Name);
                    });

        var result = await policy.ExecuteAndCaptureAsync(async () =>
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

        if (result != null && result.FinalException != null)
            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Final Exception Type : {FinalExceptionType}",
                      result.FinalException.Message, nameof(LocalizationServiceInstaller),
                      MethodBase.GetCurrentMethod()?.Name,
                      result.ExceptionType);
    }
}
