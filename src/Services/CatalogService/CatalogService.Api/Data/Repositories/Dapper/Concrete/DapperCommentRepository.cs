using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Encryption;
using CatalogService.Api.Utilities.Results;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Result = CatalogService.Api.Utilities.Results.Result;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete;

public class DapperCommentRepository : IDapperCommentRepository
{
    private readonly ICatalogDbContext _dbContext;
    private readonly ICatalogReadDbConnection _readDbConnection;
    private readonly ICatalogWriteDbConnection _writeDbConnection;
    private readonly ILogger<DapperCommentRepository> _logger;

    private string _commentTable;
    private string _productTable;

    public DapperCommentRepository(
            ICatalogDbContext dbContext, 
            ICatalogReadDbConnection readDbConnection, 
            ICatalogWriteDbConnection writeDbConnection,
            ILogger<DapperCommentRepository> logger)
    {
        _dbContext = dbContext;
        _readDbConnection = readDbConnection;
        _writeDbConnection = writeDbConnection;
        _logger = logger;

        _commentTable = dbContext.GetTableNameWithScheme<Comment>();
        _productTable = dbContext.GetTableNameWithScheme<Product>();
    }

    public async Task<Result> AddAsync(Comment entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //var commentUserQuery = $"SELECT * FROM {_commentTable} WHERE UserId = @UserId AND ProductId = @ProductId";
            //var commentUserExists = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: commentUserQuery, 
            //                                                                                   param: new { UserId = entity.UserId, ProductId = entity.ProductId });
            //if (commentUserExists != null)
            //    return new ErrorResult("User comment for this product already exists");
            ////One comment to current product for a user (Changeable later)
            //string generatedCode = HashCreator.Sha256_Hash(entity.ProductId.ToString(),
            //                                               entity.UserId,
            //                                               DateTime.Now.ToString());

            var addQuery = $"INSERT INTO {_commentTable}" +
                           $"(ProductId,Code,UserId,Content,Name,Surname,Email,UpdateDate) " +
                           $"VALUES (@ProductId,@Code,@UserId,@Content,@Name,@Surname,@Email,@UpdateDate);SELECT CAST(SCOPE_IDENTITY() as int)";

            var commentId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                          transaction: transaction,
                                                                          param: new {
                                                                              ProductId = entity.ProductId,
                                                                              Code = entity.Code,
                                                                              UserId = entity.UserId,
                                                                              Content = entity.Content,
                                                                              UpdateDate = entity.UpdateDate
                                                                          });

            if (commentId == 0)
                return new ErrorResult("Comment not added");

            transaction.Commit();
            return new SuccessResult();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Delete query
            var deleteQuery = $"DELETE FROM {_commentTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = model.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Comment not deleted");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }

    public async Task<Result> DeleteByCodeAsync(StringModel model)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Delete query
            var deleteQuery = $"DELETE FROM {_commentTable} WHERE Code=@Code";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Code = model.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Comment not deleted");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }

    public async Task<Result> UpdateAsync(Comment entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Update query
            var updateQuery = $"UPDATE {_commentTable} " +
                              $"SET Content = @Content " +
                              $"WHERE Id=@Id";

            var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                               transaction: transaction,
                                                               param: new { Content = entity.Content, Id = entity.Id });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Content not updated");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }

    public async Task<Result> UpdateByCodeAsync(Comment entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Update query
            var updateQuery = $"UPDATE {_commentTable} " +
                              $"SET Content = @Content " +
                              $"WHERE Code=@Code";

            var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                               transaction: transaction,
                                                               param: new { Content = entity.Content, Code = entity.Code });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Content not updated");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _dbContext.Connection.Close();
        }
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_commentTable}";

        var result = await _readDbConnection.QueryAsync<Comment>(query);
        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllPagedAsync(PagingModel model)
    {
        var query = $"SELECT * FROM {_commentTable} " +
                    $"ORDER BY Id DESC OFFSET (@Page-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

        var result = await _readDbConnection.QueryAsync<Comment>(sql: query,
                                                                 param: new { Page = model.Page, PageSize = model.PageSize });
        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductId(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE ProductId = @ProductId";

        var result = await _readDbConnection.QueryAsync<Comment>(sql: query,
                                                                 param: new { ProductId = model.Value });

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductCode(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE ProductCode = @ProductCode";

        var result = await _readDbConnection.QueryAsync<Comment>(sql: query,
                                                                 param: new { ProductCode = model.Value });

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllByUserId(StringModel model)
    {
        var query = $"SELECT co.*, p.Id AS ProductId, p.* FROM {_commentTable} co " +
                    $"INNER JOIN {_productTable} p ON p.Id = co.ProductId " +
                    $"WHERE UserId = @UserId";

        var commentDictionary = new Dictionary<int, Comment>();

        var result = await _dbContext.Connection.QueryAsync<Comment, Product, Comment>(query, (comment, product) =>
        {
            Comment? commentEntry;

            if (!commentDictionary.TryGetValue(comment.Id, out commentEntry))
            {
                commentEntry = comment;
                commentDictionary.Add(commentEntry.Id, commentEntry);
            }
            if (product != null && product.Id > 0)
                commentEntry.Product = product;

            return commentEntry;
        }, splitOn: "ProductId", param: new { UserId = model.Value });

        return new DataResult<IReadOnlyList<Comment>>(result.ToList());
    }

    public async Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE Id = @Id";
        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: query, 
                                                                                param: new { Id = model.Value });

        return new DataResult<Comment>(result);
    }

    public async Task<DataResult<Comment>> GetByCodeAsync(StringModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE Code = @Code";
        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: query,
                                                                                param: new { Code = model.Value });

        return new DataResult<Comment>(result);
    }
}
