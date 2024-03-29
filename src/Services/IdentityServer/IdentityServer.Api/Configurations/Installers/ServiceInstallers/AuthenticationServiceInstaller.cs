using IdentityServer.Api.Attributes;
using IdentityServer.Api.Extensions.Authentication;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 13)]
public class AuthenticationServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddLocalApiAuthentication();
        services.UseVerifyCodeTokenAuthentication();

        return Task.CompletedTask;
    }
}
