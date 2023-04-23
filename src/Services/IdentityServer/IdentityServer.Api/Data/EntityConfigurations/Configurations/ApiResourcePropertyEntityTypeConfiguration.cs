using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiResourcePropertyEntityTypeConfiguration : IEntityTypeConfiguration<ApiResourceProperty>
    {
        public void Configure(EntityTypeBuilder<ApiResourceProperty> builder)
        {
            builder.ToTable(name: "ApiResourceProperties", schema: "configuration");
        }
    }
}
