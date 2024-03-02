using Microsoft.IdentityModel.Logging;
using Web.ApiGateway.Attributes;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 6)]
public class AuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAuthorization(options =>
        {
            // Will be filled
        });

        IdentityModelEventSource.ShowPII = true;
    }
}
