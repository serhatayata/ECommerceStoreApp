using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCategoryRepository : IEfCategoryRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfCategoryRepository(
        CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Result> AddAsync(Category entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.Categories.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Category not added");

                transaction.Commit();
                return new SuccessResult("Category added");
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

    public async Task<Result> UpdateAsync(Category entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Categories.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.ParentId, entity.ParentId)
                                            .SetProperty(p => p.Name, entity.Name)
                                            .SetProperty(p => p.Link, entity.Link)
                                            .SetProperty(p => p.Line, entity.Line)
                                            .SetProperty(p => p.UpdateDate, entity.UpdateDate));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Category not updated");

                transaction.Commit();
                return new SuccessResult("Category updated");
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

                var result = await _catalogDbContext.Brands.Where(b => b.Id == model.Value)
                                                           .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Category not deleted");

                transaction.Commit();
                return new SuccessResult("Category deleted");
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

    public async Task<DataResult<Category>> GetAsync(Expression<Func<Category, bool>> predicate)
    {
        var result = await _catalogDbContext.Categories.FirstOrDefaultAsync(predicate);

        return new DataResult<Category>(result);
    }

    public async Task<DataResult<IReadOnlyList<Category>>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        var result = await _catalogDbContext.Categories.Where(predicate).ToListAsync();

        return new DataResult<IReadOnlyList<Category>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Category>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Categories.ToListAsync();

        return new DataResult<IReadOnlyList<Category>>(result);
    }

    public async Task<DataResult<Category>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Categories.FirstOrDefaultAsync(c => c.Id == model.Value);

        return new DataResult<Category>(result);
    }
}
