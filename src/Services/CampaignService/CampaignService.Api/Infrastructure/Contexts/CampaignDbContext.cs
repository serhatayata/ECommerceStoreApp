using Microsoft.EntityFrameworkCore;
using System.Data;
using CampaignService.Api.Entities;

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
        options.UseSqlServer(Configuration.GetConnectionString("OrderDB"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
    }

    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<CampaignEntity> CampaignEntities { get; set; }
    public DbSet<CampaignItem> CampaignItems { get; set; }
    public DbSet<CampaignRule> CampaignRules { get; set; }

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
