using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfProductRepository : IEfProductRepository
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly ILogger<EfProductRepository> _logger;

    public EfProductRepository(
        ICatalogDbContext catalogDbContext, 
        ILogger<EfProductRepository> logger)
    {
        _catalogDbContext = catalogDbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(Product entity)
    {
        try
        {
            var result = await _catalogDbContext.Products.AddAsync(entity);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                return new SuccessResult("Product added");
            return new ErrorResult("Product not added");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.AddAsync), "Product not added", ex.Message);
            return new ErrorResult("Product not added");
        }
    }

    public async Task<Result> UpdateAsync(Product entity)
    {
        try
        {
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

            return result > 0 ?
                new SuccessResult("Product updated") : new ErrorResult("Product not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Product not updated", ex.Message);
            return new ErrorResult("Product not updated");
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        try
        {
            var result = await _catalogDbContext.Products.Where(b => b.Id == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Product deleted") : new ErrorResult("Product not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteAsync), "Product not deleted", ex.Message);
            return new ErrorResult("Product not deleted");
        }
    }

    public async Task<Result> DeleteByCodeAsync(StringModel model)
    {
        try
        {
            var result = await _catalogDbContext.Products.Where(b => b.ProductCode == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Product deleted") : new ErrorResult("Product not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteByCodeAsync), "Product not deleted", ex.Message);
            return new ErrorResult("Product not deleted");
        }
    }

    public async Task<DataResult<Product>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(p => p.Id == model.Value);

        return new DataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetByProductCodeAsync(StringModel model)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == model.Value);

        return new DataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetAsync(Expression<Func<Product, bool>> predicate)
    {
        var result = await _catalogDbContext.Products.FirstOrDefaultAsync(predicate);

        return new DataResult<Product>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        var result = await _catalogDbContext.Products.Where(predicate).ToListAsync();

        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Products.ToListAsync();

        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandIdAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products
                                            .Where(p => p.BrandId == model.Value)
                                            .ToListAsync();

        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeIdAsync(IntModel model)
    {
        var result = await _catalogDbContext.Products
                                            .Where(p => p.ProductTypeId == model.Value)
                                            .ToListAsync();

        return new DataResult<IReadOnlyList<Product>>(result);
    }
}
