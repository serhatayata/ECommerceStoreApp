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
    public class DapperCategoryRepository : IDapperCategoryRepository
    {
        private readonly ICatalogDbContext _dbContext;
        private readonly ICatalogReadDbConnection _readDbConnection;
        private readonly ICatalogWriteDbConnection _writeDbConnection;

        private string _categoryTable;
        private string _productTable;

        public DapperCategoryRepository(ICatalogDbContext dbContext, 
                                        ICatalogReadDbConnection readDbConnection, 
                                        ICatalogWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;

            _categoryTable = dbContext.GetTableNameWithScheme<Category>();
            _productTable = dbContext.GetTableNameWithScheme<Product>();
        }

        public async Task<Result> AddAsync(Category entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);
                //Check if category exists
                bool categoryExists = await _dbContext.Categories.AnyAsync(l => l.Name == entity.Name);
                if (categoryExists)
                    return new ErrorResult("Category already exists");

                //Add category, with SELECT CAST... we get the added category's id
                var addQuery = $"INSERT INTO {_categoryTable}(ParentId,Name,Link,Line) VALUES (@ParentId,@Name,@Link,@Line);SELECT CAST(SCOPE_IDENTITY() as int)";
                var categoryId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                              transaction: transaction,
                                                                              param: new { ParentId = entity.ParentId,
                                                                                           Name = entity.Name,
                                                                                           Link = entity.Link,
                                                                                           Line = entity.Line });
                if (categoryId == 0)
                    return new ErrorResult("Category not added");

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
                bool categoryExists = await _dbContext.Categories.AnyAsync(l => l.Id == model.Value);
                if (!categoryExists)
                    return new ErrorResult("Category does not exist");

                //Delete query
                var deleteQuery = $"DELETE FROM {_categoryTable} WHERE Id=@Id";
                var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                                   transaction: transaction,
                                                                   param: new { Id = model.Value });

                transaction.Commit();

                return result > 0 ? new SuccessResult() : new ErrorResult("Member not deleted");
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

        public async Task<Result> UpdateAsync(Category entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();

            try
            {
                var categoryExists = await _dbContext.Categories.FirstOrDefaultAsync(l => l.Id == entity.Id);
                if (categoryExists == null)
                    return new ErrorResult("Category does not exist");

                bool categoryNameExists = await _dbContext.Categories.AnyAsync(l => l.Id != entity.Id && l.Name == entity.Name);
                if (categoryNameExists)
                    return new ErrorResult("Category name already exists");

                //Update query
                var updateQuery = $"UPDATE {_categoryTable} " +
                                  $"SET Name = @Name, ParentId = @ParentId, Line = @Line " +
                                  $"WHERE Id=@Id";

                var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                                   transaction: transaction,
                                                                   param: new
                                                                   {
                                                                       Id = entity.Id,
                                                                       Name = entity.Name,
                                                                       ParentId = entity.ParentId,
                                                                       Line = entity.Line
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

        public async Task<DataResult<IReadOnlyList<Category>>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_categoryTable}";

            var result = await _readDbConnection.QueryAsync<Category>(query);
            return new DataResult<IReadOnlyList<Category>>(result);
        }

        public async Task<DataResult<IReadOnlyList<Category>>> GetAllByParentId(IntModel model)
        {
            var query = $"SELECT * FROM {_categoryTable} " +
                        $"WHERE ParentId = @ParentId";

            var result = await _readDbConnection.QueryAsync<Category>(sql: query,
                                                                      param: new { ParentId = model.Value });

            return new DataResult<IReadOnlyList<Category>>(result);
        }

        public async Task<DataResult<IReadOnlyList<Category>>> GetAllWithProductsByParentId(IntModel model)
        {
            var query = $"SELECT c.*, p.Id AS ProductId, p.* FROM {_categoryTable} c " +
                        $"INNER JOIN {_productTable} p ON c.Id = p.CategoryId" +
                        $"WHERE c.ParentId = @ParentId";

            var categoryDictionary = new Dictionary<int, Category>();

            var result = await _dbContext.Connection.QueryAsync<Category, Product, Category>(query, (category,product) =>
            {
                Category? categoryEntry;

                if (!categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                {
                    categoryEntry = category;
                    categoryEntry.Products = new List<Product>();
                    categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                }
                if (product != null)
                    categoryEntry.Products.Add(product);

                return categoryEntry;
            }, splitOn: "ProductId", param: new { ParentId = model.Value });

            var filteredResult = result.DistinctBy(c => c.Id).ToList();
            return new DataResult<IReadOnlyList<Category>>(filteredResult);
        }

        public async Task<DataResult<Category>> GetAsync(IntModel model)
        {
            var query = $"SELECT * FROM {_categoryTable} " +
                        $"WHERE Id = @Id";

            var result = await _readDbConnection.QuerySingleOrDefaultAsync<Category>(sql: query, param: new { Id = model.Value });
            return new DataResult<Category>(result);
        }

        public async Task<DataResult<Category>> GetWithProducts(IntModel model)
        {
            var query = $"SELECT c.*, p.Id AS ProductId, p.* FROM {_categoryTable} c " +
            $"INNER JOIN {_productTable} p ON c.Id = p.CategoryId" +
            $"WHERE c.Id = @Id";

            var categoryDictionary = new Dictionary<int, Category>();

            var result = await _dbContext.Connection.QueryAsync<Category, Product, Category>(query, (category, product) =>
            {
                Category? categoryEntry;

                if (!categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                {
                    categoryEntry = category;
                    categoryEntry.Products = new List<Product>();
                    categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                }
                if (product != null)
                    categoryEntry.Products.Add(product);

                return categoryEntry;
            }, splitOn: "ProductId", param: new { Id = model.Value });

            var filteredResult = result.DistinctBy(c => c.Id).FirstOrDefault();
            return new DataResult<Category>(filteredResult);
        }

        public async Task<DataResult<Category>> GetByName(StringModel model)
        {
            var query = $"SELECT * FROM {_categoryTable} " +
                        $"WHERE Name = @Name";

            var result = await _readDbConnection.QuerySingleOrDefaultAsync<Category>(sql: query, param: new { Name = model.Value });
            return new DataResult<Category>(result);
        }

        public async Task<DataResult<Category>> GetByNameWithProducts(StringModel model)
        {
            var query = $"SELECT c.*, p.Id AS ProductId, p.* FROM {_categoryTable} c " +
                        $"INNER JOIN {_productTable} p ON c.Id = p.CategoryId" +
                        $"WHERE c.Name = @Name";

            var categoryDictionary = new Dictionary<int, Category>();

            var result = await _dbContext.Connection.QueryAsync<Category, Product, Category>(query, (category, product) =>
            {
                Category? categoryEntry;

                if (!categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                {
                    categoryEntry = category;
                    categoryEntry.Products = new List<Product>();
                    categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                }
                if (product != null)
                    categoryEntry.Products.Add(product);

                return categoryEntry;
            }, splitOn: "ProductId", param: new { Name = model.Value });

            var filteredResult = result.DistinctBy(c => c.Id).FirstOrDefault();
            return new DataResult<Category>(filteredResult);
        }
    }
}
