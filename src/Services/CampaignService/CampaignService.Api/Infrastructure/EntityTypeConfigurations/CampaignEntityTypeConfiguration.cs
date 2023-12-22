using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CampaignEntityTypeConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.ToTable(name: "Campaigns", schema: "campaign");

        builder.HasKey(c => c.Id);

        //builder.HasIndex(o => o.Code).IsUnique();

        builder.Property(c => c.Status)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(o => o.Name)
               .HasColumnType(typeName: "nvarchar(255)")
               .IsRequired();

        builder.Property(o => o.Description)
               .HasColumnType(typeName: "nvarchar(1000)")
               .IsRequired();

        builder.Property(o => o.ExpirationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired();

        builder.Property(o => o.StartDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired();

        builder.Property(o => o.CreationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired()
               .HasDefaultValueSql(sql: "getdate()");

        builder.Property(o => o.UpdateDate)
               .HasColumnType(typeName: "datetime2");

        builder.Property(o => o.Sponsor)
               .HasColumnType(typeName: "nvarchar(255)")
               .IsRequired();

        builder.Property(c => c.CampaignType)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(o => o.Rate)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(o => o.Amount)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(c => c.IsForAllCategory)
               .HasColumnType(typeName: "bit")
               .HasDefaultValue(0)
               .IsRequired();
    }
}
