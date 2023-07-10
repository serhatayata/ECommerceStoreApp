﻿using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(name: "Products", schema: "product");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Name).HasColumnType("nvarchar(250)");

            builder.Property(b => b.Description).HasColumnType("nvarchar(1000)");

            builder.Property(b => b.Price).HasPrecision(12, 4);

            builder.Property(b => b.Link).HasColumnType("nvarchar(320)");

            builder.Property(b => b.ProductCode).HasColumnType("nvarchar(100)");

            builder.Property(b => b.ProductTypeId).IsRequired(true);

            builder.Property(b => b.BrandId).IsRequired(false);

            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

            builder.HasOne(c => c.Brand)
                       .WithMany(p => p.Products)
                       .HasForeignKey(c => c.BrandId)
                       .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.ProductType)
                       .WithMany(p => p.Products)
                       .HasForeignKey(c => c.ProductTypeId)
                       .OnDelete(DeleteBehavior.SetNull);
        }
    }
}