using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiResourceScopeEntityTypeConfiguration : IEntityTypeConfiguration<ApiResourceScope>
    {
        public void Configure(EntityTypeBuilder<ApiResourceScope> builder)
        {
            builder.ToTable(name: "ApiResourceScopes", schema: "configuration");
        }
    }
}
