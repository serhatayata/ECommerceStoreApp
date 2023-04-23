using IdentityServer.Api.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Identity
{
    public class UserLoginEntityTypeConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(name: "UserLogins", schema: "user");

            builder.Property(u => u.ProviderDisplayName).HasColumnType("nvarchar(450)");
        }
    }
}
