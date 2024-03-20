using CatalogService.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

public class DbContextServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(connectionString: defaultConnString,
                                 sqlServerOptionsAction: sqlOptions =>
                                 {
                                     sqlOptions.MigrationsAssembly(assembly);
                                     // If we enable retry on failure, then we have to use execution strategy to follow the transactions seperately
                                     //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                 });
        }, ServiceLifetime.Scoped);

        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<CatalogDbContext>();

        context.Database.Migrate();
    }
}
