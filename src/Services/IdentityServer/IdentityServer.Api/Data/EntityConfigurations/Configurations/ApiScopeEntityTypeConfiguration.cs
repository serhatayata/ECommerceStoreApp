using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiScopeEntityTypeConfiguration : IEntityTypeConfiguration<ApiScope>
    {
        public void Configure(EntityTypeBuilder<ApiScope> builder)
        {
            builder.ToTable(name: "ApiScopes", schema: "configuration");
        }
    }
}
