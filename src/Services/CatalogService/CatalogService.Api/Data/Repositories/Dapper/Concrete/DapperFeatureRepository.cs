using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Utilities.Results;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Result = CatalogService.Api.Utilities.Results.Result;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete;

public class DapperFeatureRepository : IDapperFeatureRepository
{
    private readonly ICatalogDbContext _dbContext;
    private readonly ICatalogReadDbConnection _readDbConnection;
    private readonly ICatalogWriteDbConnection _writeDbConnection;

    private string _featureTable;
    private string _productTable;
    private string _productFeatureTable;
    private string _productFeaturePropertyTable;

    public DapperFeatureRepository(ICatalogDbContext dbContext,
                                   ICatalogReadDbConnection readDbConnection,
                                   ICatalogWriteDbConnection writeDbConnection)
    {
        _dbContext = dbContext;
        _readDbConnection = readDbConnection;
        _writeDbConnection = writeDbConnection;

        _featureTable = dbContext.GetTableNameWithScheme<Feature>();
        _productTable = dbContext.GetTableNameWithScheme<Product>();
        _productFeatureTable = dbContext.GetTableNameWithScheme<ProductFeature>();
        _productFeaturePropertyTable = dbContext.GetTableNameWithScheme<ProductFeatureProperty>();
    }

    public async Task<Result> AddAsync(Feature entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Add feature, with SELECT CAST... we get the added feature's id
            var addQuery = $"INSERT INTO {_featureTable}(Name) VALUES (@Name);SELECT CAST(SCOPE_IDENTITY() as int)";
            var featureId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                          transaction: transaction,
                                                                          param: new
                                                                          {
                                                                              Name = entity.Name
                                                                          });
            if (featureId == 0)
                return new ErrorResult("Feature not added");

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
            var deleteQuery = $"DELETE FROM {_featureTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = model.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Feature not deleted");
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

    public async Task<Result> UpdateAsync(Feature entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Update query
            var updateQuery = $"UPDATE {_featureTable} " +
                              $"SET Name = @Name " +
                              $"WHERE Id=@Id";

            var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                               transaction: transaction,
                                                               param: new
                                                               {
                                                                   Id = entity.Id,
                                                                   Name = entity.Name,
                                                               });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Member not added");
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

    public async Task<Result> AddProductFeatureAsync(ProductFeature entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);
            //Check if product feature exists
            //var query = $"SELECT * FROM {_productFeatureTable} " +
            //            $"WHERE ProductId = @ProductId AND FeatureId = @FeatureId";

            //var result = await _readDbConnection
            //                        .QuerySingleOrDefaultAsync<ProductFeature>(sql: query,
            //                                                                  param: new
            //                                                                  {
            //                                                                      ProductId = entity.ProductId,
            //                                                                      FeatureId = entity.FeatureId
            //                                                                  });
            //if (result == null)
            //    return new ErrorResult("Product feature already exists");

            //Add feature, with SELECT CAST... we get the added product feature's id
            var addQuery = $"INSERT INTO {_productFeatureTable}(ProductId,FeatureId) VALUES (@ProductId,@FeatureId);SELECT CAST(SCOPE_IDENTITY() as int)";
            var productFeatureId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                                    transaction: transaction,
                                                                                    param: new
                                                                                    {
                                                                                        ProductId = entity.ProductId,
                                                                                        FeatureId = entity.FeatureId
                                                                                    });
            if (productFeatureId == 0)
                return new ErrorResult("Product feature not added");

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

    public async Task<Result> AddProductFeaturePropertyAsync(ProductFeatureProperty entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);
            //Check if product feature property exists
            //var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
            //            $"WHERE Id = @Id AND ProductFeatureId = @ProductFeatureId";

            //var result = await _readDbConnection
            //                        .QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
            //                                                                  param: new
            //                                                                  {
            //                                                                      Id = entity.Id,
            //                                                                      ProductFeatureId = entity.ProductFeatureId
            //                                                                  });
            //if (result == null)
            //    return new ErrorResult("Product feature already exists");

            //Add feature, with SELECT CAST... we get the added product feature property's id
            var addQuery = $"INSERT INTO {_productFeaturePropertyTable}(ProductFeatureId,Name,Description) " +
                           $"VALUES (@ProductFeatureId,@Name,@Description);" +
                           $"SELECT CAST(SCOPE_IDENTITY() as int)";

            var productFeaturePropertyId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                                    transaction: transaction,
                                                                                    param: new
                                                                                    {
                                                                                        ProductFeatureId = entity.ProductFeatureId,
                                                                                        Name = entity.Name,
                                                                                        Description = entity.Description
                                                                                    });
            if (productFeaturePropertyId == 0)
                return new ErrorResult("Product feature property not added");

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

    public async Task<Result> DeleteProductFeatureAsync(IntModel entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Check if product feature exists
            //var query = $"SELECT * FROM {_productFeatureTable} " +
            //            $"WHERE Id = @Id";

            //var getResult = await _readDbConnection
            //                        .QuerySingleOrDefaultAsync<ProductFeature>(sql: query,
            //                                                                  param: new
            //                                                                  {
            //                                                                      Id = entity.Value
            //                                                                  });

            //if (getResult == null)
            //    return new ErrorResult("Product feature does not exist");

            //Delete query
            var deleteQuery = $"DELETE FROM {_productFeatureTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = entity.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Product feature not deleted");
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

    public async Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Check if product feature property exists
            //var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
            //            $"WHERE Id = @Id";

            //var getResult = await _readDbConnection
            //                        .QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
            //                                                                  param: new
            //                                                                  {
            //                                                                      Id = entity.Value
            //                                                                  });

            //if (getResult == null)
            //    return new ErrorResult("Product feature property does not exist");

            //Delete query
            var deleteQuery = $"DELETE FROM {_productFeaturePropertyTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = entity.Value });

            transaction.Commit();

            return result > 0 ? new SuccessResult() : new ErrorResult("Product feature property not deleted");
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

    public async Task<DataResult<Feature>> GetAsync(IntModel model)
    {
        var query = $"SELECT * FROM {_featureTable} WHERE Id = @Id";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Feature>(sql: query,
                                                                                param: new { Id = model.Value });

        return result == null ?
            new ErrorDataResult<Feature>(result) :
            new SuccessDataResult<Feature>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_featureTable}";

        var result = await _readDbConnection.QueryAsync<Feature>(query);

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllPagedAsync(PagingModel model)
    {
        var query = $"SELECT * FROM {_featureTable} " +
                    $"ORDER BY Id DESC OFFSET (@Page-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

        var result = await _readDbConnection.QueryAsync<Feature>(sql: query,
                                                                 param: new { Page = model.Page, PageSize = model.PageSize });

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductId(IntModel model)
    {
        var query = $"SELECT f.*, " +
                    $"pf.Id as ProductFeatureId, pf.FeatureId, pf.ProductId, " +
                    $"p.Id AS PrdId , p.* FROM {_featureTable} f " +
                    $"LEFT OUTER JOIN {_productFeatureTable} pf ON pf.FeatureId = f.Id " +
                    $"LEFT OUTER JOIN {_productTable} p ON p.Id = pf.ProductId " +
                    $"WHERE p.Id = @Id";

        var featureDictionary = new Dictionary<int, Feature>();

        var result = await _dbContext.Connection.QueryAsync<Feature, ProductFeature, Product, Feature>(query, (feature, productFeature, product) =>
        {
            Feature? featureEntry;

            if (!featureDictionary.TryGetValue(feature.Id, out featureEntry))
            {
                featureEntry = feature;
                featureEntry.ProductFeatures = new List<ProductFeature>();
                featureEntry.ProductFeatures.Add(new ProductFeature()
                {
                    Product = product,
                    FeatureId = feature.Id,
                    ProductId = product.Id
                }); 
                featureDictionary.Add(featureEntry.Id, featureEntry);
            }
            else
            {
                var isPfpExists = featureDictionary.First(f => f.Key == feature.Id).Value.ProductFeatures.Any(s => s.Id == productFeature.Id);
                if (!isPfpExists)
                    featureDictionary.First(f => f.Key == feature.Id).Value.ProductFeatures.Add(new ProductFeature()
                    {
                        Product = product,
                        FeatureId = feature.Id,
                        ProductId = product.Id
                    });
            }                

            return featureEntry;
        }, splitOn: "ProductFeatureId,PrdId", param: new { Id = model.Value });

        var filteredResult = result.DistinctBy(c => c.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductCode(StringModel model)
    {
        var query = $"SELECT f.*, " +
                    $"pf.Id as ProductFeatureId, pf.FeatureId, pf.ProductId, " +
                    $"p.Id AS PrdId , p.* FROM {_featureTable} f " +
                    $"LEFT OUTER JOIN {_productFeatureTable} pf ON pf.FeatureId = f.Id " +
                    $"LEFT OUTER JOIN {_productTable} p ON p.Id = pf.ProductId " +
                    $"WHERE p.ProductCode = @ProductCode";

        var featureDictionary = new Dictionary<int, Feature>();

        var result = await _dbContext.Connection.QueryAsync<Feature, ProductFeature, Product, Feature>(query, (feature, productFeature, product) =>
        {
            Feature? featureEntry;

            if (!featureDictionary.TryGetValue(feature.Id, out featureEntry))
            {
                featureEntry = feature;
                featureEntry.ProductFeatures = new List<ProductFeature>();
                featureEntry.ProductFeatures.Add(new ProductFeature()
                {
                    Product = product,
                    Feature = feature,
                    FeatureId = feature.Id,
                    ProductId = product.Id
                });
                featureDictionary.Add(featureEntry.Id, featureEntry);
            }
            else
            {
                var isPfpExists = featureDictionary.First(f => f.Key == feature.Id).Value.ProductFeatures.Any(s => s.Id == productFeature.Id);
                if (!isPfpExists)
                    featureDictionary.First(f => f.Key == feature.Id).Value.ProductFeatures.Add(new ProductFeature()
                    {
                        Product = product,
                        FeatureId = feature.Id,
                        ProductId = product.Id
                    });
            }

            return featureEntry;
        }, splitOn: "ProductFeatureId,PrdId", param: new { ProductCode = model.Value });

        var filteredResult = result.DistinctBy(c => c.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Feature>>(result) :
            new SuccessDataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetFeatureProducts(IntModel model)
    {
        var query = $"SELECT p.*, pf.Id AS ProductFeatureId, pf.* FROM {_productTable} p " +
                    $"INNER JOIN {_productFeatureTable} pf ON pf.ProductId = p.Id " +
                    $"WHERE pf.FeatureId = @FeatureId";

        var productDictionary = new Dictionary<int, Product>();

        var result = await _dbContext.Connection.QueryAsync<Product, ProductFeature, Product>(query,
        (product, productFeature) =>
        {
            Product? productEntry;

            if (!productDictionary.TryGetValue(product.Id, out productEntry))
            {
                productEntry = product;
                productEntry.ProductFeatures = new List<ProductFeature>();
                productDictionary.Add(productEntry.Id, productEntry);
            }
            if (productFeature != null && productFeature.Id > 0)
                productEntry.ProductFeatures.Add(productFeature);

            return productEntry;
        }, splitOn: "ProductFeatureId", param: new { FeatureId = model.Value });

        var filteredResult = result.DistinctBy(f => f.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Product>>(result) :
            new SuccessDataResult<IReadOnlyList<Product>>(result);
    }

    public async Task<DataResult<IReadOnlyList<ProductFeatureProperty>>> GetAllFeatureProperties(int featureId, int productId)
    {
        var query = $"SELECT pf.*, pfp.Id AS ProductFeaturePropertyId, pfp.* FROM {_productFeatureTable} pf " +
                    $"INNER JOIN {_productFeaturePropertyTable} pfp ON pfp.ProductFeatureId = pf.Id " +
                    $"WHERE pf.FeatureId = @FeatureId AND pf.ProductId = @ProductId";

        var featureDictionary = new Dictionary<int, ProductFeature>();

        var result = await _dbContext.Connection.QueryAsync<ProductFeature, ProductFeatureProperty, ProductFeature>(query, (productFeature, productFeatureProperty) =>
        {
            ProductFeature? productFeatureEntry;

            if (!featureDictionary.TryGetValue(productFeature.Id, out productFeatureEntry))
            {
                productFeatureEntry = productFeature;
                productFeatureEntry.ProductFeatureProperties = new List<ProductFeatureProperty>();
                featureDictionary.Add(productFeatureEntry.Id, productFeatureEntry);
            }
            if (productFeatureProperty != null)
                productFeatureEntry.ProductFeatureProperties.Add(productFeatureProperty);

            return productFeatureEntry;
        }, splitOn: "ProductFeaturePropertyId", param: new { FeatureId = featureId, ProductId = productId });

        var resultData = result.FirstOrDefault();
        if (resultData == null)
            return new DataResult<IReadOnlyList<ProductFeatureProperty>>(new List<ProductFeatureProperty>());

        var filteredResult = resultData.ProductFeatureProperties.DistinctBy(c => c.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<ProductFeatureProperty>>(result) :
            new SuccessDataResult<IReadOnlyList<ProductFeatureProperty>>(result);
    }

    public async Task<DataResult<IReadOnlyList<ProductFeatureProperty>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model)
    {
        var query = $"SELECT pf.*, pfp.Id AS ProductFeaturePropertyId, pfp.* FROM {_productFeatureTable} pf " +
                    $"INNER JOIN {_productFeaturePropertyTable} pfp ON pfp.ProductFeatureId = pf.Id " +
                    $"WHERE pf.Id = @ProductFeatureId";

        var featureDictionary = new Dictionary<int, ProductFeature>();

        var result = await _dbContext.Connection.QueryAsync<ProductFeature, ProductFeatureProperty, ProductFeature>(query, (productFeature, productFeatureProperty) =>
        {
            ProductFeature? productFeatureEntry;

            if (!featureDictionary.TryGetValue(productFeature.Id, out productFeatureEntry))
            {
                productFeatureEntry = productFeature;
                productFeatureEntry.ProductFeatureProperties = new List<ProductFeatureProperty>();
                featureDictionary.Add(productFeatureEntry.Id, productFeatureEntry);
            }
            if (productFeatureProperty != null)
                productFeatureEntry.ProductFeatureProperties.Add(productFeatureProperty);

            return productFeatureEntry;
        }, splitOn: "ProductFeaturePropertyId", param: new { ProductFeatureId = model.Value });

        var resultData = result.FirstOrDefault();
        if (resultData == null)
            return new DataResult<IReadOnlyList<ProductFeatureProperty>>(new List<ProductFeatureProperty>());

        var filteredResult = resultData.ProductFeatureProperties.DistinctBy(c => c.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<ProductFeatureProperty>>(result) :
            new SuccessDataResult<IReadOnlyList<ProductFeatureProperty>>(result);
    }

    public async Task<DataResult<ProductFeature>> GetProductFeature(ProductFeatureModel model)
    {
        var query = $"SELECT * FROM {_productFeatureTable} " +
                    $"WHERE ProductId = @ProductId " +
                    $"AND FeatureId = @FeatureId";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<ProductFeature>(sql: query,
                                                                                param: new { ProductId = model.ProductId, FeatureId = model.FeatureId });

        return result == null ?
            new ErrorDataResult<ProductFeature>(result) :
            new SuccessDataResult<ProductFeature>(result);
    }

    public async Task<DataResult<ProductFeatureProperty>> GetProductFeatureProperty(int productFeatureId, string name)
    {
        var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
                    $"WHERE ProductFeatureId = @ProductFeatureId " +
                    $"AND Name = @Name";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
                                                                                param: new { ProductFeatureId = productFeatureId, Name = name });

        return result == null ?
            new ErrorDataResult<ProductFeatureProperty>(result) :
            new SuccessDataResult<ProductFeatureProperty>(result);
    }

    public async Task<DataResult<ProductFeatureProperty>> GetProductFeatureProperty(IntModel productFeaturePropertyId)
    {
        var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
                    $"WHERE Id = @Id";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
                                                                                param: new { Id = productFeaturePropertyId.Value });

        return result == null ?
            new ErrorDataResult<ProductFeatureProperty>(result) :
            new SuccessDataResult<ProductFeatureProperty>(result);
    }
}
