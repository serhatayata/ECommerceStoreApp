using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiResourceClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApiResourceClaim>
    {
        public void Configure(EntityTypeBuilder<ApiResourceClaim> builder)
        {
            builder.ToTable(name: "ApiResourceClaims", schema: "configuration");
        }
    }
}
