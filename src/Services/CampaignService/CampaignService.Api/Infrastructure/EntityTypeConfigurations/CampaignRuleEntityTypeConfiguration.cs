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

        builder.Property(o => o.Name)
               .HasColumnType(typeName: "nvarchar(255)")
               .IsRequired();

        builder.Property(o => o.Description)
               .HasColumnType(typeName: "nvarchar(255)")
               .IsRequired();

        builder.Property(o => o.Value)
               .HasColumnType(typeName: "nvarchar(MAX)")
               .IsRequired();

        #region SEED DATA
        CampaignRule[] rules = new[]
        {
            new CampaignRule()
            {
                Id = 1,
                Name = "rule_1",
                Description = "Rule description 1 for a campaign",
                Value = "{}"
            },
            new CampaignRule()
            {
                Id = 2,
                Name = "rule_2",
                Description = "Rule description 2 for a campaign",
                Value = "{}"
            },
            new CampaignRule()
            {
                Id = 3,
                Name = "rule_3",
                Description = "Rule description 3 for a campaign",
                Value = "{}"
            }
        };

        builder.HasData(rules);
        #endregion
    }
}
