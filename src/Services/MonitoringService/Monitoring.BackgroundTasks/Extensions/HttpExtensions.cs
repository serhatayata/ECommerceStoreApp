using Monitoring.BackgroundTasks.Infrastructure.DelegatingHandlers;

namespace Monitoring.BackgroundTasks.Extensions;

public static class HttpExtensions
{
    public static void AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<MonitoringAuthorizationDelegatingHandler>();

        #region HttpClients
        string gatewayClient = configuration.GetSection($"ServiceInformation:{env}:ApiGateway:Url").Value;

        services.AddHttpClient("gateway", config =>
        {
            var baseAddress = $"{gatewayClient}";
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<MonitoringAuthorizationDelegatingHandler>();

        #endregion
    }

    public static string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
    {
        var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
        var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
        string culture = currentCulture.Value;

        return culture;
    }
}
