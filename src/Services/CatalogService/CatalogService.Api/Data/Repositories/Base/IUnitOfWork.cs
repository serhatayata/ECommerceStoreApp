using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

namespace CatalogService.Api.Data.Repositories.Base
{
    public interface IUnitOfWork
    {
        IDapperBrandRepository DapperBrandRepository { get; }
        IDapperCategoryRepository DapperCategoryRepository { get; }
        IDapperCommentRepository DapperCommentRepository { get; }
        IDapperFeatureRepository DapperFeatureRepository { get; }
        IDapperProductRepository DapperProductRepository { get; }

        IEfBrandRepository EfBrandRepository { get; }
        IEfCategoryRepository EfCategoryRepository { get; }
        IEfCommentRepository EfCommentRepository { get; }
        IEfFeatureRepository EfFeatureRepository { get; }
        IEfProductRepository EfProductRepository { get; }
    }
}
