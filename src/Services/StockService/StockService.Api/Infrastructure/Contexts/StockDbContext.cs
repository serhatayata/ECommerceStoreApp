using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockService.Api.Entities;
using StockService.Api.Infrastructure.EntityTypeConfigurations;
using System.Data;

namespace StockService.Api.Infrastructure.Contexts;

public class StockDbContext: DbContext, IStockDbContext
{
    private readonly IConfiguration _configuration;

    public StockDbContext(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlserver with connection string from app settings
        options.UseSqlServer(_configuration.GetConnectionString("StockDb"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new StockEntityTypeConfiguration());
    }

    public DbSet<Stock> Stocks { get; set; }

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
