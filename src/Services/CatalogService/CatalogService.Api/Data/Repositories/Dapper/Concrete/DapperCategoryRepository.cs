using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

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

        public Task<Result> AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IReadOnlyList<Category>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IReadOnlyList<Category>>> GetAllByParentId()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<Category>> GetAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<Category>> GetByName()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
