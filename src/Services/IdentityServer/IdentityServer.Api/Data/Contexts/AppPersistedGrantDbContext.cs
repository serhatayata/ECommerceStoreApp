using IdentityServer.Api.Data.EntityConfigurations.PersistedGrants;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Data.Contexts
{
    public class AppPersistedGrantDbContext : PersistedGrantDbContext<AppPersistedGrantDbContext>
    {
        public AppPersistedGrantDbContext(DbContextOptions<AppPersistedGrantDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Default schema
            modelBuilder.HasDefaultSchema("persisted");
            // Entity configurations
            modelBuilder.ApplyConfiguration(new DeviceCodeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PersistedGrantEntityTypeConfiguration());
        }
    }
}
