using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdentityServer.Api.Data.EntityConfigurations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Data.Contexts
{
    public class AppConfigurationDbContext : ConfigurationDbContext<AppConfigurationDbContext>
    {
        public AppConfigurationDbContext(DbContextOptions<AppConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) : base(options, storeOptions)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Default schema
            modelBuilder.HasDefaultSchema("configuration");
            // Entity configurations
            modelBuilder.ApplyConfiguration(new ApiResourceClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiResourcePropertyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiResourceScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiResourceSecretEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiScopeClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApiScopePropertyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCorsOriginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientGrantTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientIdPRestrictionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPostLogoutRedirectUriEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPropertyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientRedirectUriEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientScopeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientSecretEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityResourceClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityResourcePropertyEntityTypeConfiguration());
        }
    }
}
