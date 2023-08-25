using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfBrandRepository : BaseRepository, IEfBrandRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfBrandRepository(
        CatalogDbContext catalogDbContext,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
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
                    return new ErrorResult(this.GetLocalizedValue("ef.brandpository.add.notadded"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.brandpository.add.added"));
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
                    return new ErrorResult(this.GetLocalizedValue("ef.brandpository.update.notupdated"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.brandpository.update.updated"));
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
                    return new ErrorResult(this.GetLocalizedValue("ef.brandpository.delete.notdeleted"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.brandpository.delete.deleted"));
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

        return result == null ?
            new ErrorDataResult<Brand>(result) :
            new SuccessDataResult<Brand>(result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync(Expression<Func<Brand, bool>> predicate)
    {
        var result = await _catalogDbContext.Brands.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Brand>>(result) :
            new SuccessDataResult<IReadOnlyList<Brand>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Brands.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Brand>>(result) :
            new SuccessDataResult<IReadOnlyList<Brand>>(result);
    }

    public async Task<DataResult<Brand>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Brands.FirstOrDefaultAsync(b => b.Id == model.Value);

        return result == null ?
            new ErrorDataResult<Brand>(result) :
            new SuccessDataResult<Brand>(result);
    }

}
