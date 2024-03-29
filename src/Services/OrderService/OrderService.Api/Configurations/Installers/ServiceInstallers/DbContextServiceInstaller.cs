using Microsoft.EntityFrameworkCore;
using OrderService.Api.Infrastructure.Contexts;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class DbContextServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var connString = configuration.GetConnectionString("OrderDb");
        services.AddDbContext<OrderDbContext>(options =>
                                              options.UseSqlServer(connectionString: connString));

        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<OrderDbContext>();

        context.Database.Migrate();

        return Task.CompletedTask;
    }
}
