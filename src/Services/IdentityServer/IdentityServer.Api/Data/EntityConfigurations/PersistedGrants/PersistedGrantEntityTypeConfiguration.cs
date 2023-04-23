using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.PersistedGrants
{
    public class PersistedGrantEntityTypeConfiguration : IEntityTypeConfiguration<PersistedGrant>
    {
        public void Configure(EntityTypeBuilder<PersistedGrant> builder)
        {
            builder.ToTable(name: "PersistedGrants", schema: "persisted");
        }
    }
}
