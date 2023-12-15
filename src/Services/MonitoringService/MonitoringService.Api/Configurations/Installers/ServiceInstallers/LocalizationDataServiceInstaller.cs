using MonitoringService.Api.Extensions;
using MonitoringService.Api.Models.Base.Concrete;
using MonitoringService.Api.Models.CacheModels;
using MonitoringService.Api.Models.Settings;
using MonitoringService.Api.Services.Cache.Abstract;
using MonitoringService.Api.Utilities.Results;
using Polly;
using StackExchange.Redis;
using System.Reflection;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

public class LocalizationDataServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsProduction())
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

            var policy = Polly.Policy.Handle<Exception>()
                        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            Serilog.Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                ex.Message, nameof(Install),
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
                    var gatewayClient = httpClientFactory.CreateClient("LocalizationService");
                    _ = await gatewayClient.PostGetResponseAsync<Result, StringModel>("members/get-with-resources-by-memberkey-and-save-default", new StringModel() { Value = localizationMemberKey });
                }
            });

            if (result != null && result.FinalException != null)
                Serilog.Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Final Exception Type : {FinalExceptionType}",
                          result.FinalException.Message, nameof(LocalizationServiceInstaller),
                          MethodBase.GetCurrentMethod()?.Name,
                          result.ExceptionType);
        }
    }
}
