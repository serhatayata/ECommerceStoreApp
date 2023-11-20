using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Api.Entities;

namespace OrderService.Api.Infrastructure.EntityTypeConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(name: "OrderItems", schema: "order");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.ProductId).HasColumnType(typeName: "int");

        builder.Property(o => o.Price).HasPrecision(12, 4);

        builder.Property(o => o.Count).HasColumnType(typeName: "int");

        builder.HasOne(r => r.Order)
               .WithMany(l => l.Items)
               .HasForeignKey(rl => rl.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
