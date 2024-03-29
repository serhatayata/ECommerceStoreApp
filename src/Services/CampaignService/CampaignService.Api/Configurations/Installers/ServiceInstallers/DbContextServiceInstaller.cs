using CampaignService.Api.Attributes;
using CampaignService.Api.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class DbContextServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetConnectionString("CampaignDB");

        services.AddDbContext<CampaignDbContext>(options =>
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
        var context = serviceProvider.GetRequiredService<CampaignDbContext>();

        context.Database.EnsureCreated();

        return Task.CompletedTask;
    }
}
