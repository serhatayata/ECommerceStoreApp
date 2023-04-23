using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Data.Contexts.Factories
{
    public class AppConfigurationDbContextFactory : IDesignTimeDbContextFactory<AppConfigurationDbContext>
    {
        public AppConfigurationDbContext CreateDbContext(string[] args)
        {
            var assembly = typeof(Program).Assembly.GetName().Name;

            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("Configurations/appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppConfigurationDbContext>();
            var storeOptions = new ConfigurationStoreOptions();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assembly));

            return new AppConfigurationDbContext(optionsBuilder.Options, storeOptions);
        }
    }
}
