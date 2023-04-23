using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiScopePropertyEntityTypeConfiguration : IEntityTypeConfiguration<ApiScopeProperty>
    {
        public void Configure(EntityTypeBuilder<ApiScopeProperty> builder)
        {
            builder.ToTable(name: "ApiScopeProperties", schema: "configuration");
        }
    }
}
