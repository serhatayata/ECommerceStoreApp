using Microsoft.EntityFrameworkCore;
using OrderService.Api.Entities;
using OrderService.Api.Infrastructure.EntityTypeConfigurations;
using System.Data;

namespace OrderService.Api.Infrastructure.Contexts;

public class OrderDbContext : DbContext, IOrderDbContext
{
    protected readonly IConfiguration Configuration;

    public OrderDbContext(
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
        builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        builder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

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
