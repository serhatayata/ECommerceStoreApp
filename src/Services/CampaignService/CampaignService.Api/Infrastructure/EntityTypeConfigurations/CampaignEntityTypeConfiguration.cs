using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
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
                Rate = 20M,
                Amount = 100,
                IsForAllCategory = true,
                CampaignType = Models.Enums.CampaignTypes.Percentage,
                ExpirationDate = DateTime.Now.AddMonths(3),
                StartDate = DateTime.Now.AddHours(2),
                MaxUsage = 1000,
                UsageCount = 0
            },
            new Campaign()
            {
                Id = 2,
                Name = "Campaign_test_2",
                Status = Models.Enums.CampaignStatus.Active,
                Description = "Campaign test 2 description",
                Sponsor = "Sponsor 2",
                Rate = 30M,
                Amount = 200,
                IsForAllCategory = false,
                CampaignType = Models.Enums.CampaignTypes.Price,
                ExpirationDate = DateTime.Now.AddMonths(2),
                StartDate = DateTime.Now.AddHours(1),
                MaxUsage = 2000,
                UsageCount = 0
            }
        };

        builder.HasData(campaigns);
        #endregion
    }
}
