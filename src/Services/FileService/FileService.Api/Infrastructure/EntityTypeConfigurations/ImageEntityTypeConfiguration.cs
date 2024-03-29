using FileService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Api.Infrastructure.EntityTypeConfigurations;

public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(name: "Images", schema: "file");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.EntityId)
               .HasColumnType(typeName: "nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Path)
               .HasColumnType(typeName: "nvarchar(1000)")
               .IsRequired();

        builder.Property(o => o.CreateDate)
               .HasColumnType(typeName: "datetime2")
               .IsRequired();

        builder.HasOne(ci => ci.FileUser)
               .WithMany(c => c.Images)
               .HasForeignKey(cs => cs.FileUserId)
               .IsRequired(required: false)
               .OnDelete(DeleteBehavior.Cascade);

        Image[] images = new[]
        {
            new Image()
            {
                Id = 1,
                Type = Models.ImageModels.ImageType.Production,
                EntityId= 1,
                CreateDate = DateTime.Now,
                FileUserId = 1,
                Path = "Path_1"
            },
            new Image()
            {
                Id = 2,
                Type = Models.ImageModels.ImageType.Production,
                EntityId= 2,
                CreateDate = DateTime.Now,
                FileUserId = 2,
                Path = "Path_2"
            }
        };

        builder.HasData(images);
    }
}
