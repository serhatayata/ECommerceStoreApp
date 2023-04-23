using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class IdentityResourceClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityResourceClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityResourceClaim> builder)
        {
            builder.ToTable(name: "IdentityResourceClaims", schema: "configuration");
        }
    }
}
