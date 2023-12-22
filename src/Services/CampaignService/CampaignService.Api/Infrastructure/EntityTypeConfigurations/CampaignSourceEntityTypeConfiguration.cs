using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CampaignSourceEntityTypeConfiguration : IEntityTypeConfiguration<CampaignSource>
{
    public void Configure(EntityTypeBuilder<CampaignSource> builder)
    {
        builder.ToTable(name: "CampaignSources", schema: "campaign");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.EntityId)
               .IsRequired();

        builder.HasOne(cs => cs.Campaign)
               .WithMany(c => c.CampaignSources)
               .HasForeignKey(cs => cs.CampaignId)
               .IsRequired(required: false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
