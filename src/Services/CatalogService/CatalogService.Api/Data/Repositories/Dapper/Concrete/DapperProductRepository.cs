using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Data.Common;
using Result = CatalogService.Api.Utilities.Results.Result;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete;

public class DapperProductRepository : IDapperProductRepository
{
    private readonly ICatalogDbContext _dbContext;
    private readonly ICatalogReadDbConnection _readDbConnection;
    private readonly ICatalogWriteDbConnection _writeDbConnection;

    private string _productTable;

    public DapperProductRepository(ICatalogDbContext dbContext, 
                                   ICatalogReadDbConnection readDbConnection, 
                                   ICatalogWriteDbConnection writeDbConnection)
    {
        _dbContext = dbContext;
        _readDbConnection = readDbConnection;
        _writeDbConnection = writeDbConnection;

        _productTable = dbContext.GetTableNameWithScheme<Product>();
    }

    public async Task<Result> AddAsync(Product entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);
            //Check if product exists
            var productCodeGenerated = Guid.NewGuid().ToString();
            var productQuery = $"SELECT TOP 1 * FROM {_productTable} WHERE ProductCode = @ProductCode";
            var productExists = await _readDbConnection.QuerySingleOrDefaultAsync<Product>(sql: productQuery,
                                                                                           param: new { ProductCode = productCodeGenerated });

            if (productExists != null)
                return new ErrorResult("Product code exists, please try again");

            //Add feature, with SELECT CAST... we get the added feature's id
            var addQuery = $"INSERT INTO {_productTable}(Name, Description, Price, AvailableStock, Link, ProductCode, ProductTypeId, BrandId, CreateDate, UpdateDate) " +
                           $"VALUES (@Name, @Description, @Price, @AvailableStock, @Link, @ProductCode, @ProductTypeId, @BrandId, DEFAULT, @UpdateDate);SELECT CAST(SCOPE_IDENTITY() as int)";
            var productId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                          transaction: transaction,
                                                                          param: new
                                                                          {
                                                                              Name = entity.Name,
                                                                              Description = entity.Description,
                                                                              Price = entity.Price,
                                                                              AvailableStock = entity.AvailableStock,
                                                                              Link = entity.Link,
                                                                              ProductCode = entity.ProductCode,
                                                                              ProductTypeId = entity.ProductTypeId,
                                                                              BrandId = entity.BrandId,
                                                                              UpdateDate = entity.UpdateDate
                                                                          });
            if (productId == 0)
                return new ErrorResult("Product not added");

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
            var productQuery = $"SELECT TOP 1 * FROM {_productTable} WHERE Id = @Id";
            var productExists = await _readDbConnection.QuerySingleOrDefaultAsync<Product>(sql: productQuery,
                                                                                           param: new { Id = model.Value });

            if (productExists != null)
                return new ErrorResult("Product does not exist");

            //Delete query
            var deleteQuery = $"DELETE FROM {_productTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = model.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Product not deleted");
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

    public async Task<Result> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllBetweenPrices(PriceModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<Product>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<Product>> GetByProductCode(StringModel model)
    {
        throw new NotImplementedException();
    }
}
