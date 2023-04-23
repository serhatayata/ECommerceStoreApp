using IdentityServer.Api.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Identity
{
    public class RoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable(name: "RoleClaims", schema: "user");

            builder.Property(u => u.ClaimType).HasColumnType("nvarchar(450)");
            builder.Property(u => u.ClaimValue).HasColumnType("nvarchar(450)");
        }
    }
}
