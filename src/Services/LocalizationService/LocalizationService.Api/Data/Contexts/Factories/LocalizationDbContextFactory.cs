using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LocalizationService.Api.Data.Contexts.Factories
{
    public class LocalizationDbContextFactory : IDesignTimeDbContextFactory<LocalizationDbContext>
    {
        public LocalizationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configurations/appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<LocalizationDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new LocalizationDbContext(builder.Options);
        }
    }
}
