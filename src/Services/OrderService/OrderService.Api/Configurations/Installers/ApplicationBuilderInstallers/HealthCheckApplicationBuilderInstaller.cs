using OrderService.Api.Extensions;

namespace OrderService.Api.Configurations.Installers.ApplicationBuilderInstallers;

public class HealthCheckApplicationBuilderInstaller : IApplicationBuilderInstaller
{
    public void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            ResponseWriter = HealthCheckExtensions.WriteHealthCheckResponse
        });
    }
}
