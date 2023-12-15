using BasketService.Api.Dtos.Localization;
using BasketService.Api.Extensions;
using BasketService.Api.Utilities.Results;
using Microsoft.AspNetCore.Localization;
using Polly;
using Serilog;
using System.Globalization;
using System.Reflection;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class LocalizationServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddLocalization();

        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        var policy = Polly.Policy.Handle<Exception>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
        {
            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                ex.Message, nameof(LocalizationServiceInstaller),
                                MethodBase.GetCurrentMethod()?.Name);
        });

        var result = await policy.ExecuteAndCaptureAsync(async () =>
        {
            var gatewayClient = httpClientFactory.CreateClient("gateway");
            var languageResult = await gatewayClient.PostGetResponseAsync<DataResult<List<LanguageDto>>, string>("localization/languages/get-all-for-clients", string.Empty);

            if (languageResult == null || languageResult?.Data == null || !languageResult.Success)
                throw new Exception($"Language result not found for {nameof(LocalizationServiceInstaller)}");

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

        if (result != null && result.FinalException != null)
            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Final Exception Type : {FinalExceptionType}",
                      result.FinalException.Message, nameof(LocalizationServiceInstaller),
                      MethodBase.GetCurrentMethod()?.Name,
                      result.ExceptionType);
    }
}
