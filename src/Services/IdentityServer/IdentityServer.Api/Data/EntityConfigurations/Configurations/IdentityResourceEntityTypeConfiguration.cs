using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class IdentityResourceEntityTypeConfiguration : IEntityTypeConfiguration<IdentityResource>
    {
        public void Configure(EntityTypeBuilder<IdentityResource> builder)
        {
            builder.ToTable(name: "IdentityResources", schema: "configuration");
        }
    }
}
