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
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.EntityId)
               .IsRequired();

        builder.HasOne(cs => cs.Campaign)
               .WithMany(c => c.CampaignSources)
               .HasForeignKey(cs => cs.CampaignId)
               .IsRequired(required: false)
               .OnDelete(DeleteBehavior.Cascade);

        #region SEED DATA
        CampaignSource[] sources = new[]
        {
            new CampaignSource()
            {
                Id = 1,
                EntityId = 11,
                CampaignId = 1
            },
            new CampaignSource()
            {
                Id = 2,
                EntityId = 14,
                CampaignId = 1
            },
            new CampaignSource()
            {
                Id = 3,
                EntityId = 4,
                CampaignId = 1
            },
            new CampaignSource()
            {
                Id = 4,
                EntityId = 7,
                CampaignId = 2
            },
            new CampaignSource()
            {
                Id = 5,
                EntityId = 23,
                CampaignId = 2
            },
            new CampaignSource()
            {
                Id = 6,
                EntityId = 45,
                CampaignId = 2
            }
        };

        builder.HasData(sources);
        #endregion
    }
}
