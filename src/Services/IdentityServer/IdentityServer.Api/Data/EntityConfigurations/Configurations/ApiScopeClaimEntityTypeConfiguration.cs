using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiScopeClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApiScopeClaim>
    {
        public void Configure(EntityTypeBuilder<ApiScopeClaim> builder)
        {
            builder.ToTable(name: "ApiScopeClaims", schema: "configuration");
        }
    }
}
