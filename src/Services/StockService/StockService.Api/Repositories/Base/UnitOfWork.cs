using StockService.Api.Repositories.EntityFramework.Abstract;

namespace StockService.Api.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly IEfStockRepository _stockRepository;

    public UnitOfWork(
        IEfStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
}
