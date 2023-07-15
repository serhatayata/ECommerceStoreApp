using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(name: "Categories", schema: "category");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Name).HasColumnType("nvarchar(10)");

            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

            builder.HasOne(b => b.ParentCategory)
                   .WithMany(b => b.SubCategories)
                   .HasForeignKey(b => b.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
