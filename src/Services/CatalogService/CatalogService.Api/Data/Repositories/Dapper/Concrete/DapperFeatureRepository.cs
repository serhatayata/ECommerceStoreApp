﻿using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Dapper;
using AutoMapper.Features;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            //Check if feature exists
            bool featureExists = await _dbContext.Features.AnyAsync(l => l.Name == entity.Name);
            if (featureExists)
                return new ErrorResult("Feature already exists");

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
            bool featureExists = await _dbContext.Features.AnyAsync(l => l.Id == model.Value);
            if (!featureExists)
                return new ErrorResult("Feature does not exist");

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
            var featureExists = await _dbContext.Features.FirstOrDefaultAsync(l => l.Id == entity.Id);
            if (featureExists == null)
                return new ErrorResult("Feature does not exist");

            bool featureNameExists = await _dbContext.Features.AnyAsync(l => l.Id != entity.Id && l.Name == entity.Name);
            if (featureNameExists)
                return new ErrorResult("Feature name already exists");

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
            var query = $"SELECT * FROM {_productFeatureTable} " +
                        $"WHERE ProductId = @ProductId AND FeatureId = @FeatureId";

            var result = await _readDbConnection
                                    .QuerySingleOrDefaultAsync<ProductFeature>(sql: query,
                                                                              param: new
                                                                              {
                                                                                  ProductId = entity.ProductId,
                                                                                  FeatureId = entity.FeatureId
                                                                              });
            if (result == null)
                return new ErrorResult("Product feature already exists");

            //Add feature, with SELECT CAST... we get the added product feature's id
            var addQuery = $"INSERT INTO {_featureTable}(ProductId,FeatureId) VALUES (@ProductId,@FeatureId);SELECT CAST(SCOPE_IDENTITY() as int)";
            var featureId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                                    transaction: transaction,
                                                                                    param: new
                                                                                    {
                                                                                        ProductId = entity.ProductId,
                                                                                        FeatureId = entity.FeatureId
                                                                                    });
            if (featureId == 0)
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
            var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
                        $"WHERE Id = @Id AND ProductFeatureId = @ProductFeatureId";

            var result = await _readDbConnection
                                    .QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
                                                                              param: new
                                                                              {
                                                                                  Id = entity.Id,
                                                                                  ProductFeatureId = entity.ProductFeatureId
                                                                              });
            if (result == null)
                return new ErrorResult("Product feature already exists");

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
            //Check if product feature exists
            var query = $"SELECT * FROM {_productFeatureTable} " +
                        $"WHERE Id = @Id";

            var getResult = await _readDbConnection
                                    .QuerySingleOrDefaultAsync<ProductFeature>(sql: query,
                                                                              param: new
                                                                              {
                                                                                  Id = entity.Value
                                                                              });

            if (getResult == null)
                return new ErrorResult("Product feature does not exist");

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
            //Check if product feature property exists
            var query = $"SELECT * FROM {_productFeaturePropertyTable} " +
                        $"WHERE Id = @Id";

            var getResult = await _readDbConnection
                                    .QuerySingleOrDefaultAsync<ProductFeatureProperty>(sql: query,
                                                                              param: new
                                                                              {
                                                                                  Id = entity.Value
                                                                              });

            if (getResult == null)
                return new ErrorResult("Product feature property does not exist");

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
        return new DataResult<Feature>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_featureTable}";

        var result = await _readDbConnection.QueryAsync<Feature>(query);
        return new DataResult<IReadOnlyList<Feature>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductId(IntModel model)
    {
        {
            var query = $"SELECT f.*, pf.FeatureId AS FeatureId, pf.* FROM {_featureTable} f " +
                        $"INNER JOIN {_productFeatureTable} pf ON pf.FeatureId = f.Id " +
                        $"WHERE pf.ProductId = @ProductId";

            var result = await _dbContext.Connection.QueryAsync<Feature, ProductFeature, Feature>(query, (feature, productFeature) =>
            {
                return feature;
            }, splitOn: "FeatureId", param: new { ProductId = model.Value });

            var filteredResult = result.DistinctBy(c => c.Id).ToList();
            return new DataResult<IReadOnlyList<Feature>>(filteredResult);
        }
    }

    public async Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesWithPropertiesByProductId(IntModel model)
    {
        var query = $"SELECT f.*, pf.FeatureId AS FeatureId, pf.*, p.Id AS ProductId, p.* FROM {_featureTable} f " +
                    $"INNER JOIN {_productFeatureTable} pf ON pf.FeatureId = f.Id " +
                    $"INNER JOIN {_productTable} p ON p.Id = pf.ProductId " +
                    $"WHERE pf.ProductId = @ProductId";

        var featureDictionary = new Dictionary<int, Feature>();

        var result = await _dbContext.Connection.QueryAsync<Feature, ProductFeature, Product, Feature>(query,
        (feature, productFeature, product) =>
        {
            Feature? featureEntry;

            if (!featureDictionary.TryGetValue(feature.Id, out featureEntry))
            {
                featureEntry = feature;
                featureEntry.ProductFeatures = new List<ProductFeature>();
                featureDictionary.Add(featureEntry.Id, featureEntry);
            }

            if (productFeature != null)
                featureEntry.ProductFeatures.Add(productFeature);

            if (productFeature != null && product != null)
                productFeature.Product = product;

            return featureEntry;
        }, splitOn: "ProductId", param: new { ParentId = model.Value });

        var filteredResult = result.DistinctBy(c => c.Id).ToList();
        return new DataResult<IReadOnlyList<Feature>>(filteredResult);

    }

    public async Task<DataResult<IReadOnlyList<Product>>> GetFeatureProducts(IntModel model)
    {
        var query = $"SELECT p.*, p.ProductId AS ProductId, pf.* FROM {_productTable} p " +
                    $"INNER JOIN {_productFeatureTable} pf ON pf.ProductId = p.Id " +
                    $"WHERE pf.FeatureId = @FeatureId";

        var result = await _dbContext.Connection.QueryAsync<Product, ProductFeature, Product>(query,
        (product, productFeature) =>
        {
            return product;
        }, splitOn: "ProductId", param: new { FeatureId = model.Value });

        var filteredResult = result.ToList();
        return new DataResult<IReadOnlyList<Product>>(filteredResult);
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
        return new DataResult<IReadOnlyList<ProductFeatureProperty>>(filteredResult);
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
        return new DataResult<IReadOnlyList<ProductFeatureProperty>>(filteredResult);
    }
}
