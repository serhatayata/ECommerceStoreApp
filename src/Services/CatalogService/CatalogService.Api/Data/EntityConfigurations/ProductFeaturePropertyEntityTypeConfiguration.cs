using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class ProductFeaturePropertyEntityTypeConfiguration : IEntityTypeConfiguration<ProductFeatureProperty>
    {
        public void Configure(EntityTypeBuilder<ProductFeatureProperty> builder)
        {
            builder.ToTable(name: "ProductFeatureProperties", schema: "product");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Name).HasColumnType("nvarchar(100)");

            builder.Property(b => b.Description).HasColumnType("nvarchar(500)");
        }
    }
}
