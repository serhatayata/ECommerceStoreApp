using IdentityServer.Api.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace IdentityServer.Api.Data.EntityConfigurations.Identity
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.ToTable(name: "Roles", schema: "user");

            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}
