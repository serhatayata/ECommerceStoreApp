using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete;

public class DapperBrandRepository : BaseRepository ,IDapperBrandRepository
{
    private readonly ICatalogDbContext _dbContext;
    private readonly ICatalogReadDbConnection _readDbConnection;
    private readonly ICatalogWriteDbConnection _writeDbConnection;

    private readonly string _brandTable;
    private readonly string _productTable;

    public DapperBrandRepository(ICatalogDbContext dbContext, 
                                 ICatalogReadDbConnection readDbConnection, 
                                 ICatalogWriteDbConnection writeDbConnection,
                                 IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _dbContext = dbContext;
        _readDbConnection = readDbConnection;
        _writeDbConnection = writeDbConnection;

        _brandTable = dbContext.GetTableNameWithScheme<Brand>();
        _productTable = dbContext.GetTableNameWithScheme<Product>();
    }

    public async Task<Result> AddAsync(Brand entity)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                //Add brand, with SELECT CAST... we get the added brand's id
                var addQuery = $"INSERT INTO {_brandTable}(Name,Description) VALUES (@Name,@Description);SELECT CAST(SCOPE_IDENTITY() as int)";

                var brandId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                                      transaction: transaction,
                                                                                      param: new { Name = entity.Name, Description = entity.Description });

                if (brandId == 0)
                    return new ErrorResult(this.GetLocalizedValue("dapper.brandrepository.add.notadded"));

                transaction.Commit();
                return new SuccessResult(this.GetLocalizedValue("dapper.brandrepository.add.added"));
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
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();
        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Delete query
            var deleteQuery = $"DELETE FROM {_brandTable} WHERE Id=@Id";
            var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                               transaction: transaction,
                                                               param: new { Id = model.Value });

            transaction.Commit();

            return result > 0 ? 
                new SuccessResult(this.GetLocalizedValue("dapper.brandrepository.add.added")) : 
                new ErrorResult(this.GetLocalizedValue("dapper.brandrepository.add.notadded"));
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

    public async Task<Result> UpdateAsync(Brand entity)
    {
        _dbContext.Connection.Open();
        using var transaction = _dbContext.Connection.BeginTransaction();

        try
        {
            _dbContext.Database.UseTransaction(transaction as DbTransaction);

            //Update query
            var updateQuery = $"UPDATE {_brandTable} " +
                              $"SET Name = @Name, Description = @Description WHERE Id = @Id";

            var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                               transaction: transaction,
                                                               param: new { 
                                                                            Name = entity.Name, 
                                                                            Description = entity.Description, 
                                                                            Id = entity.Id 
                                                                          });

            transaction.Commit();

            return result > 0 ? 
                new SuccessResult(this.GetLocalizedValue("dapper.brandrepository.add.updated")) : 
                new ErrorResult(this.GetLocalizedValue("dapper.brandrepository.add.notupdated"));
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

    public async Task<DataResult<Brand>> GetAsync(IntModel model)
    {
        var query = $"SELECT * FROM {_brandTable} " +
                    $"WHERE Id = @Id";

        var result = await _readDbConnection.QuerySingleOrDefaultAsync<Brand>(sql: query, param: new { Id = model.Value });

        return result == null ? 
            new ErrorDataResult<Brand>(result) : 
            new SuccessDataResult<Brand>(result);
    }

    public  async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_brandTable}";
        var result = await _readDbConnection.QueryAsync<Brand>(sql: query);

        return result == null ? 
            new ErrorDataResult<IReadOnlyList<Brand>> (result) : 
            new SuccessDataResult<IReadOnlyList<Brand>> (result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllPagedAsync(PagingModel model)
    {
        var query = $"SELECT * FROM {_brandTable} " +
                    $"ORDER BY Id DESC OFFSET (@Page-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
        var result = await _readDbConnection.QueryAsync<Brand>(sql: query,
                                                               param: new { Page = model.Page, PageSize = model.PageSize});
        return result == null ?
            new ErrorDataResult<IReadOnlyList<Brand>>(result) :
            new SuccessDataResult<IReadOnlyList<Brand>>(result);
    }

    public async Task<DataResult<IReadOnlyList<Brand>>> GetAllWithProductsAsync()
    {
        var query = $"SELECT b.*, p.Id as ProductId, p.* FROM {_brandTable} b " +
                    $"LEFT OUTER JOIN {_productTable} p ON p.BrandId = b.Id";

        var brandDictionary = new Dictionary<int, Brand>();

        var result = await _dbContext.Connection.QueryAsync<Brand, Product, Brand>(query, (brand, product) =>
        {
            Brand? brandEntry;

            if (!brandDictionary.TryGetValue(brand.Id, out brandEntry))
            {
                brandEntry = brand;
                brandEntry.Products = new List<Product>();
                brandDictionary.Add(brandEntry.Id, brandEntry);
            }
            if (product != null && product.Id > 0)
                brandEntry.Products.Add(product);

            return brandEntry;
        }, splitOn: "ProductId");

        var filteredResult = result.DistinctBy(b => b.Id).ToList();

        return result == null ?
            new ErrorDataResult<IReadOnlyList<Brand>>(result) :
            new SuccessDataResult<IReadOnlyList<Brand>>(result);
    }
}
