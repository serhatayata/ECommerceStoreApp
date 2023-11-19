using StockService.Api.Extensions;

namespace StockService.Api.Configurations.Installers.WebApplicationInstallers;

public class HealthCheckWebAppInstaller : IWebAppInstaller
{
    public void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            ResponseWriter = HealthCheckExtensions.WriteHealthCheckResponse
        });
    }
}
