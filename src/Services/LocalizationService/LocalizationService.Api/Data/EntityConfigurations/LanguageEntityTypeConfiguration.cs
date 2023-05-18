using LocalizationService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Api.Data.EntityConfigurations
{
    public class LanguageEntityTypeConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(name: "Languages", schema: "localization");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasIndex(r => r.Code).IsUnique();

            builder.Property(r => r.Code).HasColumnType("nvarchar(10)");

            builder.Property(r => r.DisplayName).HasColumnType("nvarchar(300)");
        }
    }
}
