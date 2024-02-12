using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.Infrastructure.Contexts;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CampaignDbContext>
{
    public CampaignDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Configurations/Settings/appsettings.json",
                         optional: false,
                         reloadOnChange: true)
            .Build();

        var builder = new DbContextOptionsBuilder<CampaignDbContext>();

        var connectionString = configuration.GetConnectionString("CampaignDB");

        builder.UseSqlServer(connectionString);

        return new CampaignDbContext(configuration);
    }
}