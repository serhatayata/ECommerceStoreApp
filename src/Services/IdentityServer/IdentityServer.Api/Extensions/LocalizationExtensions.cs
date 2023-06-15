using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Utilities.Results;
using Microsoft.AspNetCore.Localization;
using Polly;
using Serilog;
using System.Globalization;
using System.Net.Http;
using System.Reflection;

namespace IdentityServer.Api.Extensions
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
                var gatewayClient = httpClientFactory.CreateClient("gateway-specific");
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
    }
}
