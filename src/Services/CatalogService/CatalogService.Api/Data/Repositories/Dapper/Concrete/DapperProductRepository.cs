using AutoMapper.Features;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;
using Dapper;
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
    private string _commentTable;
    private string _featureTable;
    private string _productFeatureTable;

    public DapperProductRepository(ICatalogDbContext dbContext, 
                                   ICatalogReadDbConnection readDbConnection, 
                                   ICatalogWriteDbConnection writeDbConnection)
    {
        _dbContext = dbContext;
        _readDbConnection = readDbConnection;
        _writeDbConnection = writeDbConnection;

        _productTable = dbContext.GetTableNameWithScheme<Product>();
        _commentTable = dbContext.GetTableNameWithScheme<Comment>();
        _featureTable = dbContext.GetTableNameWithScheme<Feature>();
        _productFeatureTable = dbContext.GetTableNameWithScheme<ProductFeature>();
    }

    public async Task<Result> AddAsync(Product entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

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
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

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
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            //Update query
            var updateQuery = $"UPDATE {_productTable} " +
                              $"SET Name = @Name, Description = @Description, Price = @Price, " +
                              $"AvailableStock = @AvailableStock, Link = @Link, ProductTypeId = @ProductTypeId " +
                              $"BrandId = @BrandId, UpdateDate = @UpdateDate " +
                              $"WHERE Id=@Id";

            var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                               transaction: transaction,
                                                               param: new
                                                               {
                                                                   Id = entity.Id,
                                                                   Name = entity.Name,
                                                                   Description = entity.Description,
                                                                   Price = entity.Price,
                                                                   AvailableStock = entity.AvailableStock,
                                                                   Link = entity.Link,
                                                                   ProductTypeId = entity.ProductTypeId,
                                                                   BrandId = entity.BrandId,
                                                                   UpdateDate = entity.UpdateDate
                                                               });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Product not updated");
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

    public async Task<DataResult<Product>> GetAsync(IntModel model)
    {
        var query = $"SELECT TOP 1 * FROM {_productTable} WHERE Id = @Id";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Product>(sql: query,
                                                                                param: new { Id = model.Value });
        return new DataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetByProductCode(StringModel model)
    {
        var query = $"SELECT TOP 1 * FROM {_productTable} WHERE ProductCode = @ProductCode";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Product>(sql: query,
                                                                                param: new { ProductCode = model.Value });
        return new DataResult<Product>(result);
    }

    public async Task<DataResult<Product>> GetWithComments(IntModel model)
    {
        var query = $"SELECT p.*, c.Id AS CommentId, c.* FROM {_productTable} p " +
                    $"INNER JOIN {_commentTable} c ON c.ProductId = p.Id " +
                    $"WHERE p.Id = @Id";

        var productDictionary = new Dictionary<int, Product>();

        var result = await _dbContext.Connection.QueryAsync<Product, Comment, Product>(query, (product, comment) =>
        {
            Product? productEntry;

            if (!productDictionary.TryGetValue(product.Id, out productEntry))
            {
                productEntry = product;
                productEntry.Comments = new List<Comment>();
                productDictionary.Add(product.Id, productEntry);
            }
            if (comment != null)
                productEntry.Comments.Add(comment);

            return productEntry;
        }, splitOn: "CommentId", param: new { Id = model.Value });

        var filteredResult = result.DistinctBy(c => c.Id).FirstOrDefault();
        return new DataResult<Product>(filteredResult);
    }

    public async Task<DataResult<Product>> GetWithFeatures(IntModel model)
    {
        var query = $"SELECT p.*, f.Id AS FeatureId ,f.Name, FROM {_productTable} " +
                    $"INNER JOIN {_productFeatureTable} pf ON pf.ProductId = p.Id " +
                    $"INNER JOIN {_featureTable} f ON f.Id = pf.FeatureId " +
                    $"WHERE p.Id = @Id";

        var productDictionary = new Dictionary<int, Product>();

        var result = await _dbContext.Connection.QueryAsync<Product, Feature, Product>(query, (product, feature) =>
        {
            Product? productEntry;

            if (!productDictionary.TryGetValue(product.Id, out productEntry))
            {
                productEntry = product;
                productEntry.ProductFeatures = new List<ProductFeature>();
                productDictionary.Add(productEntry.Id, productEntry);
            }

            if (feature != null)
                product.ProductFeatures.Add(new ProductFeature()
                {
                    Feature = feature,
                    FeatureId = feature.Id,
                    ProductId = product.Id
                });

            return productEntry;
        }, splitOn: "FeatureId", param: new { Id = model.Value });

        var filteredResult = result.DistinctBy(c => c.Id).FirstOrDefault();
        return new DataResult<Product>(filteredResult);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_productTable}";

        var result = await _readDbConnection.QueryAsync<Product>(query);
        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllBetweenPrices(PriceBetweenModel model)
    {
        var query = $"SELECT * FROM {_productTable} WHERE Price >= @MinimumPrice AND Price <= @MaximumPrice";

        var result = await _readDbConnection.QueryAsync<Product>(sql : query,
                                                                 param : new { 
                                                                                MinimumPrice = model.MinimumPrice,
                                                                                MaximumPrice = model.MaximumPrice,
                                                                             });
        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandId(IntModel model)
    {
        var query = $"SELECT * FROM {_productTable} WHERE BrandId = @BrandId";

        var result = await _readDbConnection.QueryAsync<Product>(sql: query,
                                                                 param: new
                                                                 {
                                                                     BrandId = model.Value
                                                                 });
        return new DataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeId(IntModel model)
    {
        var query = $"SELECT * FROM {_productTable} WHERE ProductTypeId = @ProductTypeId";

        var result = await _readDbConnection.QueryAsync<Product>(sql: query,
                                                                 param: new
                                                                 {
                                                                     ProductTypeId = model.Value
                                                                 });
        return new DataResult<IReadOnlyList<Product>>(result);
    }
}
