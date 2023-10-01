using Microsoft.AspNetCore.Localization;
using MonitoringService.Api.Dtos.Localization;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Utilities.Results;
using Polly;
using System.Globalization;
using System.Reflection;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers
{
    public class LocalizationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsProduction())
            {
                services.AddLocalization();

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

                var policy = Polly.Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    Serilog.Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                        ex.Message, nameof(LocalizationServiceInstaller),
                                        MethodBase.GetCurrentMethod()?.Name);
                });

                Task.Run(async () =>
                {
                    await policy.ExecuteAsync(async () =>
                    {
                        var gatewayClient = httpClientFactory.CreateClient("LocalizationService");
                        var languageResult = await gatewayClient.PostGetResponseAsync<DataResult<List<LanguageDto>>, string>("languages/get-all-for-clients", string.Empty);

                        if (languageResult == null || languageResult?.Data == null || !languageResult.Success)
                            throw new Exception($"Language result not found for {nameof(Install)}");

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
                }).Wait();
            }
        }
    }
}
