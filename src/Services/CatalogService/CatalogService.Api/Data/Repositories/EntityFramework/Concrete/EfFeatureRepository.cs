using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfFeatureRepository : IEfFeatureRepository
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly ILogger<EfFeatureRepository> _logger;

    public EfFeatureRepository(
        ICatalogDbContext catalogDbContext, 
        ILogger<EfFeatureRepository> logger)
    {
        _catalogDbContext = catalogDbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(Feature entity)
    {
        try
        {
            var result = await _catalogDbContext.Features.AddAsync(entity);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                return new SuccessResult("Feature added");
            return new ErrorResult("Feature not added");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.AddAsync), "Feature not added", ex.Message);
            return new ErrorResult("Feature not added");
        }
    }

    public async Task<Result> UpdateAsync(Feature entity)
    {
        try
        {
            var result = await _catalogDbContext.Features.Where(b => b.Id == entity.Id)
                                    .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.Name, entity.Name));

            return result > 0 ?
                new SuccessResult("Feature updated") : new ErrorResult("Feature not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Feature not updated", ex.Message);
            return new ErrorResult("Feature not updated");
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        try
        {
            var result = await _catalogDbContext.Features.Where(b => b.Id == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Feature deleted") : new ErrorResult("Feature not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteAsync), "Feature not deleted", ex.Message);
            return new ErrorResult("Feature not deleted");
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

        return new DataResult<Feature>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync(Expression<Func<Feature, bool>> predicate)
    {
        var result = await _catalogDbContext.Features.Where(predicate).ToListAsync();

        return new DataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Features.ToListAsync();

        return new DataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllWithProductFeaturesAsync()
    {
        var result = await _catalogDbContext.Features
                                    .Include(f => f.ProductFeatures)
                                    .ThenInclude(pf => pf.Product)
                                    .AsNoTracking()
                                    .ToListAsync();

        return new DataResult<IReadOnlyList<Feature>>(result);
    }
}
