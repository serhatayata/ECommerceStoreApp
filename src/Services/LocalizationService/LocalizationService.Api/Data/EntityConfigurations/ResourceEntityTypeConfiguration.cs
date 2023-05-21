using LocalizationService.Api.Entities;
using LocalizationService.Api.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Api.Data.EntityConfigurations
{
    public class ResourceEntityTypeConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable(name: "Resources", schema: "localization");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasIndex(r => r.ResourceCode).IsUnique();
            builder.HasIndex(r => new {r.Tag, r.LanguageId, r.MemberId}).IsUnique();
            
            builder.Property(r => r.Tag).HasColumnType("nvarchar(300)");

            builder.Property(r => r.LanguageId).IsRequired(false);

            builder.Property(r => r.ResourceCode).HasColumnType("nvarchar(40)");
            builder.Property(r => r.LanguageCode).HasColumnType("nvarchar(10)");

            builder.Property(r => r.CreateDate).HasColumnType("datetime2");
            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

            builder.HasOne(r => r.Language)
                   .WithMany(l => l.Resources)
                   .HasForeignKey(rl => rl.LanguageId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.Member)
                   .WithMany(l => l.Resources)
                   .HasForeignKey(rl => rl.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
