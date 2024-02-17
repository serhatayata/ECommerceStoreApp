using CampaignService.Api.Entities;
using CampaignService.Api.Models.Enums;
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

        #region SEED DATA
        CampaignRule[] rules = new []
        {
            new CampaignRule()
            {
                Id = 1,
                CampaignId = 1,
                Type = CampaignRuleTypes.NProductDiscount,
                Data = "2",
                Value = "15"
            },
            new CampaignRule()
            {
                Id = 2,
                CampaignId = 1,
                Type = CampaignRuleTypes.NProductDiscount,
                Data = "3",
                Value = "20"
            },
            new CampaignRule()
            {
                Id = 3,
                CampaignId = 2,
                Type = CampaignRuleTypes.BuyAPayB,
                Data = "4",
                Value = "3"
            }
        };

        builder.HasData(rules);
        #endregion
    }
}
