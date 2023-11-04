using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using OrderService.Api.Entities;

namespace OrderService.Api.Infrastructure.Contexts;

public interface IOrderDbContext
{
    public IDbConnection Connection { get; }
    DatabaseFacade Database { get; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    string GetTableNameWithScheme<T>() where T : class, IEntity;
}
