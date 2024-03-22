using IdentityServer.Api.Attributes;
using IdentityServer.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class DbContextServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

        services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);
    }
}
