using StockService.Api.Models.StockModels;
using StockService.Api.Utilities.Results;

namespace StockService.Api.Services.Abstract;

public interface IStockService
{
    Task<DataResult<int>> AddAsync(StockAddModel model);
    Task<Result> UpdateAsync(StockUpdateModel model);
    Task<Result> UpdateAsync(List<StockUpdateModel> models);
    Task<Result> DecreaseStockAsync(int productId, int count);
    Task<Result> IncreaseStockAsync(int productId, int count);
    Task<Result> DeleteAsync(int id);

    Task<DataResult<StockModel>> GetAsync(int id);
    Task<DataResult<StockModel>> GetByProductIdAsync(int productId);
    Task<DataResult<int>> GetStockCountByProductIdAsync(int productId);
    Task<DataResult<IReadOnlyList<StockModel>>> GetAllAsync();
}
