using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class ProductFeatureEntityTypeConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.ToTable(name: "ProductFeatures", schema: "product");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasIndex(p => new { p.FeatureId, p.ProductId });

            builder.HasOne(p => p.Product)
                       .WithMany(p => p.ProductFeatures)
                       .HasForeignKey(c => c.ProductId)
                       .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Feature)
                       .WithMany(p => p.ProductFeatures)
                       .HasForeignKey(f => f.FeatureId)
                       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
