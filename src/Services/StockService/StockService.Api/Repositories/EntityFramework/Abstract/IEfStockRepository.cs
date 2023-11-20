using StockService.Api.Entities;
using StockService.Api.Models.Base;

namespace StockService.Api.Repositories.EntityFramework.Abstract;

public interface IEfStockRepository : IEfGenericRepository<Stock, int>
{
    Task<Utilities.Results.Result> IncreaseCountAsync(int productId, int count);
    Task<Utilities.Results.Result> DecreaseCountAsync(int productId, int count);
    Task<Utilities.Results.Result> DecreaseCountAsync(Dictionary<int, int> prodIdCounts);
}
