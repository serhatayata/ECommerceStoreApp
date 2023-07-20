using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCategoryRepository : IEfCategoryRepository
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly ILogger<EfCategoryRepository> _logger;

    public EfCategoryRepository(
        ICatalogDbContext catalogDbContext, 
        ILogger<EfCategoryRepository> logger)
    {
        _catalogDbContext = catalogDbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(Category entity)
    {
        try
        {
            var result = await _catalogDbContext.Categories.AddAsync(entity);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                return new SuccessResult("Category added");
            return new ErrorResult("Category not added");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.AddAsync), "Category not added", ex.Message);
            return new ErrorResult("Category not added");
        }
    }

    public async Task<Result> UpdateAsync(Category entity)
    {
        try
        {
            var result = await _catalogDbContext.Categories.Where(b => b.Id == entity.Id)
                                    .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.ParentId, entity.ParentId)
                                        .SetProperty(p => p.Name, entity.Name)
                                        .SetProperty(p => p.Link, entity.Link)
                                        .SetProperty(p => p.Line, entity.Line)
                                        .SetProperty(p => p.UpdateDate, entity.UpdateDate);

            return result > 0 ?
                new SuccessResult("Category updated") : new ErrorResult("Category not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Category not updated", ex.Message);
            return new ErrorResult("Category not updated");
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        try
        {
            var result = await _catalogDbContext.Categories.Where(b => b.Id == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Category deleted") : new ErrorResult("Category not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteAsync), "Category not deleted", ex.Message);
            return new ErrorResult("Category not deleted");
        }
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
