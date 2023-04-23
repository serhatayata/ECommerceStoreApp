using IdentityServer.Api.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Identity
{
    public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(name: "UserClaims", schema: "user");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.ClaimType).HasColumnType("nvarchar(450)");
            builder.Property(u => u.ClaimValue).HasColumnType("nvarchar(450)");
            builder.Property(x => x.CreateTime).HasDefaultValueSql("(getdate())");
        }
    }
}
