using Microsoft.EntityFrameworkCore;
using System.Data;
using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.EntityTypeConfigurations;

namespace CampaignService.Api.Infrastructure.Contexts;

public class CampaignDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public CampaignDbContext(
        IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlserver with connection string from app settings
        options.UseSqlServer(Configuration.GetConnectionString("CampaignDB"));
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CampaignEntityTypeConfiguration());
        builder.ApplyConfiguration(new CampaignSourceEntityTypeConfiguration());
        builder.ApplyConfiguration(new CampaignItemEntityTypeConfiguration());

        base.OnModelCreating(builder);
    }

    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<CampaignSource> CampaignSources { get; set; }
    public DbSet<CampaignItem> CampaignItems { get; set; }

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
