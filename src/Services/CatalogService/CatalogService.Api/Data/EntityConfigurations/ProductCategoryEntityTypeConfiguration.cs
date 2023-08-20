using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable(name: "ProductCategories", schema: "product");

            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.HasOne<Product>(p => p.Product)
                   .WithMany(pc => pc.ProductCategories)
                   .HasForeignKey(p => p.ProductId);

            builder.HasOne<Category>(c => c.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey(c => c.CategoryId);

        }
    }
}
