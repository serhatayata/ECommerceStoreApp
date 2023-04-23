using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientPostLogoutRedirectUriEntityTypeConfiguration : IEntityTypeConfiguration<ClientPostLogoutRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientPostLogoutRedirectUri> builder)
        {
            builder.ToTable(name: "ClientPostLogoutRedirectUris", schema: "configuration");
        }
    }
}
