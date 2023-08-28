using MonitoringService.Api.Models.Settings;

namespace MonitoringService.Api.Extensions;

public static class HttpExtensions
{
    public static void AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        #region Service Http Clients
        var serviceInformation = configuration.GetSection($"ServiceInformation:{env}").Get<ServiceInformationSettings[]>();

        foreach (var info in serviceInformation)
        {
            services.AddHttpClient(info.Name, config =>
            {
                var baseAddress = info.Url;
                config.BaseAddress = new Uri(baseAddress);
            });
        }

        #endregion
    }
}
