using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClientClaim>
    {
        public void Configure(EntityTypeBuilder<ClientClaim> builder)
        {
            builder.ToTable(name: "ClientClaims", schema: "configuration");
        }
    }
}
