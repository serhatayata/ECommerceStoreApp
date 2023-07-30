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

            builder.Property(b => b.Name).HasColumnType("nvarchar(300)");

            builder.Property(b => b.Link).HasColumnType("nvarchar(500)");

            builder.Property(b => b.Code).HasColumnType("nvarchar(20)");
            builder.HasIndex(b => b.Code).IsUnique();

            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

            builder.HasOne(b => b.ParentCategory)
                   .WithMany(b => b.SubCategories)
                   .HasForeignKey(b => b.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
