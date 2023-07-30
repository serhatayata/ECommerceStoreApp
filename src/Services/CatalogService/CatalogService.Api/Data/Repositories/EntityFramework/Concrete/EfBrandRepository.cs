using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfBrandRepository : IEfBrandRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfBrandRepository(
        CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Result> AddAsync(Brand entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.Brands.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Brand not added");

                transaction.Commit();
                return new SuccessResult("Brand added");
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

    public async Task<Result> UpdateAsync(Brand entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Brands.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.Name, entity.Name)
                                            .SetProperty(b => b.Description, entity.Description));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Brand not updated");

                transaction.Commit();
                return new SuccessResult("Brand updated");
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
                    return new ErrorResult("Brand not deleted");

                transaction.Commit();
                return new SuccessResult("Brand deleted");
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

    public async Task<DataResult<Brand>> GetAsync(Expression<Func<Brand, bool>> predicate)
    {
        var result = await _catalogDbContext.Brands.FirstOrDefaultAsync(predicate);

        return new DataResult<Brand>(result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync(Expression<Func<Brand, bool>> predicate)
    {
        var result = await _catalogDbContext.Brands.Where(predicate).ToListAsync();

        return new DataResult<IReadOnlyList<Brand>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Brands.ToListAsync();

        return new DataResult<IReadOnlyList<Brand>>(result);
    }

    public async Task<DataResult<Brand>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Brands.FirstOrDefaultAsync(b => b.Id == model.Value);

        return new DataResult<Brand>(result);
    }

}
