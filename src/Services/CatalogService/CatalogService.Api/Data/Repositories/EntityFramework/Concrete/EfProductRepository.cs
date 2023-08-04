using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfProductRepository : IEfProductRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfProductRepository(
        CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Result> AddAsync(Product entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.Products.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product not added");

                transaction.Commit();
                return new SuccessResult("Product added");
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

    public async Task<Result> UpdateAsync(Product entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Products.Where(b => b.Id == entity.Id)
                                        .ExecuteUpdateAsync(b => b
                                            .SetProperty(p => p.Name, entity.Name)
                                            .SetProperty(p => p.Description, entity.Description)
                                            .SetProperty(p => p.Price, entity.Price)
                                            .SetProperty(p => p.AvailableStock, entity.AvailableStock)
                                            .SetProperty(p => p.Link, entity.Link)
                                            .SetProperty(p => p.ProductCode, entity.ProductCode)
                                            .SetProperty(p => p.ProductTypeId, entity.ProductTypeId)
                                            .SetProperty(p => p.BrandId, entity.BrandId)
                                            .SetProperty(p => p.UpdateDate, entity.UpdateDate));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product not updated");

                transaction.Commit();
                return new SuccessResult("Product updated");
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

                var result = await _catalogDbContext.Products.Where(b => b.Id == model.Value)
                                                           .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product not deleted");

                transaction.Commit();
                return new SuccessResult("Product deleted");
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

                var result = await _catalogDbContext.Products.Where(b => b.ProductCode == model.Value)
                                                                .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product not deleted");

                transaction.Commit();
                return new SuccessResult("Product deleted");
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

    public async Task<DataResult<Product>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(p => p.Id == model.Value);

        return result == null ?
            new ErrorDataResult<Product>(result) :
            new SuccessDataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetByProductCodeAsync(StringModel model)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == model.Value);

        return result == null ?
            new ErrorDataResult<Product>(result) :
            new SuccessDataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetAsync(Expression<Func<Product, bool>> predicate)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(predicate);

        return result == null ?
            new ErrorDataResult<Product>(result) :
            new SuccessDataResult<Product>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        var result = await _catalogDbContext.Products.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Product>>(result) :
            new SuccessDataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Products.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Product>>(result) :
            new SuccessDataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandIdAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products
                                            .Where(p => p.BrandId == model.Value)
                                            .ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Product>>(result) :
            new SuccessDataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeIdAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products
                                            .Where(p => p.ProductTypeId == model.Value)
                                            .ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Product>>(result) :
            new SuccessDataResult<IReadOnlyList<Product>>(result);
    }
}
