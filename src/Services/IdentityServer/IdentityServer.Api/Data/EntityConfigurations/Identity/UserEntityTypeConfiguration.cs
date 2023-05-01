using IdentityServer.Api.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.Identity
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(name: "Users", schema: "user");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.UserName).HasMaxLength(13);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Surname).HasMaxLength(100);
            builder.Property(x => x.Surname).HasMaxLength(30);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PhoneNumber).HasMaxLength(13);
            builder.HasIndex(x => x.PhoneNumber).IsUnique();

            builder.Property(x => x.CreateTime).HasDefaultValueSql("(getdate())");
            builder.Property(x => x.UpdateTime).HasDefaultValueSql("(getdate())");
            builder.Property(x => x.LastSeen).HasDefaultValueSql("(getdate())");
        }
    }
}
