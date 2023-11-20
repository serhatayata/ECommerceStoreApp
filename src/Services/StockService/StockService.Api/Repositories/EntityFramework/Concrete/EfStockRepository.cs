using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using StockService.Api.Entities;
using StockService.Api.Infrastructure.Contexts;
using StockService.Api.Repositories.EntityFramework.Abstract;
using StockService.Api.Utilities.Results;
using System.Data.Common;
using System.Linq.Expressions;

namespace StockService.Api.Repositories.EntityFramework.Concrete;

public class EfStockRepository : IEfStockRepository
{
    private readonly StockDbContext _dbContext;

    public EfStockRepository(
        StockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataResult<int>> AddAsync(Stock entity)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                await _dbContext.Stocks.AddAsync(entity);

                var result = _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorDataResult<int>(default(int), "error for adding");

                transaction.Commit();
                return new SuccessDataResult<int>(entity.Id, "error for adding");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> UpdateAsync(Stock entity)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.Stocks.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.Count, entity.Count));

                _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("error updating");

                transaction.Commit();
                return new SuccessResult("success updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> UpdateAsync(List<Stock> models)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                foreach (var stock in models)
                {
                    await _dbContext.Stocks.Where(b => b.Id == stock.Id)
                                           .ExecuteUpdateAsync(b => b
                                           .SetProperty(p => p.Count, stock.Count));
                }

                _dbContext.SaveChanges();

                transaction.Commit();
                return new SuccessResult("success updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> DecreaseCountAsync(int productId, int count)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.Stocks.Where(b => b.ProductId == productId)
                                                    .ExecuteUpdateAsync(b => b
                                                    .SetProperty(p => p.Count, p => p.Count - count));

                _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("error updating");

                transaction.Commit();
                return new SuccessResult("success updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> IncreaseCountAsync(int productId, int count)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.Stocks.Where(b => b.ProductId == productId)
                                                    .ExecuteUpdateAsync(b => b
                                                    .SetProperty(p => p.Count, p => p.Count + count));

                _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("error updating");

                transaction.Commit();
                return new SuccessResult("success updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> DecreaseCountAsync(Dictionary<int, int> prodIdCounts)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                foreach (var pdc in prodIdCounts)
                {
                    await _dbContext.Stocks.Where(b => b.ProductId == pdc.Key)
                                           .ExecuteUpdateAsync(b => b
                                           .SetProperty(p => p.Count, p => p.Count - pdc.Value));
                }

                _dbContext.SaveChanges();

                transaction.Commit();
                return new SuccessResult("success updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.Stocks.Where(b => b.Id == id)
                                                    .ExecuteDeleteAsync();

                _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("deleting error");

                transaction.Commit();
                return new SuccessResult("deleting error");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<DataResult<Stock>> GetAsync(Expression<Func<Stock, bool>> predicate)
    {
        var result = await _dbContext.Stocks.FirstOrDefaultAsync(predicate);
        return result == null ?
            new ErrorDataResult<Stock>(result) :
            new SuccessDataResult<Stock>(result);
    }

    public async Task<DataResult<IReadOnlyList<Stock>>> GetAllAsync(Expression<Func<Stock, bool>> predicate)
    {
        var result = await _dbContext.Stocks.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Stock>>(result) :
            new SuccessDataResult<IReadOnlyList<Stock>>(result);
    }

    public async Task<DataResult<Stock>> GetAsync(int id)
    {
        var result = await _dbContext.Stocks.FirstOrDefaultAsync(b => b.Id == id);

        return result == null ?
            new ErrorDataResult<Stock>(result) :
            new SuccessDataResult<Stock>(result);
    }

    public async Task<DataResult<IReadOnlyList<Stock>>> GetAllAsync()
    {
        var result = await _dbContext.Stocks.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Stock>>(result) :
            new SuccessDataResult<IReadOnlyList<Stock>>(result);
    }
}
