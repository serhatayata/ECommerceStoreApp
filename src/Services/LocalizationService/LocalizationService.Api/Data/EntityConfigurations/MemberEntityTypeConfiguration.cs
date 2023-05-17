using LocalizationService.Api.Entities;
using LocalizationService.Api.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Api.Data.EntityConfigurations
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable(name: "Resources", schema: "localization");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasIndex(r => r.MemberKey).IsUnique();

            builder.Property(r => r.Name).HasColumnType("nvarchar(200)");

            builder.Property(r => r.MemberKey).HasColumnType("nvarchar(40)");

            builder.Property(r => r.CreateDate).HasColumnType("datetime2");
            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");
        }
    }
}
