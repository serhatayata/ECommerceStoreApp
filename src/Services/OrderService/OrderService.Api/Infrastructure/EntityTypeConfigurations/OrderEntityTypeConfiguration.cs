using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Api.Entities;

namespace OrderService.Api.Infrastructure.EntityTypeConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(name: "Orders", schema: "order");

        builder.HasKey(o => o.Id);

        builder.HasIndex(o => o.Code).IsUnique();

        builder.Property(o => o.CreatedDate).HasColumnType(typeName: "datetime2");
        builder.Property(o => o.CreatedDate).HasDefaultValueSql(sql: "getdate()");

        builder.Property(o => o.BuyerId).HasColumnType(typeName: "nvarchar(40)");
        builder.Property(o => o.BuyerId).IsRequired(required: true);
                             
        builder.Property(o => o.Status).HasColumnType(typeName: "smallint");
        builder.Property(o => o.Status).IsRequired(required: true);
                             
        builder.Property(o => o.FailMessage).IsRequired(required: false);
        builder.Property(o => o.FailMessage).HasColumnType(typeName: "nvarchar(300)");
    }
}
