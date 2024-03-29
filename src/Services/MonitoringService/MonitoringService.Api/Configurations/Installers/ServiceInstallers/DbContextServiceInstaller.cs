using Microsoft.EntityFrameworkCore;
using MonitoringService.Api.Attributes;
using MonitoringService.Api.Infrastructure.Contexts;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 9)]
public class DbContextServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var connString = configuration.GetConnectionString("MonitoringDB");
        services.AddEntityFrameworkNpgsql()
                        .AddDbContext<MonitoringDbContext>(options =>
                                                            options.UseNpgsql(connString)
                                                                   .UseLowerCaseNamingConvention());

        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<MonitoringDbContext>();

        _ = context.Database.EnsureCreated();

        return Task.CompletedTask;
    }
}
