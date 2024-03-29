using Microsoft.EntityFrameworkCore;
using StockService.Api.Infrastructure.Contexts;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class DbContextServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var connString = configuration.GetConnectionString("StockDb");
        services.AddDbContext<StockDbContext>(options =>
                                              options.UseSqlServer(connectionString: connString));

        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<StockDbContext>();

        context.Database.Migrate();

        return Task.CompletedTask;
    }
}
