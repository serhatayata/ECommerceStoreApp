using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Data.Contexts.Factories
{
    public class AppPersistedGrantDbContextFactory : IDesignTimeDbContextFactory<AppPersistedGrantDbContext>
    {
        public AppPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var assembly = typeof(Program).Assembly.GetName().Name;

            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("Configurations/Settings/appsettings.json", 
                            optional: false, 
                            reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppPersistedGrantDbContext>();
            var operationOptions = new OperationalStoreOptions();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assembly));

            return new AppPersistedGrantDbContext(optionsBuilder.Options, operationOptions);
        }
    }
}
