using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientPropertyEntityTypeConfiguration : IEntityTypeConfiguration<ClientProperty>
    {
        public void Configure(EntityTypeBuilder<ClientProperty> builder)
        {
            builder.ToTable(name: "ClientProperties", schema: "configuration");
        }
    }
}
