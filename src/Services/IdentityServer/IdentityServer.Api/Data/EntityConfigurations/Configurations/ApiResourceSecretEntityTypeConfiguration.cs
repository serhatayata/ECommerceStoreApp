using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Configurations
{
    public class ApiResourceSecretEntityTypeConfiguration : IEntityTypeConfiguration<ApiResourceSecret>
    {
        public void Configure(EntityTypeBuilder<ApiResourceSecret> builder)
        {
            builder.ToTable(name: "ApiResourceSecrets", schema: "configuration");

            builder.Property(s => s.Type).HasDefaultValue("SharedSecret");
        }
    }
}
