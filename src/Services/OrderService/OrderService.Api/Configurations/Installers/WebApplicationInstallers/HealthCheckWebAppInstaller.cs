using OrderService.Api.Extensions;

namespace OrderService.Api.Configurations.Installers.WebApplicationInstallers;

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
