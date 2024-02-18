using CampaignService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CouponItemEntityTypeConfiguration : IEntityTypeConfiguration<CouponItem>
{
    public void Configure(EntityTypeBuilder<CouponItem> builder)
    {
        builder.ToTable(name: "CouponItems", schema: "coupon");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.UserId)
               .HasColumnType(typeName: "nvarchar(40)")
               .IsRequired(required: false);

        builder.Property(c => c.Status)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(c => c.OrderId)
               .HasColumnType(typeName: "int")
               .IsRequired(required: false);
    }
}
