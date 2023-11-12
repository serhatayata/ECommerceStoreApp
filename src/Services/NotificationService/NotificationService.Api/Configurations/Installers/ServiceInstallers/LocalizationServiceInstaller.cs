using Microsoft.AspNetCore.Localization;
using NotificationService.Api.Dtos.Localization;
using NotificationService.Api.Extensions;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Utilities.Results;
using Polly;
using System.Globalization;
using System.Net.Http;
using System.Reflection;

namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class LocalizationServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsProduction())
        {
            services.AddLocalization();

            var environmentName = hostEnvironment.EnvironmentName;

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

            await policy.ExecuteAsync(async () =>
            {
                var localizationInfo = configuration.GetSection($"ServiceInformation:{environmentName}:LocalizationService").Get<ServiceInformationSettings>();

                var gatewayClient = httpClientFactory.CreateClient(localizationInfo.Name);
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
        }
    }
}