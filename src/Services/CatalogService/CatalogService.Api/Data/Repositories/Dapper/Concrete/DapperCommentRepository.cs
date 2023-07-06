using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Encryption;
using CatalogService.Api.Utilities.Results;
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

            var commentUserQuery = $"SELECT * FROM {_commentTable} WHERE UserId = @UserId AND ProductId = @ProductId";
            var commentUserExists = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: commentUserQuery, 
                                                                                               param: new { UserId = entity.UserId, ProductId = entity.ProductId });
            if (commentUserExists != null)
                return new ErrorResult("User comment for this product already exists");
            //One comment to current product for a user (Changeable later)
            string generatedCode = HashCreator.Sha256_Hash(entity.ProductId.ToString(),
                                                           entity.Name,
                                                           entity.Surname,
                                                           entity.UserId,
                                                           DateTime.Now.ToString());

            var addQuery = $"INSERT INTO {_commentTable}" +
                           $"(ProductId,UserId,Content,Name,Surname,Email) " +
                           $"VALUES (@ProductId,@Code,@UserId,@Content,@Name,@Surname,@Email);SELECT CAST(SCOPE_IDENTITY() as int)";

            var commentId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                          transaction: transaction,
                                                                          param: new {
                                                                              ProductId = entity.ProductId,
                                                                              Code = generatedCode,
                                                                              UserId = entity.UserId,
                                                                              Content = entity.Content,
                                                                              Name = entity.Name,
                                                                              Surname = entity.Surname,
                                                                              Email = entity.Email });

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

            bool commentExists = await _dbContext.Comments.AnyAsync(l => l.Id == model.Value);
            if (!commentExists)
                return new ErrorResult("Comment does not exist");

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

            bool commentExists = await _dbContext.Comments.AnyAsync(l => l.Code == model.Value);
            if (!commentExists)
                return new ErrorResult("Comment does not exist");

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
            var commentExists = await _dbContext.Comments.FirstOrDefaultAsync(l => l.Id == entity.Id);
            if (commentExists == null)
                return new ErrorResult("Comment does not exist");

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
            var commentExists = await _dbContext.Comments.FirstOrDefaultAsync(l => l.Code == entity.Code);
            if (commentExists == null)
                return new ErrorResult("Comment does not exist");

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

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductId(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE ProductId = @ProductId";

        var result = await _readDbConnection.QueryAsync<Comment>(sql: query,
                                                                 param: new { ProductId = model.Value });

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Comment>>> GetAllByUserId(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE UserId = @UserId";
        var result = await _readDbConnection.QueryAsync<Comment>(sql: query,
                                                                 param: new { UserId = model.Value });

        return new DataResult<IReadOnlyList<Comment>>(result);
    }

    public async Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE Id = @Id";
        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: query, 
                                                                                param: new { Id = model.Value });

        return new DataResult<Comment>(result);
    }

    public async Task<DataResult<Comment>> GetByCodeAsync(IntModel model)
    {
        var query = $"SELECT * FROM {_commentTable} WHERE Code = @Code";
        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Comment>(sql: query,
                                                                                param: new { Code = model.Value });

        return new DataResult<Comment>(result);
    }
}
