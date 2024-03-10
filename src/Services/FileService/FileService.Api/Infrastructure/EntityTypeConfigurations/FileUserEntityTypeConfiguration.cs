using FileService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Api.Infrastructure.EntityTypeConfigurations;

public class FileUserEntityTypeConfiguration : IEntityTypeConfiguration<FileUser>
{
    public void Configure(EntityTypeBuilder<FileUser> builder)
    {
        builder.ToTable(name: "FileUsers", schema: "file");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .HasColumnType(typeName: "nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Description)
               .HasColumnType(typeName: "nvarchar(500)")
               .IsRequired();

        FileUser[] fileUsers = new[]
        {
            new FileUser()
            {
                Id = 1,
                Name = "fileUser_1",
                Description = "description 1"
            },
            new FileUser()
            {
                Id = 2,
                Name = "fileUser_2",
                Description = "description 2"
            }
        };

        builder.HasData(fileUsers);
    }
}
