using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ClientCorsOriginEntityTypeConfiguration : IEntityTypeConfiguration<ClientCorsOrigin>
    {
        public void Configure(EntityTypeBuilder<ClientCorsOrigin> builder)
        {
            builder.ToTable(name: "ClientCorsOrigins", schema: "configuration");
        }
    }
}
