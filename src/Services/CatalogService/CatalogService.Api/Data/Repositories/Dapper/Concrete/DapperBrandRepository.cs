using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete
{
    public class DapperBrandRepository : IDapperBrandRepository
    {
        private readonly ICatalogDbContext _dbContext;
        private readonly ICatalogReadDbConnection _readDbConnection;
        private readonly ICatalogWriteDbConnection _writeDbConnection;

        private readonly string _brandTable;
        private readonly string _productTable;

        public DapperBrandRepository(ICatalogDbContext dbContext, 
                                     ICatalogReadDbConnection readDbConnection, 
                                     ICatalogWriteDbConnection writeDbConnection)
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
                        return new ErrorResult("Brand not added");

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

                return result > 0 ? new SuccessResult() : new ErrorResult("Brand not added");
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

                return result > 0 ? new SuccessResult() : new ErrorResult("Brand not updated");
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
            var query = $"SELECT b.*, p.Id AS ProductId, p.* FROM {_brandTable} b " +
                        $"INNER JOIN {_productTable} p ON p.BrandId = b.Id " +
                        $"WHERE b.Id = @Id";

            var brandDictionary = new Dictionary<int, Brand>();

            var result = await _dbContext.Connection.QueryAsync<Brand, Product, Brand>(query, (brand, product) =>
            {
                Brand? brandEntry;

                if (!brandDictionary.TryGetValue(brand.Id, out brandEntry))
                {
                    brandEntry = brand;
                    brandEntry.Products = new List<Product>();
                    brandDictionary.Add(brand.Id, brandEntry);
                }
                if (product != null)
                    brandEntry.Products.Add(product);

                return brandEntry;
            }, splitOn: "ProductId", param: new { Id = model.Value });

            var filteredResult = result.DistinctBy(c => c.Id).FirstOrDefault();
            return new DataResult<Brand>(filteredResult);
        }

        public  async Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_brandTable}";
            var result = await _readDbConnection.QueryAsync<Brand>(sql: query);
            return new DataResult<IReadOnlyList<Brand>>(result);
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
            return new DataResult<IReadOnlyList<Brand>>(filteredResult);
        }
    }
}
