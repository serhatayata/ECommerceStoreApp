using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCommentRepository : IEfCommentRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfCommentRepository(
        CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Result> AddAsync(Comment entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.Comments.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Comment not added");

                transaction.Commit();
                return new SuccessResult("Comment added");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _catalogDbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> UpdateAsync(Comment entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Comments.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.Content, entity.Content));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Comment not updated");

                transaction.Commit();
                return new SuccessResult("Comment updated");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _catalogDbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> UpdateByCodeAsync(Comment entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Comments.Where(b => b.Code == entity.Code)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.Content, entity.Content));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Comment not updated");

                transaction.Commit();
                return new SuccessResult("Comment updated");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _catalogDbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Comments.Where(b => b.Id == model.Value)
                                                           .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Comment not deleted");

                transaction.Commit();
                return new SuccessResult("Comment deleted");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _catalogDbContext.Connection.Close();
            }
        }
    }

    public async Task<Result> DeleteByCodeAsync(StringModel model)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Comments.Where(b => b.Code == model.Value)
                                                             .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Comment not deleted");

                transaction.Commit();
                return new SuccessResult("Comment deleted");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _catalogDbContext.Connection.Close();
            }
        }
    }

    public async Task<DataResult<Comment>> GetAsync(Expression<Func<Comment, bool>> predicate)
    {
        var result = await _catalogDbContext.Comments.FirstOrDefaultAsync(predicate);

        return result == null ?
            new ErrorDataResult<Comment>(result) :
            new SuccessDataResult<Comment>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync(Expression<Func<Comment, bool>> predicate)
    {
        var result = await _catalogDbContext.Comments.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Comment>>(result) :
            new SuccessDataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Comments.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Comment>>(result) :
            new SuccessDataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Comments.FirstOrDefaultAsync(p => p.Id == model.Value);

        return result == null ?
            new ErrorDataResult<Comment>(result) :
            new SuccessDataResult<Comment>(result);
    }
}
