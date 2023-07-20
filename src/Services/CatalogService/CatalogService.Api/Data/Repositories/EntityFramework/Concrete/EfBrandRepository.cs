using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfBrandRepository : IEfBrandRepository
{
    private readonly CatalogDbContext _catalogDbContext;
    private ILogger<EfBrandRepository> _logger;

    public EfBrandRepository(
        CatalogDbContext catalogDbContext,
        ILogger<EfBrandRepository> logger)
    {
        _catalogDbContext = catalogDbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(Brand entity)
    {
        try
        {
            var result = await _catalogDbContext.Brands.AddAsync(entity);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                return new SuccessResult("Brand added");
            return new ErrorResult("Brand not added");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.AddAsync), "Brand not added", ex.Message);
            return new ErrorResult("Brand not added");
        }
    }

    public async Task<Result> UpdateAsync(Brand entity)
    {
        try
        {
            var result = await _catalogDbContext.Brands.Where(b => b.Id == entity.Id)
                                    .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.Name, entity.Name)
                                        .SetProperty(p => p.Description, entity.Description));

            return result > 0 ? 
                new SuccessResult("Brand updated") : new ErrorResult("Brand not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Brand not updated", ex.Message);
            return new ErrorResult("Brand not updated");
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        try
        {
            var result = await _catalogDbContext.Brands.Where(b => b.Id == model.Value)
                                                       .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Brand deleted") : new ErrorResult("Brand not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteAsync), "Brand not deleted", ex.Message);
            return new ErrorResult("Brand not deleted");
        }
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
