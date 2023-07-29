using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCommentRepository : IEfCommentRepository
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly ILogger<EfCommentRepository> _logger;

    public EfCommentRepository(
        ICatalogDbContext catalogDbContext, 
        ILogger<EfCommentRepository> logger)
    {
        _catalogDbContext = catalogDbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(Comment entity)
    {
        try
        {
            var result = await _catalogDbContext.Comments.AddAsync(entity);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                return new SuccessResult("Comment added");
            return new ErrorResult("Comment not added");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.AddAsync), "Comment not added", ex.Message);
            return new ErrorResult("Comment not added");
        }
    }

    public async Task<Result> UpdateAsync(Comment entity)
    {
        try
        {
            var result = await _catalogDbContext.Comments.Where(b => b.Id == entity.Id)
                                    .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.Content, entity.Content));

            return result > 0 ?
                new SuccessResult("Comment updated") : new ErrorResult("Comment not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Comment not updated", ex.Message);
            return new ErrorResult("Comment not updated");
        }
    }

    public async Task<Result> UpdateByCodeAsync(Comment entity)
    {
        try
        {
            var result = await _catalogDbContext.Comments.Where(b => b.Code == entity.Code)
                                    .ExecuteUpdateAsync(b => b
                                        .SetProperty(p => p.Content, entity.Content));

            return result > 0 ?
                new SuccessResult("Comment updated") : new ErrorResult("Comment not updated");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.UpdateAsync), "Comment not updated", ex.Message);
            return new ErrorResult("Comment not updated");
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        try
        {
            var result = await _catalogDbContext.Comments.Where(b => b.Id == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Comment deleted") : new ErrorResult("Comment not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteAsync), "Comment not deleted", ex.Message);
            return new ErrorResult("Comment not deleted");
        }
    }

    public async Task<Result> DeleteByCodeAsync(StringModel model)
    {
        try
        {
            var result = await _catalogDbContext.Comments.Where(b => b.Code == model.Value)
                                                            .ExecuteDeleteAsync();

            return result > 0 ?
                new SuccessResult("Comment deleted") : new ErrorResult("Comment not deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("{0} - {1} - Exception : {2}", nameof(this.DeleteByCodeAsync), "Comment not deleted", ex.Message);
            return new ErrorResult("Comment not deleted");
        }
    }

    public async Task<DataResult<Comment>> GetAsync(Expression<Func<Comment, bool>> predicate)
    {
        var result = await _catalogDbContext.Comments.FirstOrDefaultAsync(predicate);

        return new DataResult<Comment>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync(Expression<Func<Comment, bool>> predicate)
    {
        var result = await _catalogDbContext.Comments.Where(predicate).ToListAsync();

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync()
    {
        var result = await _catalogDbContext.Comments.ToListAsync();

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        var result = await _catalogDbContext.Comments.FirstOrDefaultAsync(p => p.Id == model.Value);

        return new DataResult<Comment>(result);
    }
}
