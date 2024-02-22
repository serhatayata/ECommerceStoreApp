using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignService.Api.Infrastructure.EntityTypeConfigurations;

public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable(name: "Coupons", schema: "coupon");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.RowVersion).IsRowVersion();

        builder.Property(o => o.Name)
               .HasColumnType(typeName: "nvarchar(255)")
               .IsRequired();

        builder.Property(o => o.Description)
               .HasColumnType(typeName: "nvarchar(500)")
               .IsRequired();

        builder.Property(c => c.Type)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(c => c.UsageType)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(c => c.CalculationType)
               .HasColumnType(typeName: "smallint")
               .IsRequired();

        builder.Property(o => o.CalculationAmount)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(o => o.Amount)
               .HasColumnType(typeName: "decimal")
               .HasPrecision(8, 2)
               .IsRequired();

        builder.Property(c => c.MaxUsage)
               .HasColumnType(typeName: "int")
               .IsRequired();

        builder.Property(c => c.UsageCount)
               .HasColumnType(typeName: "int")
               .HasDefaultValue(0)
               .IsRequired();

        builder.Property(o => o.Code)
               .HasColumnType(typeName: "nvarchar(40)")
               .IsRequired();

        builder.Property(o => o.Name)
               .HasColumnType(typeName: "nvarchar(40)")
               .IsRequired();

        builder.Property(o => o.CreationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired()
               .HasDefaultValueSql(sql: "getdate()");

        builder.Property(o => o.ExpirationDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired();

        #region SEED DATA
        Coupon[] coupons = new[]
        {
            /// Maximum 100 usage
            /// 2 months limited,
            /// Price discount,
            /// 200TRY discount,
            /// Activatable with code,
            new Coupon()
            {
                Id = 1,
                Name = "Coupon_1",
                Description = "Coupon 1 description",
                Type = CouponTypes.Price,
                UsageType = UsageTypes.CodeBased,
                CalculationType = CalculationTypes.Normal,
                CalculationAmount = 0.0M,
                Amount = 200.0M,
                MaxUsage = 100,
                UsageCount = 0,
                Code = DataGenerationExtensions.RandomCode(10),
                ExpirationDate = DateTime.Now.AddMonths(2),
                RowVersion = DataGenerationExtensions.GenerateRandomByteArray()
            },
            /// Maximum 150 usage
            /// 1 month limited,
            /// Percentage discount,
            /// %15 discount,
            /// Activatable with userId (login is required),
            new Coupon()
            {
                Id = 2,
                Name = "Coupon_2",
                Description = "Coupon 2 description",
                Type = CouponTypes.Percentage,
                UsageType = UsageTypes.UserBased,
                CalculationType = CalculationTypes.Normal,
                CalculationAmount = 0.0M,
                Amount = 15.0M,
                MaxUsage = 150,
                UsageCount = 0,
                Code = DataGenerationExtensions.RandomCode(10),
                ExpirationDate = DateTime.Now.AddMonths(1),
                RowVersion = DataGenerationExtensions.GenerateRandomByteArray()
            },
            /// Maximum 200 usage
            /// 3 months limited,
            /// Over price discount with 1000TRY,
            /// 150TRY discount,
            /// Activatable with code,
            new Coupon()
            {
                Id = 3,
                Name = "Coupon_3",
                Description = "Coupon 3 description",
                Type = CouponTypes.OverPrice,
                UsageType = UsageTypes.CodeBased,
                CalculationType = CalculationTypes.OverPrice,
                CalculationAmount = 1000.0M,
                Amount = 150.0M,
                MaxUsage = 200,
                UsageCount = 0,
                Code = DataGenerationExtensions.RandomCode(10),
                ExpirationDate = DateTime.Now.AddMonths(3),
                RowVersion = DataGenerationExtensions.GenerateRandomByteArray()
            },
            /// Maximum 120 usage
            /// 1 month limited,
            /// Over price discount with 2000TRY,
            /// 300TRY discount,
            /// Activatable with code,
            new Coupon()
            {
                Id = 4,
                Name = "Coupon_4",
                Description = "Coupon 4 description",
                Type = CouponTypes.Price,
                UsageType = UsageTypes.CodeBased,
                CalculationType = CalculationTypes.OverPrice,
                CalculationAmount = 2000.0M,
                Amount = 300.0M,
                MaxUsage = 120,
                UsageCount = 0,
                Code = DataGenerationExtensions.RandomCode(10),
                ExpirationDate = DateTime.Now.AddMonths(1),
                RowVersion = DataGenerationExtensions.GenerateRandomByteArray()
            }
        };

        builder.HasData(coupons);
        #endregion
    }
}
