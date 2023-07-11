using CatalogService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Data.EntityConfigurations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(name: "Comments", schema: "product");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Code).HasColumnType("nvarchar(100)");

            builder.Property(b => b.ProductId).IsRequired(true);

            builder.Property(b => b.UserId).IsRequired(false);

            builder.Property(b => b.Content).HasColumnType("nvarchar(MAX)");

            builder.Property(b => b.Name).IsRequired(false);
            builder.Property(b => b.Name).HasColumnType("nvarchar(100)");

            builder.Property(b => b.Surname).IsRequired(false);
            builder.Property(b => b.Surname).HasColumnType("nvarchar(100)");

            builder.Property(b => b.Email).IsRequired(false);
            builder.Property(b => b.Email).HasColumnType("nvarchar(300)");

            builder.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

            builder.HasOne(c => c.Product)
                       .WithMany(p => p.Comments)
                       .HasForeignKey(c => c.ProductId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
