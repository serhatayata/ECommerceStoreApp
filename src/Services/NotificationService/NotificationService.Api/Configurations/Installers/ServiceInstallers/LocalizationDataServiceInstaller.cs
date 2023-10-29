using NotificationService.Api.Extensions;
using NotificationService.Api.Models.Base;
using NotificationService.Api.Models.CacheModels;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Cache.Abstract;
using NotificationService.Api.Utilities.Results;
using Polly;
using System.Reflection;

namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class LocalizationDataServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
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

            Task.Run(async () =>
            {
                await policy.ExecuteAsync(async () =>
                {
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
            }).Wait();
        }
    }
}
