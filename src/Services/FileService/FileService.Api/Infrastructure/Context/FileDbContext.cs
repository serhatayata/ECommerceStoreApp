using FileService.Api.Entities;
using FileService.Api.Infrastructure.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FileService.Api.Infrastructure.Context;

public class FileDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public FileDbContext(
        IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlserver with connection string from app settings
        options.UseSqlServer(Configuration.GetConnectionString("FileDB"));
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ImageEntityTypeConfiguration());
        builder.ApplyConfiguration(new FileUserEntityTypeConfiguration());

        base.OnModelCreating(builder);
    }

    public DbSet<Image> Images { get; set; }
    public DbSet<FileUser> FileUsers { get; set; }

    public string GetTableNameWithScheme<T>() where T : class, IEntity
    {
        var entityType = this.Model.FindEntityType(typeof(T));
        var schema = entityType?.GetSchema();
        var tableName = entityType?.GetTableName();
        if (schema == null)
            return entityType?.GetTableName() ?? string.Empty;

        return $"{schema}.{tableName}";
    }
}
