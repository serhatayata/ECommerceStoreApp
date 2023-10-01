using Microsoft.EntityFrameworkCore;
using MonitoringService.Api.Infrastructure.Contexts;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

public class DbContextServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var connString = configuration.GetConnectionString("MonitoringDB");
        services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MonitoringDbContext>(options =>
                                                            options.UseNpgsql(connString)
                                                                   .UseLowerCaseNamingConvention());
    }
}
