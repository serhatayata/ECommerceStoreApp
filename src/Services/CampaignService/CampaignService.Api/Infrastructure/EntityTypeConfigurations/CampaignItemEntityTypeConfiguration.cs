using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CampaignService.Api.Extensions;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CampaignItemEntityTypeConfiguration : IEntityTypeConfiguration<CampaignItem>
{
    public void Configure(EntityTypeBuilder<CampaignItem> builder)
    {
        builder.ToTable(name: "CampaignItems", schema: "campaign");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

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
               .OnDelete(DeleteBehavior.Cascade);

        #region SEED DATA
        CampaignItem[] items = new[]
        {
            new CampaignItem()
            {
                Id = 1,
                CampaignId = 1,
                UserId = "12s3a45d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Active,
                Description = "Campaign Item 1",
                ExpirationDate = DateTime.Now.AddMonths(2)
            },
            new CampaignItem()
            {
                Id = 2,
                CampaignId = 1,
                UserId = "21d23a45d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Passive,
                Description = "Campaign Item 2",
                ExpirationDate = DateTime.Now.AddMonths(1)
            },
            new CampaignItem()
            {
                Id = 3,
                CampaignId = 1,
                UserId = "41d2dd46d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Active,
                Description = "Campaign Item 3",
                ExpirationDate = DateTime.Now.AddMonths(2)
            },
            new CampaignItem()
            {
                Id = 4,
                CampaignId = 2,
                UserId = "188aa45d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Active,
                Description = "Campaign Item 4",
                ExpirationDate = DateTime.Now.AddMonths(1)
            },
            new CampaignItem()
            {
                Id = 5,
                CampaignId = 2,
                UserId = "2er23575d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Passive,
                Description = "Campaign Item 5",
                ExpirationDate = DateTime.Now.AddDays(6)
            },
            new CampaignItem()
            {
                Id = 6,
                CampaignId = 2,
                UserId = "51d24as46d6",
                Code = DataGenerationExtensions.RandomCode(10),
                Status = Models.Enums.CampaignItemStatus.Active,
                Description = "Campaign Item 6",
                ExpirationDate = DateTime.Now.AddMonths(1)
            }
        };

        builder.HasData(items);
        #endregion
    }
}
