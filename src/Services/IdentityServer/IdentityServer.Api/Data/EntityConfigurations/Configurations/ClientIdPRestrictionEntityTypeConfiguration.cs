using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientIdPRestrictionEntityTypeConfiguration : IEntityTypeConfiguration<ClientIdPRestriction>
    {
        public void Configure(EntityTypeBuilder<ClientIdPRestriction> builder)
        {
            builder.ToTable(name: "ClientIdPRestrictions", schema: "configuration");
        }
    }
}
