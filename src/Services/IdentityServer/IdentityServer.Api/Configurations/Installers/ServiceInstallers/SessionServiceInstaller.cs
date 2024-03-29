namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

public class SessionServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
        });

        return Task.CompletedTask;
    }
}
