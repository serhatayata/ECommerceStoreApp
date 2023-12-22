using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CampaignItemEntityTypeConfiguration : IEntityTypeConfiguration<CampaignItem>
{
    public void Configure(EntityTypeBuilder<CampaignItem> builder)
    {
        builder.ToTable(name: "CampaignItems", schema: "campaign");

        builder.HasKey(c => c.Id);

        builder.Property(o => o.UserId)
               .HasColumnType(typeName: "nvarchar(40)")
               .IsRequired();

        builder.Property(o => o.Description)
               .HasColumnType(typeName: "nvarchar(1000)")
               .IsRequired();

        builder.Property(o => o.Code)
               .HasColumnType(typeName: "nvarchar(40)")
               .IsRequired();

        builder.Property(c => c.Status)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(o => o.CreationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired()
               .HasDefaultValueSql(sql: "getdate()");

        builder.Property(o => o.ExpirationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired();

        builder.HasOne(ci => ci.Campaign)
               .WithMany(c => c.CampaignItems)
               .HasForeignKey(cs => cs.CampaignId)
               .IsRequired(required: false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
