using Microsoft.EntityFrameworkCore;
using OrderService.Api.Entities;
using OrderService.Api.Infrastructure.Contexts;
using OrderService.Api.Models.Base;
using OrderService.Api.Repositories.EntityFramework.Abstract;
using OrderService.Api.Utilities.Results;
using System;
using System.Data.Common;
using System.Linq.Expressions;

namespace OrderService.Api.Repositories.EntityFramework.Concrete;

public class EfOrderRepository : IEfOrderRepository
{
    private readonly OrderDbContext _context;

    public EfOrderRepository(
        OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddAsync(Order entity)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.Orders.AddAsync(entity);

                var result = _context.SaveChanges();

                if (result < 1)
                    return new ErrorResult("error for adding");

                transaction.Commit();
                return new SuccessResult("error for adding");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<Result> UpdateAsync(Order entity)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.Orders.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.Status, entity.Status)
                                            .SetProperty(b => b.FailMessage, entity.FailMessage)
                                            .SetProperty(b => b.Address, entity.Address));

                _context.SaveChanges();

                if (result < 1)
                    return new ErrorResult("error updating");

                transaction.Commit();
                return new SuccessResult("error updating");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.Orders.Where(b => b.Id == model.Value)
                                                  .ExecuteDeleteAsync();

                _context.SaveChanges();

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
                _context.Connection.Close();
            }
        }
    }

    public async Task<DataResult<Order>> GetAsync(Expression<Func<Order, bool>> predicate)
    {
        var result = await _context.Orders.FirstOrDefaultAsync(predicate);

        return result == null ?
            new ErrorDataResult<Order>(result) :
            new SuccessDataResult<Order>(result);
    }

    public async Task<DataResult<IReadOnlyList<Order>>> GetAllAsync(Expression<Func<Order, bool>> predicate)
    {
        var result = await _context.Orders.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Order>>(result) :
            new SuccessDataResult<IReadOnlyList<Order>>(result);
    }

    public async Task<DataResult<Order>> GetAsync(IntModel model)
    {
        var result = await _context.Orders.FirstOrDefaultAsync(b => b.Id == model.Value);

        return result == null ?
            new ErrorDataResult<Order>(result) :
            new SuccessDataResult<Order>(result);
    }

    public async Task<DataResult<IReadOnlyList<Order>>> GetAllAsync()
    {
        var result = await _context.Orders.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Order>>(result) :
            new SuccessDataResult<IReadOnlyList<Order>>(result);
    }
}
