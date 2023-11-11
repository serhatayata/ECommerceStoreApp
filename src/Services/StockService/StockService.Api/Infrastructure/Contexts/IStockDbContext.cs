using MassTransit.Transports;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using StockService.Api.Entities;

namespace StockService.Api.Infrastructure.Contexts;

public interface IStockDbContext
{
    public IDbConnection Connection { get; }
    DatabaseFacade Database { get; }
    public DbSet<Stock> Stocks { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    string GetTableNameWithScheme<T>() where T : class, IEntity;
}
