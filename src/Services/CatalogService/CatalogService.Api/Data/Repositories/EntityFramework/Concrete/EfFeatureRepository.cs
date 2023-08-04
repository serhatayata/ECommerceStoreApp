using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfFeatureRepository : IEfFeatureRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfFeatureRepository(
        CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Result> AddAsync(Feature entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.Features.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Feature not added");

                transaction.Commit();
                return new SuccessResult("Feature added");
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

    public async Task<Result> AddProductFeatureAsync(ProductFeature entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.ProductFeatures.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product feature not added");

                transaction.Commit();
                return new SuccessResult("Product feature added");
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

    public async Task<Result> AddProductFeaturePropertyAsync(ProductFeatureProperty entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                await _catalogDbContext.ProductFeatureProperties.AddAsync(entity);

                var result = _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product feature property not added");

                transaction.Commit();
                return new SuccessResult("Product feature property added");
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

    public async Task<Result> UpdateAsync(Feature entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.Features.Where(b => b.Id == entity.Id)
                                                             .ExecuteUpdateAsync(b => b
                                                             .SetProperty(p => p.Name, entity.Name));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Feature not updated");

                transaction.Commit();
                return new SuccessResult("Feature updated");
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

    public async Task<Result> UpdateProductFeaturePropertyAsync(ProductFeatureProperty entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.ProductFeatureProperties.Where(b => b.Id == entity.Id)
                                                                             .ExecuteUpdateAsync(b => b
                                                                             .SetProperty(p => p.Name, entity.Name)
                                                                             .SetProperty(p => p.Description, entity.Description));

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Feature not updated");

                transaction.Commit();
                return new SuccessResult("Feature updated");
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

                var result = await _catalogDbContext.Features.Where(b => b.Id == model.Value)
                                                             .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Feature not deleted");

                transaction.Commit();
                return new SuccessResult("Feature deleted");
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

    public async Task<Result> DeleteProductFeatureAsync(ProductFeatureModel entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.ProductFeatures.Where(b => b.ProductId == entity.ProductId && 
                                                                                b.FeatureId == entity.FeatureId)
                                                                                .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product feature not deleted");

                transaction.Commit();
                return new SuccessResult("Product feature deleted");
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

    public async Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity)
    {
        _catalogDbContext.Connection.Open();
        using (var transaction = _catalogDbContext.Connection.BeginTransaction())
        {
            try
            {
                _catalogDbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _catalogDbContext.ProductFeatureProperties.Where(b => b.Id == entity.Value)
                                                                             .ExecuteDeleteAsync();

                _catalogDbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Product feature property not deleted");

                transaction.Commit();
                return new SuccessResult("Product feature property deleted");
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

    public async Task<DataResult<Feature>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Features.FirstOrDefaultAsync(f => f.Id == model.Value);

        return new DataResult<Feature>(result);
    }

    public async Task<DataResult<Feature>> GetAsync(Expression<Func<Feature, bool>> predicate)
    {
        var result = await _catalogDbContext.Features.FirstOrDefaultAsync(predicate);

        return result == null ?
            new ErrorDataResult<Feature>(result) :
            new SuccessDataResult<Feature>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync(Expression<Func<Feature, bool>> predicate)
    {
        var result = await _catalogDbContext.Features.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Features.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllWithProductFeaturesAsync()
    {
        var result = await _catalogDbContext.Features
                                    .Include(f => f.ProductFeatures)
                                    .ThenInclude(pf => pf.Product)
                                    .AsNoTracking()
                                    .ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }
}
