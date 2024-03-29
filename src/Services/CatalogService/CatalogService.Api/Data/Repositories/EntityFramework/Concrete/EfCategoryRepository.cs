﻿using CatalogService.Api.Data.Contexts;
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

public class EfCategoryRepository : BaseRepository, IEfCategoryRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public EfCategoryRepository(
        CatalogDbContext catalogDbContext,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
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
                    return new ErrorResult(this.GetLocalizedValue("ef.categoryrepository.add.notadded"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.categoryrepository.add.added"));
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
                    return new ErrorResult(this.GetLocalizedValue("ef.categoryrepository.update.notupdated"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.categoryrepository.update.updated"));
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
                    return new ErrorResult(this.GetLocalizedValue("ef.categoryrepository.delete.notdeleted"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("ef.categoryrepository.delete.deleted"));
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

        return result == null ?
            new ErrorDataResult<Category>(result) :
            new SuccessDataResult<Category>(result);
    }

    public async Task<DataResult<IReadOnlyList<Category>>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        var result = await _catalogDbContext.Categories.Where(predicate).ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Category>>(result) :
            new SuccessDataResult<IReadOnlyList<Category>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Category>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Categories.ToListAsync();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Category>>(result) :
            new SuccessDataResult<IReadOnlyList<Category>>(result);
    }

    public async Task<DataResult<Category>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Categories.FirstOrDefaultAsync(c => c.Id == model.Value);

        return result == null ?
            new ErrorDataResult<Category>(result) :
            new SuccessDataResult<Category>(result);
    }
}
