using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class IdentityResourcePropertyEntityTypeConfiguration : IEntityTypeConfiguration<IdentityResourceProperty>
    {
        public void Configure(EntityTypeBuilder<IdentityResourceProperty> builder)
        {
            builder.ToTable(name: "IdentityResourceProperties", schema: "configuration");
        }
    }
}
