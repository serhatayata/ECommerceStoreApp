using FileService.Api.Attributes;
using FileService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class DbContextServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetConnectionString("FileDB");

        services.AddDbContext<FileDbContext>(options =>
        {
            options.UseSqlServer(connectionString: defaultConnString,
                                 sqlServerOptionsAction: sqlOptions =>
                                 {
                                     sqlOptions.MigrationsAssembly(assembly);
                                     // If we enable retry on failure, then we have to use execution strategy to follow the transactions seperately
                                     //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                 });
        }, ServiceLifetime.Scoped);
    }
}
