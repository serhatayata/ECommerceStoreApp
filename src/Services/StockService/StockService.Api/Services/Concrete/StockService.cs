using AutoMapper;
using MassTransit.Initializers;
using StockService.Api.Entities;
using StockService.Api.Models.StockModels;
using StockService.Api.Repositories.EntityFramework.Abstract;
using StockService.Api.Services.Abstract;
using StockService.Api.Utilities.Results;

namespace StockService.Api.Services.Concrete;

public class StockService : IStockService
{
    private readonly IEfStockRepository _efStockRepository;
    private readonly IMapper _mapper;

    public StockService(
        IEfStockRepository efStockRepository,
        IMapper mapper)
    {
        _efStockRepository = efStockRepository;
        _mapper = mapper;
    }

    public async Task<DataResult<int>> AddAsync(StockAddModel model)
    {
        var addModel = _mapper.Map<Stock>(model);
        return await _efStockRepository.AddAsync(addModel);
    }

    public async Task<Result> UpdateAsync(List<StockUpdateModel> models)
    {
        var updateModel = _mapper.Map<List<Stock>>(models);
        return await _efStockRepository.UpdateAsync(updateModel);
    }

    public async Task<Result> UpdateAsync(StockUpdateModel model)
    {
        var updateModel = _mapper.Map<Stock>(model);
        return await _efStockRepository.UpdateAsync(updateModel);
    }

    public async Task<Result> DecreaseStockAsync(int productId, int count)
    {
        return await _efStockRepository.DecreaseCountAsync(productId, count);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        return await _efStockRepository.DeleteAsync(id);
    }

    public async Task<DataResult<IReadOnlyList<StockModel>>> GetAllAsync()
    {
        var allStocks = await _efStockRepository.GetAllAsync();
        var result = _mapper.Map<IReadOnlyList<StockModel>>(allStocks.Data);
        return new DataResult<IReadOnlyList<StockModel>>(result);
    }

    public async Task<DataResult<StockModel>> GetAsync(int id)
    {
        var stock = await _efStockRepository.GetAsync(id);
        var result = _mapper.Map<StockModel>(stock.Data);
        return new DataResult<StockModel>(result);
    }

    public async Task<DataResult<StockModel>> GetByProductIdAsync(int productId)
    {
        var stock = await _efStockRepository.GetAsync(s => s.ProductId == productId);
        var result = _mapper.Map<StockModel>(stock.Data);
        return new DataResult<StockModel>(result);
    }

    public async Task<DataResult<int>> GetStockCountByProductIdAsync(int productId)
    {
        var stock = await _efStockRepository.GetAsync(s => s.ProductId == productId);
        return new DataResult<int>(stock.Data?.Count ?? default(int));
    }
}
