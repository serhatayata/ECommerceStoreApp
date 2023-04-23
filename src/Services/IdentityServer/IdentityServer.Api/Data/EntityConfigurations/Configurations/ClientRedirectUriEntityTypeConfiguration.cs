using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientRedirectUriEntityTypeConfiguration : IEntityTypeConfiguration<ClientRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientRedirectUri> builder)
        {
            builder.ToTable(name: "ClientRedirectUris", schema: "configuration");
        }
    }
}
