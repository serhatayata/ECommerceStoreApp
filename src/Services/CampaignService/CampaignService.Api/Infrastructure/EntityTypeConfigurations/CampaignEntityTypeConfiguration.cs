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
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

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

        builder.Property(o => o.Amount)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(o => o.CalculationAmount)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(c => c.CalculationType)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(c => c.IsForAllCategory)
               .HasColumnType(typeName: "bit")
               .HasDefaultValue(0)
               .IsRequired();

        #region SEED DATA
        Campaign[] campaigns = new[]
        {
            new Campaign()
            {
                Id = 1,
                Name = "Campaign_test_1",
                Status = Models.Enums.CampaignStatus.Active,
                Description = "Campaign test 1 description",
                Sponsor = "Sponsor 1",
                IsForAllCategory = true,
                ExpirationDate = DateTime.Now.AddMonths(3),
                StartDate = DateTime.Now.AddHours(2),
                MaxUsage = 1000,
                CampaignType = Models.Enums.CampaignTypes.Percentage,
                Amount = 10,
                CalculationAmount = 1000.0M,
                CalculationType = Models.Enums.CalculationTypes.Normal,
                MaxUsagePerUser = 1
            },
            new Campaign()
            {
                Id = 2,
                Name = "Campaign_test_2",
                Status = Models.Enums.CampaignStatus.Active,
                Description = "Campaign test 2 description",
                Sponsor = "Sponsor 2",
                IsForAllCategory = false,
                ExpirationDate = DateTime.Now.AddMonths(2),
                StartDate = DateTime.Now.AddHours(1),
                MaxUsage = 2000,
                CampaignType = Models.Enums.CampaignTypes.Price,
                Amount = 200,
                CalculationAmount = 2000.0M,
                CalculationType = Models.Enums.CalculationTypes.OverPrice,
                MaxUsagePerUser = 1
            }
        };

        builder.HasData(campaigns);
        #endregion
    }
}
