using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CampaignRuleEntityTypeConfiguration : IEntityTypeConfiguration<CampaignRule>
{
    public void Configure(EntityTypeBuilder<CampaignRule> builder)
    {
        builder.ToTable(name: "CampaignRules", schema: "campaign");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Data)
               .HasColumnType(typeName: "nvarchar(MAX)")
               .IsRequired();

        builder.Property(c => c.Value)
               .HasColumnType(typeName: "nvarchar(MAX)")
               .IsRequired();

        builder.Property(c => c.Type)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(c => c.CampaignId)
               .IsRequired();

        builder.HasOne(cs => cs.Campaign)
               .WithMany(c => c.CampaignRules)
               .HasForeignKey(cs => cs.CampaignId)
               .IsRequired(required: false)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
