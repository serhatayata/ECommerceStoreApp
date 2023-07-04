using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Encryption;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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

            var commentUserQuery = $"SELECT * FROM {_commentTable} WHERE UserId = @UserId";
            var commentUserExists = await _readDbConnection.QuerySingleOrDefaultAsync<int>(sql: commentUserQuery, param: new { UserId = entity.UserId });
            if (commentUserExists != 0)
                return new ErrorResult("User comment for this product already exists");

            string generatedCode = HashCreator.Sha256_Hash(entity.ProductId.ToString(),
                                                           entity.Name,
                                                           entity.Surname,
                                                           entity.UserId);

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

    public Task<Result> DeleteByCodeAsync(StringModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Comment entity)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Comment>>> GetAllByUserId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }
}
